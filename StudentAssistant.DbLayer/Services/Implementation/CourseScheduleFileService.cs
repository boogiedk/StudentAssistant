using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleFileService : ICourseScheduleFileService
    {
        private readonly IImportDataExcelService _importDataExcelService;

        public CourseScheduleFileService(
            IImportDataExcelService importDataExcelService)
        {
            _importDataExcelService =
                importDataExcelService ?? throw new ArgumentNullException(nameof(importDataExcelService));
        }

        public async Task<List<CourseScheduleDatabaseModel>> GetFromExcelFile(string fileName)
        {
            var result = Task.Run(() =>
            {
                var courseScheduleDatabaseModels = _importDataExcelService
                    .GetCourseScheduleDatabaseModels(fileName).ToList();

                return courseScheduleDatabaseModels;
            });

            return await result;
        }

        public async Task<List<ExamScheduleDatabaseModel>> GetExamScheduleFromExcelFile(string fileName)
        {
            var result = Task.Run(() =>
            {
                var courseScheduleDatabaseModels = _importDataExcelService
                    .GetExamScheduleDatabaseModels(fileName).ToList();

                return courseScheduleDatabaseModels;
            });

            return await result;
        }

        // добавляем в модель с расписанием комбинированные пары с другими группами
        /*foreach (var courseScheduleDatabaseModel in filterByGroup)
        {
            foreach (var scheduleDatabaseModel in filterByParameters)
            {
                if (courseScheduleDatabaseModel.CoursePlace == scheduleDatabaseModel.CoursePlace
                    && courseScheduleDatabaseModel.CourseNumber == scheduleDatabaseModel.CourseNumber
                    && courseScheduleDatabaseModel.StudyGroupModel.Name !=
                    scheduleDatabaseModel.StudyGroupModel.Name)
                {
                    result.Add(scheduleDatabaseModel.StudyGroupModel);

                    courseScheduleDatabaseModel.CombinedGroup = result.Distinct().ToList();
                }
            }
        }*/
    }
}