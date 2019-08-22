using System.Collections.Generic;
using Microsoft.Extensions.Options;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Services.Interfaces;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class ImportDataJsonService : IImportDataJsonService
    {
        private readonly CourseScheduleDataServiceConfigurationModel _courseScheduleDataServiceConfigurationModel;

        public ImportDataJsonService(IOptions<CourseScheduleDataServiceConfigurationModel> courseScheduleDataServiceConfigurationModel)
        {
            _courseScheduleDataServiceConfigurationModel = courseScheduleDataServiceConfigurationModel.Value;
        }

        public IEnumerable<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels()
        {
            return _courseScheduleDataServiceConfigurationModel?.ListCourseScheduleDatabaseModel;
        }
    }
}
