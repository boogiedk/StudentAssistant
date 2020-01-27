using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;
using StudentAssistant.DbLayer.Models.ImportData;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class ImportDataExcelService : IImportDataExcelService
    {
        private readonly ILogger<ImportDataExcelService> _logger;

        public ImportDataExcelService(ILogger<ImportDataExcelService> logger)
        {
            _logger = logger;
        }

        private IEnumerable<ImportDataExcelModel> ParseExcelFileForThreeGroup(string fileName)
        {
            try
            {
                _logger.LogInformation("ParseExcelFileForThreeGroup: Start parse Excel file.");

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

                                model.CourseName = sheet.GetRow(secondIterator).GetCell(5 + sixIterator)
                                    ?.StringCellValue;
                                model.CourseType = sheet.GetRow(secondIterator).GetCell(6 + sixIterator)
                                    ?.StringCellValue;
                                model.TeacherFullName = sheet.GetRow(secondIterator).GetCell(7 + sixIterator)
                                    ?.StringCellValue;

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

                _logger.LogInformation("ParseExcelFileForThreeGroup: End parse Excel file. Object count: " +
                                       importDataExcelModels.Count);
                return importDataExcelModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("ParseExcelFileForThreeGroup: Exception when parse excel file. " + ex);
                throw new NotSupportedException();
            }
        }


        private IEnumerable<CourseScheduleDatabaseModel> PrepareImportDataExcelModelToDatabaseModel(
            IEnumerable<ImportDataExcelModel> importDataExcelModels)
        {
            try
            {
                var studyGroups = importDataExcelModels.GroupBy(g => g.GroupName)
                    .Select(y => y.First())
                    .Select(s => new StudyGroupModel
                    {
                        Id = Guid.NewGuid(),
                        Name = ParseGroupName(s.GroupName)
                    })
                    .ToList();

                var teachers = importDataExcelModels.GroupBy(g => g.TeacherFullName)
                    .Select(y => y.First())
                    .Select(s => new TeacherModel
                    {
                        Id = Guid.NewGuid(),
                        FullName = s.TeacherFullName
                    })
                    .ToList();

                var result = importDataExcelModels.Select(importDataExcelModel => new CourseScheduleDatabaseModel
                    {
                        Id = Guid.NewGuid(),
                        CoursePlace = importDataExcelModel.CoursePlace,
                        CourseName = PrepareCourseName(importDataExcelModel.CourseName),
                        CourseNumber = (int) importDataExcelModel.CourseNumber,
                        CourseType = ParseCourseType(importDataExcelModel.CourseType),
                        NameOfDayWeek = importDataExcelModel.DayOfTheWeek,

                        NumberWeek = ParseNumberWeek(importDataExcelModel.CourseName),
                        NumberWeekString = String.Join(",",
                            ParseNumberWeek(importDataExcelModel.CourseName).Select(p => p.ToString())),

                        ParityWeek = ParseParityWeek(importDataExcelModel.ParityWeek),
                        TeacherModel = teachers.FirstOrDefault(w => w.FullName == importDataExcelModel.TeacherFullName),
                        StudyGroupModel = studyGroups.FirstOrDefault(w => string.Equals(w.Name,ParseGroupName(importDataExcelModel.GroupName))),
                        StartOfClasses = importDataExcelModel.StartOfClasses,
                        EndOfClasses = importDataExcelModel.EndOfClasses,
                        DateTimeCreate = DateTimeOffset.UtcNow,
                        DateTimeUpdate = DateTimeOffset.UtcNow,
                        IsDeleted = false,
                        //TODO: исправить
                        Version = DateTimeOffset.UtcNow.Minute.ToString()
                    })
                    .ToList();


                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("PrepareImportDataExcelModelToDatabaseModel: Exception when prepare excel file. " +
                                 ex);
                throw new NotSupportedException();
            }
        }

        public IEnumerable<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels(string fileName)
        {
            var importDataExcelModels = ParseExcelFileForThreeGroup(fileName);

            var courseScheduleDatabaseModel =
                PrepareImportDataExcelModelToDatabaseModel(importDataExcelModels);


            return courseScheduleDatabaseModel;
        }

        public IEnumerable<ExamScheduleDatabaseModel> GetExamScheduleDatabaseModels(string fileName)
        {
            var importDataExcelModels = ParseExcelFileWithExams(fileName);

            var examScheduleDatabaseModel =
                PrepareExcelFileWithExamsModelToDatabaseModel(importDataExcelModels);

            return examScheduleDatabaseModel;
        }

        private IEnumerable<ExamScheduleExcelModel> ParseExcelFileWithExams(string fileName)
        {
            try
            {
                // _logger.LogInformation("ParseExcelFileWithExams: Start parse Excel file.");

                var file = new FileInfo(fileName);

                var importDataExcelModels = new List<ExamScheduleExcelModel>();

                if (file.Length > 0)
                {
                    using (var stream = new FileStream(fileName, FileMode.Open))
                    {
                        stream.Position = 0;

                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);

                        //Excel format  
                        var sheet = hssfwb.GetSheetAt(0);

                        var firstIterator = 0;

                        var secondIterator = 0;

                        var thirdIterator = 0;

                        var fourthIterator = 0;

                        Func<(string, bool)> getCellValue = () =>
                        {
                            string cellValue;
                            bool isNumeric;

                            if (sheet.GetRow(2 + firstIterator).GetCell(2).CellType ==
                                CellType.Numeric) // проверяем тип ячейки
                            {
                                cellValue = sheet.GetRow(2 + firstIterator).GetCell(2).NumericCellValue
                                    .ToString(CultureInfo.InvariantCulture); // если нумерик, то достаем double

                                isNumeric = true;
                            }
                            else
                            {
                                cellValue = sheet.GetRow(2 + firstIterator).GetCell(2)
                                    ?.StringCellValue; // если строковая, то достаем string

                                isNumeric = false;
                            }

                            return (cellValue, isNumeric);
                        };

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 23; i++)
                            {
                                var model = new ExamScheduleExcelModel
                                {
                                    Month = sheet.GetRow(2).GetCell(1)?.StringCellValue,

                                    StartOfClasses = sheet.GetRow(2 + firstIterator).GetCell(4 + fourthIterator)
                                        ?.StringCellValue,
                                    CoursePlace = sheet.GetRow(2 + firstIterator).GetCell(5 + fourthIterator)
                                        ?.StringCellValue,

                                    CourseType = sheet.GetRow(2 + 0 + secondIterator).GetCell(3 + fourthIterator)
                                        ?.StringCellValue,
                                    CourseName = sheet.GetRow(2 + 1 + secondIterator).GetCell(3 + fourthIterator)
                                        ?.StringCellValue,
                                    TeacherFullName =
                                        sheet.GetRow(2 + 2 + secondIterator).GetCell(3 + fourthIterator)
                                            ?.StringCellValue,

                                    GroupName = sheet.GetRow(1 + thirdIterator).GetCell(3 + fourthIterator)
                                        ?.StringCellValue,
                                };

                                var (cellValue, isNumeric) = getCellValue.Invoke();

                                model.Date = cellValue;

                                if (isNumeric)
                                {
                                    firstIterator++;
                                    secondIterator++;
                                    continue;
                                }

                                importDataExcelModels.Add(model);

                                firstIterator += 3;
                                secondIterator += 3;
                            }

                            fourthIterator += 3;
                            firstIterator = 0;
                            secondIterator = 0;
                        }
                    }
                }

                return importDataExcelModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("ParseExcelFileWithExams: Exception when parse excel file. " + ex);
                throw new NotSupportedException();
            }
        }

        /*
                /// <summary>
                /// Создает Json файл с данными из Excel.
                /// </summary>
                /// <param name="importDataExcelModels"></param>
                private void CreateJsonFileForConfig(IEnumerable<ImportDataExcelModel> importDataExcelModels)
                {
                    string fileName = Path.Combine("Infrastructure", "ScheduleFile", "output.json");

                    var result = PrepareImportDataExcelModelToDatabaseModel(importDataExcelModels);

                    string jsonData = JsonConvert.SerializeObject(result);

                    File.WriteAllText(fileName, jsonData);
                }
        */

        public IEnumerable<ExamScheduleDatabaseModel> PrepareExcelFileWithExamsModelToDatabaseModel(
            IEnumerable<ExamScheduleExcelModel> input)
        {
            try
            {
                var studyGroups = input.GroupBy(g => g.GroupName)
                    .Select(y => y.First())
                    .Select(s => new StudyGroupModel
                    {
                        Id = Guid.NewGuid(),
                        Name = ParseGroupName(s.GroupName)
                    })
                    .ToList();

                var teachers = input.GroupBy(g => g.TeacherFullName)
                    .Select(y => y.First())
                    .Select(s => new TeacherModel
                    {
                        Id = Guid.NewGuid(),
                        FullName = s.TeacherFullName
                    })
                    .ToList();
                
                var result = input.Select(importDataExcelModel => new ExamScheduleDatabaseModel
                    {
                        Id = Guid.NewGuid(),
                        CoursePlace = importDataExcelModel.CoursePlace,
                        CourseName = importDataExcelModel.CourseName,
                        CourseType = ParseCourseType(importDataExcelModel.CourseType),
                        TeacherModel = teachers.FirstOrDefault(w => w.FullName == importDataExcelModel.TeacherFullName),
                        StudyGroupModel = studyGroups.FirstOrDefault(w => string.Equals(w.Name,ParseGroupName(importDataExcelModel.GroupName))),
                        StartOfClasses = importDataExcelModel.StartOfClasses,
                        NumberDate = PrepareDate(importDataExcelModel.Date).Item1, // число
                        DayOfWeek = PrepareDate(importDataExcelModel.Date).Item2, // день недели 
                        Month = importDataExcelModel?.Month.Replace(" ", ""),
                        
                        DateTimeCreate = DateTimeOffset.UtcNow,
                        DateTimeUpdate = DateTimeOffset.UtcNow,
                        IsDeleted = false,
                        //TODO: исправить
                        Version = DateTimeOffset.UtcNow.Minute.ToString()
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("PrepareExcelFileWithExamsModelToDatabaseModel: Exception when prepare excel file. " +
                                 ex);
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Парсит строку с группой и возвращает название группы.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private string ParseGroupName(string groupName)
        {
            if (groupName == null) // БББО-01-16 (КБ-1)10.03.01
            {
                return string.Empty;
            }

            var groupNameResult = groupName.Split(' ')[0];

            return groupNameResult; // БББО-01-16
        }

        /// <summary>
        /// Парсит строку с датой и возвращает кортеж с числом и днем недели.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public (string, string) PrepareDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return (string.Empty, string.Empty);
            }

            var dateArray = date.Split(" ");

            var numberDate = dateArray[0];

            var dayOfWeek = dateArray[1];

            return (numberDate, dayOfWeek);
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

            switch (courseType.ToLower())
            {
                case "пр":
                    return CourseType.Practicte;
                case "лр":
                    return CourseType.LaboratoryWork;
                case "лб":
                    return CourseType.LaboratoryWork;
                case "лек":
                    return CourseType.Lecture;
                case "зач":
                    return CourseType.ControlCourse;
                case "консультация":
                    return CourseType.СonsultationCourse;
                case "экзамен":
                    return CourseType.ExamCourse;
                case "лк":
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
        public List<int> ParseNumberWeek(string numberWeek)
        {
            try
            {
                if (string.IsNullOrEmpty(numberWeek))
                {
                    return new List<int>();
                }

                var stringNumbers = numberWeek.Split('н')[0];

                if (stringNumbers[0] == 'к')
                {
                    return new List<int>();
                }

                var numbers = new List<int>();

                if (IsNumberContains(stringNumbers))
                {
                    numbers = stringNumbers.Split(',').ToList().Select(int.Parse)
                        .Where(n => n != 0).ToList();
                }


                return numbers;
            }
            catch (Exception)
            {
                //exception handler
            }

            return new List<int>();
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