using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class ImportDataJsonService : IImportDataJsonService
    {
        private readonly CourseScheduleDataServiceConfigurationModel _courseScheduleDataServiceConfigurationModel;

        public ImportDataJsonService(IOptions<CourseScheduleDataServiceConfigurationModel> courseScheduleDataServiceConfigurationModel, IImportDataExcelService importDataExcelService)
        {
            _courseScheduleDataServiceConfigurationModel = courseScheduleDataServiceConfigurationModel.Value;
        }

        public List<CourseScheduleDatabaseModel> GetCourseScheduleDatabaseModels()
        {
            return _courseScheduleDataServiceConfigurationModel?.ListCourseScheduleDatabaseModel;
        }
    }
}
