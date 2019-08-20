using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.ImportData;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class ImportDataExcelService : IImportDataExcelService
    {
        private List<ImportDataExcelModel> LoadExcelFile()
        {
            var fileName = Path.Combine("Infrastructure", "ScheduleFile", "scheduleFile.xlsx");

            var file = new FileInfo(fileName);

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

                    // Итератор группы
                    var fiveIterator = 0;

                    // Итератор расписания группы
                    var sixIterator = 0;

                    for (int j = 0; j < 3; j++)
                    {

                        for (int i = 3; i < 75; i++)
                        {
                            var model = new ImportDataExcelModel()
                            {
                                DayOfTheWeek = sheet.GetRow(fourIterator).GetCell(0).StringCellValue?.ToLower(),
                                GroupName = sheet.GetRow(1).GetCell(5 + fiveIterator)?.StringCellValue,
                            };

                            if (j != 0)
                            {
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
                            }

                            model.CourseName = sheet.GetRow(secondIterator).GetCell(5 + sixIterator)?.StringCellValue;
                            model.CourseType = sheet.GetRow(secondIterator).GetCell(6 + sixIterator)?.StringCellValue;
                            model.TeacherFullName = sheet.GetRow(secondIterator).GetCell(7 + sixIterator)?.StringCellValue;

                            model.CoursePlace =
                                sheet.GetRow(secondIterator).GetCell(8 + sixIterator).CellType ==
                                CellType.Numeric // проверяем тип ячейки

                                    ? sheet.GetRow(secondIterator).GetCell(8 + sixIterator).NumericCellValue
                                        .ToString(CultureInfo.InvariantCulture) // если нумерик, то достаем double

                                    : sheet.GetRow(secondIterator).GetCell(8 + sixIterator)
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

                        sixIterator += 8;
                    }
                }
            }

            // Создает Json файл с расписанием
            // CreateJsonFileForConfig(importDataExcelModels);

            return importDataExcelModels;
        }


        private List<ImportDataExcelModel> LoadExcelFileThreeGroup()
        {
            var fileName = Path.Combine("Infrastructure", "ScheduleFile", "scheduleFile.xlsx");

            var file = new FileInfo(fileName);

            var importDataExcelModels = new List<ImportDataExcelModel>();

            if (file.Length > 0)
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    stream.Position = 0;

                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007+
                    //Excel format  
                    var sheet = hssfwb.GetSheetAt(0);

                    // Итератор группы
                    var fiveIterator = 0;

                    // Итератор расписания группы
                    var sixIterator = 0;

                    for (int j = 0; j < 3; j++)
                    {
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
                                GroupName = sheet.GetRow(1).GetCell(5 + fiveIterator)?.StringCellValue,
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

                            model.CourseName = sheet.GetRow(secondIterator).GetCell(5 + sixIterator)?.StringCellValue;
                            model.CourseType = sheet.GetRow(secondIterator).GetCell(6 + sixIterator)?.StringCellValue;
                            model.TeacherFullName = sheet.GetRow(secondIterator).GetCell(7 + sixIterator)?.StringCellValue;

                            model.CoursePlace =
                                sheet.GetRow(secondIterator).GetCell(8 + sixIterator).CellType ==
                                CellType.Numeric // проверяем тип ячейки

                                    ? sheet.GetRow(secondIterator).GetCell(8 + sixIterator).NumericCellValue
                                        .ToString(CultureInfo.InvariantCulture) // если нумерик, то достаем double

                                    : sheet.GetRow(secondIterator).GetCell(8 + sixIterator)
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
                        sixIterator += 4;
                        fiveIterator += 4;
                    }
                }
            }

            // Создает Json файл с расписанием
            // CreateJsonFileForConfig(importDataExcelModels);

            return importDataExcelModels;
        }


        private List<CourseScheduleDatabaseModel> PrepareImportDataExcelModelToDatabaseModel(List<ImportDataExcelModel> importDataExcelModels)
        {
            var resultList = new List<CourseScheduleDatabaseModel>();

            foreach (var importDataExcelModel in importDataExcelModels)
            {
                var model = new CourseScheduleDatabaseModel
                {
                    CoursePlace = importDataExcelModel.CoursePlace,
                    CourseName = PrepareCourseName(importDataExcelModel.CourseName),
                    CourseNumber = (int)importDataExcelModel.CourseNumber,
                    CourseType = ParseCourseType(importDataExcelModel.CourseType),
                    NameOfDayWeek = importDataExcelModel.DayOfTheWeek,
                    NumberWeek = ParseNumberWeek(importDataExcelModel.CourseName),
                    ParityWeek = ParseParityWeek(importDataExcelModel.ParityWeek),
                    TeacherFullName = importDataExcelModel.TeacherFullName,
                    GroupName = ParseGroupName(importDataExcelModel.GroupName),
                    StartOfClasses = importDataExcelModel.StartOfClasses,
                    EndOfClasses = importDataExcelModel.EndOfClasses
                };

                resultList.Add(model);
            }

            return resultList;
        }

        private string ParseGroupName(string groupName) // БББО-01-16 (КБ-1)10.03.01
        {
            if (groupName == null)
            {
                return string.Empty;
            }

            var groupNameResult = groupName.Split(' ')[0];

            return groupNameResult;
        }

        public List<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels()
        {
            var importDataExcelModels = LoadExcelFileThreeGroup(); //LoadExcelFile();

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
            string fileName = Path.Combine("Infrastructure", "ScheduleFile", "output.json");

            var result = PrepareImportDataExcelModelToDatabaseModel(importDataExcelModels);

            string jsonData = JsonConvert.SerializeObject(result);

            File.WriteAllText(fileName, jsonData);
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

