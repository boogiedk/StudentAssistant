using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using StudentAssistant.Backend.Services;


namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleDataService : ICourseScheduleDataService
    {
        private readonly CourseScheduleDataServiceConfigurationModel _courseScheduleDataServiceConfigurationModel;
        private readonly IImportDataExcelService _importDataExcelService;

        public CourseScheduleDataService(IOptions<CourseScheduleDataServiceConfigurationModel> courseScheduleDataServiceConfigurationModel, IImportDataExcelService importDataExcelService)
        {
            _importDataExcelService = importDataExcelService;
            _courseScheduleDataServiceConfigurationModel = courseScheduleDataServiceConfigurationModel.Value;
        }

        public List<CourseScheduleDatabaseModel> GetCourseScheduleFromJsonFile(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new NullReferenceException("Отсутствуют входные параметры.");

                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var courseScheduleModel = _courseScheduleDataServiceConfigurationModel
                    .ListCourseScheduleDatabaseModel.Where(w => w.NameOfDayWeek == input.NameOfDayWeek);

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                courseScheduleModel = courseScheduleModel.Where(w => (w.NumberWeek != null
                                                                      && w.NumberWeek.Contains(input.NumberWeek)) 
                || (w.NumberWeek == null) && w.ParityWeek == input.ParityWeek);


                return courseScheduleModel.ToList();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }
        }

        public List<CourseScheduleDatabaseModel> GetCourseScheduleFromExcelFile(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new NullReferenceException("Отсутствуют входные параметры.");

                // производим импорт данных из Excel файла
                var importDataExcelModels = _importDataExcelService.LoadExcelFile();

                // маппим модель импорта в модель бд
                var courseScheduleDatabaseModel =
                    _importDataExcelService
                        .PrepareImportDataExcelModelToDatabaseModel(importDataExcelModels);

                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var courseScheduleModel = courseScheduleDatabaseModel
                    .Where(w => w.NameOfDayWeek == input.NameOfDayWeek);

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                courseScheduleModel = courseScheduleModel
                    .Where(w => (w.NumberWeek != null 
                                 && w.NumberWeek.Contains(input.NumberWeek))
                                 || (w.NumberWeek == null || w.NumberWeek.Count==0) 
                                 && w.ParityWeek == input.ParityWeek);


                return courseScheduleModel.ToList();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }

        }
    }
}
