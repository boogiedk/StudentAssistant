using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StudentAssistant.DbLayer.Models.CourseSchedule;


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

        public List<CourseScheduleDatabaseModel> GetCourseScheduleFromJsonFileByParameters(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new ArgumentNullException(nameof(input));

                // все данные из расписания
                var courseScheduleDatabaseModels = _importDataJsonService.GetCourseScheduleDatabaseModels();

                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var courseScheduleModel = courseScheduleDatabaseModels
                    .Where(w => string.Equals(w.NameOfDayWeek, input.NameOfDayWeek, StringComparison.InvariantCultureIgnoreCase));

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                courseScheduleModel = courseScheduleModel.Where(w => (w.NumberWeek != null
                                                                      && w.NumberWeek.Contains(input.NumberWeek))
                || (w.NumberWeek == null) && w.ParityWeek == input.ParityWeek);


                return courseScheduleModel.ToList();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public List<CourseScheduleDatabaseModel> GetCourseScheduleFromExcelFileByParameters(CourseScheduleParameters input)
        {
            try
            {
                //TODO: отрефакторить метод
                if (input == null) throw new ArgumentNullException(nameof(input));

                // маппим модель импорта в модель бд
                var courseScheduleDatabaseModels =
                    _importDataExcelService
                        .GetCourseScheduleDatabaseModels();


                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var scheduleDatabaseModels = courseScheduleDatabaseModels
                    .Where(w=>string.Equals(w.GroupName,input.GroupName)) 
                    .Where(w => string.Equals(w.NameOfDayWeek, input.NameOfDayWeek, StringComparison.InvariantCultureIgnoreCase));

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                scheduleDatabaseModels = scheduleDatabaseModels
                    .Where(w => (w.NumberWeek != null
                                 && w.NumberWeek.Contains(input.NumberWeek))
                                 || (w.NumberWeek == null || w.NumberWeek.Count == 0)
                                 && w.ParityWeek == input.ParityWeek);

                return scheduleDatabaseModels.ToList();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }
        }

        public List<CourseScheduleDatabaseModel> GetFromExcel()
        {
            try
            {
                var courseScheduleDatabaseModel =
                    _importDataExcelService
                        .GetCourseScheduleDatabaseModels();

                return courseScheduleDatabaseModel.ToList();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения." + ex);
            }

        }
    }
}
