using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;
using StudentAssistant.DbLayer.Models.ImportData;


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

        public IEnumerable<CourseScheduleDatabaseModel> GetFromExcelFileByParameters(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new ArgumentNullException(nameof(input));

                var courseScheduleDatabaseModels = _importDataExcelService
                    .GetCourseScheduleDatabaseModels(input.FileName);

                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var filterByNameOfWeek = courseScheduleDatabaseModels
                    .Where(w => string.Equals(w.NameOfDayWeek, input.NameOfDayWeek,
                        StringComparison.InvariantCultureIgnoreCase));

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                var filterByParameters = filterByNameOfWeek
                    .Where(w => (w.NumberWeek != null
                                 && w.NumberWeek.Any(s=>s.NumberWeek == input.NumberWeek))
                                || (w.NumberWeek == null || w.NumberWeek.Count == 0)
                                && w.ParityWeek == input.ParityWeek).ToArray();

                // фильтруем по группе
                var filterByGroup =
                    filterByParameters.Where(w => string.Equals(w.StudyGroupModel.Name, input.GroupName));

                var result = new List<StudyGroupModel>();

                // добавляем в модель с расписанием комбинированные пары с другими группами
                foreach (var courseScheduleDatabaseModel in filterByGroup)
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
                }

                return filterByGroup;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }
    }
}