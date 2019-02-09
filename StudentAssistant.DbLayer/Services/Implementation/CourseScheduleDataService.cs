using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

                var courseScheduleModel = _courseScheduleDataServiceConfigurationModel.ListCourseScheduleDatabaseModel
                     .Where(w =>
                     w.ParityWeek == input.ParityWeek
                     && w.NumberWeek.Contains(input.NumberWeek)
                     && w.NameOfDayWeek == input.NameOfDayWeek
                     ).ToList();

                return courseScheduleModel;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Ошибка во время выполнения. " + ex);
            }
        }
    }
}
