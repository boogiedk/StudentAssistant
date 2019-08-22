using System;
using System.Collections.Generic;
using System.Linq;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services.Interfaces;


namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleFileService : ICourseScheduleFileService
    {
        private readonly IImportDataExcelService _importDataExcelService;
        private readonly IImportDataJsonService _importDataJsonService;

        public CourseScheduleFileService(
            IImportDataExcelService importDataExcelService,
            IImportDataJsonService importDataJsonService
            )
        {
            _importDataExcelService = importDataExcelService ?? throw new ArgumentNullException(nameof(importDataExcelService));
            _importDataJsonService = importDataJsonService ?? throw new ArgumentNullException(nameof(importDataJsonService));
        }

        public IEnumerable<CourseScheduleDatabaseModel> GetFromJsonFileByParameters(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new ArgumentNullException(nameof(input));

                // все данные из расписания
                var courseScheduleDatabaseModels = _importDataJsonService.GetCourseScheduleDatabaseModels();

                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var filterByNameOfWeek = courseScheduleDatabaseModels
                    .Where(w => string.Equals(w.NameOfDayWeek, input.NameOfDayWeek, StringComparison.InvariantCultureIgnoreCase));

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                var filterByParameters = filterByNameOfWeek
                    .Where(w => (w.NumberWeek != null
                                 && w.NumberWeek.Contains(input.NumberWeek))
                                || (w.NumberWeek == null || w.NumberWeek.Count == 0)
                                && w.ParityWeek == input.ParityWeek).ToArray();

                // фильтруем по группе
                var filterByGroup = filterByParameters.Where(w => string.Equals(w.GroupName, input.GroupName));

                return filterByGroup;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public IEnumerable<CourseScheduleDatabaseModel> GetFromExcelFileByParameters(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new ArgumentNullException(nameof(input));

                var courseScheduleDatabaseModels =
                    _importDataExcelService
                        .GetCourseScheduleDatabaseModels();

                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var filterByNameOfWeek = courseScheduleDatabaseModels
                    .Where(w => string.Equals(w.NameOfDayWeek, input.NameOfDayWeek, StringComparison.InvariantCultureIgnoreCase));

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                var filterByParameters = filterByNameOfWeek
                    .Where(w => (w.NumberWeek != null
                                 && w.NumberWeek.Contains(input.NumberWeek))
                                 || (w.NumberWeek == null || w.NumberWeek.Count == 0)
                                 && w.ParityWeek == input.ParityWeek).ToArray();

                // фильтруем по группе
               var filterByGroup = filterByParameters.Where(w => string.Equals(w.GroupName, input.GroupName));

                return filterByGroup;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }
    }
}
