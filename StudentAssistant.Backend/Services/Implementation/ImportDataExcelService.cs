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
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;
using StudentAssistant.Backend.Models.ImportData;
using Excel = Microsoft.Office.Interop.Excel;


namespace StudentAssistant.Backend.Services.Implementation
{
    public class ImportDataExcelService : IImportDataExcelService
    {

        public ImportDataExcelService()
        {

        }


        public void LoadExcelFile()
        {
            string fileName = @"Infrastructure\ScheduleFile\file.xlsx";
            FileInfo file = new FileInfo(fileName);

            if (file.Length > 0)
            {
                ISheet sheet;

                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    stream.Position = 0;

                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

                    #region Тест

                    //var scheduleList = new List<ImportDataExcelModel>
                    //{
                    //    #region Понедельник
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(3).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(3).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(3).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(3).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(3).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(3).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(3).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(3).GetCell(8).StringCellValue,
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(3).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(3).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(3).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(4).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(4).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(4).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(4).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(4).GetCell(8).StringCellValue,
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(5).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(5).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(5).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(5).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(5).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(5).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(5).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(5).GetCell(8).NumericCellValue.ToString(),
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(5).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(5).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(5).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(6).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(6).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(6).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(6).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(6).GetCell(8).StringCellValue
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(7).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(7).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(7).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(7).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(7).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(7).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(7).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(7).GetCell(8).NumericCellValue.ToString()
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(7).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(7).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(7).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(8).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(8).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(8).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(8).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(8).GetCell(8).StringCellValue
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(9).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(9).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(9).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(9).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(9).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(9).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(9).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(9).GetCell(8).StringCellValue
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(9).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(9).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(9).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(10).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(10).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(10).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(10).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(10).GetCell(8).StringCellValue
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(11).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(11).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(11).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(11).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(11).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(11).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(11).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(11).GetCell(8).NumericCellValue.ToString()
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(11).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(11).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(11).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(12).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(12).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(12).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(12).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(12).GetCell(8).StringCellValue
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(13).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(13).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(13).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(13).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(13).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(13).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(13).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(13).GetCell(8).StringCellValue
                    //    },
                    //    new ImportDataExcelModel()
                    //    {
                    //        GroupName = sheet.GetRow(1).GetCell(5).StringCellValue,

                    //        DayOfTheWeek = sheet.GetRow(3).GetCell(0).StringCellValue,
                    //        CourseNumber = sheet.GetRow(13).GetCell(1).NumericCellValue,
                    //        StartOfClasses = sheet.GetRow(13).GetCell(2).StringCellValue,
                    //        EndOfClasses = sheet.GetRow(13).GetCell(3).StringCellValue,

                    //        ParityWeek = sheet.GetRow(14).GetCell(4).StringCellValue,
                    //        CourseName = sheet.GetRow(14).GetCell(5).StringCellValue,
                    //        CourseType = sheet.GetRow(14).GetCell(6).StringCellValue,
                    //        TeacherFullName = sheet.GetRow(14).GetCell(7).StringCellValue,
                    //        CoursePlace = sheet.GetRow(14).GetCell(8).StringCellValue
                    //    },
                    //    #endregion
                    //};

                    #endregion

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

                    var importDataExcelModels = new List<ImportDataExcelModel>();

                    for (int i = 3; i < 75; i++)
                    {

                        var model = new ImportDataExcelModel()
                        {
                            DayOfTheWeek = sheet.GetRow(fourIterator).GetCell(0).StringCellValue,
                            GroupName = sheet.GetRow(1).GetCell(5)?.StringCellValue,
                        };

                        if (firstIterator == 2)
                        {
                            model.CourseNumber = sheet.GetRow(i + firstIterator).GetCell(1).NumericCellValue;
                            model.StartOfClasses = sheet.GetRow(i + firstIterator).GetCell(2)?.StringCellValue;
                            model.EndOfClasses = sheet.GetRow(i + firstIterator).GetCell(3)?.StringCellValue;

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

                        model.CoursePlace = sheet.GetRow(secondIterator).GetCell(8).CellType == CellType.Numeric // проверяем тип ячейки

                            ? sheet.GetRow(secondIterator).GetCell(8).NumericCellValue
                                .ToString(CultureInfo.InvariantCulture) // если нумерик, то достаем double

                            : sheet.GetRow(secondIterator).GetCell(8)?.StringCellValue // если строковая, то достаем string

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
        }
    }
}

