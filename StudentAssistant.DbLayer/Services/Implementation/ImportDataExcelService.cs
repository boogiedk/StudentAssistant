using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.ImportData;
using StudentAssistant.DbLayer.Services;


namespace StudentAssistant.Backend.Services.Implementation
{
    public class ImportDataExcelService : IImportDataExcelService
    {
        public List<ImportDataExcelModel> LoadExcelFile()
        {
            string fileName = @"Infrastructure\ScheduleFile\scheduleFile.xlsx";

            FileInfo file = new FileInfo(fileName);

            var importDataExcelModels = new List<ImportDataExcelModel>();

            if (file.Length > 0)
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    stream.Position = 0;

                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007+
                                                                    //Excel format  
                    var sheet = hssfwb.GetSheetAt(0);

                    // Итератор перехода для номера предмета, начала и
                    // конца занятий внутри одного номера.
                    var firstIterator = 0;

                    // Итератор для четности, названия предмета, типа предмета.
                    // имени преподавателя и кабинета.
                    var secondIterator = 3;

                    // Итератор для номера предмета, начала и конца занятий.
                    var thirdIterator = 3;

                    // Итератор для дня недели.
                    var fourIterator = 3;

                    for (int i = 3; i < 75; i++)
                    {

                        var model = new ImportDataExcelModel()
                        {
                            DayOfTheWeek = sheet.GetRow(fourIterator).GetCell(0).StringCellValue?.ToLower(),
                            GroupName = sheet.GetRow(1).GetCell(5)?.StringCellValue,
                        };

                        if (firstIterator == 2)
                        {
                            model.CourseNumber = sheet.GetRow(i).GetCell(1).NumericCellValue;
                            model.StartOfClasses = sheet.GetRow(i).GetCell(2)?.StringCellValue;
                            model.EndOfClasses = sheet.GetRow(i).GetCell(3)?.StringCellValue;

                            firstIterator = 0;
                            thirdIterator += 2;
                        }
                        else
                        {
                            model.CourseNumber = sheet.GetRow(thirdIterator).GetCell(1).NumericCellValue;
                            model.StartOfClasses = sheet.GetRow(thirdIterator).GetCell(2)?.StringCellValue;
                            model.EndOfClasses = sheet.GetRow(thirdIterator).GetCell(3)?.StringCellValue;
                        }

                        model.ParityWeek = sheet.GetRow(secondIterator).GetCell(4)?.StringCellValue;
                        model.CourseName = sheet.GetRow(secondIterator).GetCell(5)?.StringCellValue;
                        model.CourseType = sheet.GetRow(secondIterator).GetCell(6)?.StringCellValue;
                        model.TeacherFullName = sheet.GetRow(secondIterator).GetCell(7)?.StringCellValue;

                        model.CoursePlace =
                            sheet.GetRow(secondIterator).GetCell(8).CellType == CellType.Numeric // проверяем тип ячейки

                                ? sheet.GetRow(secondIterator).GetCell(8).NumericCellValue
                                    .ToString(CultureInfo.InvariantCulture) // если нумерик, то достаем double

                                : sheet.GetRow(secondIterator).GetCell(8)
                                      ?.StringCellValue // если строковая, то достаем string

                                  ?? ""; // если ячейка null, присваиваем ""

                        importDataExcelModels.Add(model);

                        secondIterator++;
                        firstIterator++;

                        // каждые 12 итераций меняем итератор
                        // дня недели на 12 (след. день недели)
                        if (i == 14 || i == 26 || i == 38 || i == 50 || i == 62 || i == 74)
                        {
                            fourIterator += 12;
                        }
                    }
                }
            }

            // Создает Json файл с расписанием
            CreateJsonFileForConfig(importDataExcelModels);

            return importDataExcelModels;
        }

        private List<CourseScheduleDatabaseModel> PrepareImportDataExcelModelToDatabaseModel(List<ImportDataExcelModel> importDataExcelModels)
        {
            var resultList = new List<CourseScheduleDatabaseModel>();

            foreach (var importDataExcelModel in importDataExcelModels)
            {
                var model = new CourseScheduleDatabaseModel()
                {
                    CoursePlace = importDataExcelModel.CoursePlace,
                    CourseName = PrepareCourseName(importDataExcelModel.CourseName),
                    CourseNumber = (int)importDataExcelModel.CourseNumber,
                    CourseType = ParseCourseType(importDataExcelModel.CourseType),
                    NameOfDayWeek = importDataExcelModel.DayOfTheWeek,
                    NumberWeek = ParseNumberWeek(importDataExcelModel.CourseName),
                    ParityWeek = ParseParityWeek(importDataExcelModel.ParityWeek),
                    TeacherFullName = importDataExcelModel.TeacherFullName
                };

                resultList.Add(model);
            }

            return resultList;
        }

        public List<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels()
        {
            var importDataExcelModels = LoadExcelFile();

            var courseScheduleDatabaseModel = 
                PrepareImportDataExcelModelToDatabaseModel(importDataExcelModels);

            return courseScheduleDatabaseModel;
        }

        /// <summary>
        /// Создает Json файл с данными из Excel.
        /// </summary>
        /// <param name="importDataExcelModels"></param>
        private void CreateJsonFileForConfig(List<ImportDataExcelModel> importDataExcelModels)
        {
            string fileName = @"Infrastructure\ScheduleFile\";

            var result = PrepareImportDataExcelModelToDatabaseModel(importDataExcelModels);

            string jsonData = JsonConvert.SerializeObject(result);

            File.WriteAllText(fileName + "output.json", jsonData);
        }

        /// <summary>
        /// Парсит строку с четностью и возвращает true или false в зависимости от результата.
        /// </summary>
        /// <param name="parityWeek"></param>
        /// <returns></returns>
        private bool ParseParityWeek(string parityWeek)
        {
            if (string.IsNullOrEmpty(parityWeek))
            {
                return false;
            }

            if (parityWeek.Equals("I"))
            {
                return false;
            }

            if (parityWeek.Equals("II"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Парсит строку с типом предмета и возвращает CourseType в зависимости от результата.
        /// </summary>
        /// <param name="courseType"></param>
        /// <returns></returns>
        private CourseType ParseCourseType(string courseType)
        {
            if (string.IsNullOrEmpty(courseType))
            {
                return CourseType.Other;
            }

            switch (courseType)
            {
                case "пр":
                    return CourseType.Practicte;
                case "лр":
                    return CourseType.LaboratoryWork;
                case "лек":
                    return CourseType.Lecture;
                default:
                    return CourseType.Other;
            }
        }

        /// <summary>
        /// Парсит строку с номерами дней, в которые будет предмет.
        /// </summary>
        /// <param name="numberWeek"></param>
        /// <returns></returns>
        private List<int> ParseNumberWeek(string numberWeek)
        {
            if (string.IsNullOrEmpty(numberWeek))
            {
                return new List<int>();
            }

            var stringNumbers = numberWeek.Split('н')[0];

            List<int> numbers = new List<int>();

            if (IsNumberContains(stringNumbers))
            {
                numbers = stringNumbers
                       .Split(',').Select(int.Parse)
                       .Where(n => n != 0).ToList();
            }

            return numbers;
        }

        /// <summary>
        /// Определяет, содержит ли строка цифры.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsNumberContains(string input)
        {
            foreach (char c in input)
                if (Char.IsNumber(c))
                    return true;
            return false;
        }

        /// <summary>
        /// Парсит название предмета и возвращает значение предмета без номеров недель.
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        private string PrepareCourseName(string courseName)
        {
            if (string.IsNullOrEmpty(courseName))
            {
                return string.Empty;
            }

            var stringNumbers = courseName.Split('н')[0];

            if (IsNumberContains(stringNumbers))
            {
                var result = courseName.Remove(0, stringNumbers.Length + 1);

                return result;
            }

            return courseName;
        }
    }
}

