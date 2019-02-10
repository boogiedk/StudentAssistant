using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleDataService : ICourseScheduleDataService
    {
        private readonly CourseScheduleDataServiceConfigurationModel _courseScheduleDataServiceConfigurationModel;

        public CourseScheduleDataService(IOptions<CourseScheduleDataServiceConfigurationModel> courseScheduleDataServiceConfigurationModel)
        {
            _courseScheduleDataServiceConfigurationModel = courseScheduleDataServiceConfigurationModel.Value;
        }

        public List<CourseScheduleDatabaseModel> GetCourseScheduleFromDatabase(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new NullReferenceException("Отсутствуют входные параметры.");

                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var courseScheduleModel = _courseScheduleDataServiceConfigurationModel.ListCourseScheduleDatabaseModel.Where(w => w.NameOfDayWeek == input.NameOfDayWeek);

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                courseScheduleModel = courseScheduleModel.Where(w => (w.NumberWeek != null && w.NumberWeek.Contains(input.NumberWeek)) 
                || (w.NumberWeek == null) && w.ParityWeek == input.ParityWeek);


                return courseScheduleModel.ToList();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }
        }
    }
}
