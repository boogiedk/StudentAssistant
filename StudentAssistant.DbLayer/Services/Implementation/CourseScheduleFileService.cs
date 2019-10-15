using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models.CourseSchedule;


namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleFileService : ICourseScheduleFileService
    {
        private readonly IImportDataExcelService _importDataExcelService;

        private readonly ICourseScheduleMongoDbService _courseScheduleMongo;

        public CourseScheduleFileService(
            IImportDataExcelService importDataExcelService,
                ICourseScheduleMongoDbService courseScheduleMongo)
        {
            _importDataExcelService =
                importDataExcelService ?? throw new ArgumentNullException(nameof(importDataExcelService));
            _courseScheduleMongo = courseScheduleMongo;
        }

        public async Task<IEnumerable<CourseScheduleDatabaseModel>> GetFromExcelFileByParameters(CourseScheduleParameters input)
        {
            try
            {
                if (input == null) throw new ArgumentNullException(nameof(input));
                
                _courseScheduleMongo.RemoveAllAsync();

                var z = _importDataExcelService
                    .GetCourseScheduleDatabaseModels().ToList();
                
                _courseScheduleMongo.InsertAsync(z);

                var courseScheduleDatabaseModels = await _courseScheduleMongo.GetByParameters(input);
                   // _importDataExcelService
                    //    .GetCourseScheduleDatabaseModels();
                
                // фильтруем по дням недели - берем только то, что может быть в указанный день недели.
                var filterByNameOfWeek = courseScheduleDatabaseModels
                    .Where(w => string.Equals(w.NameOfDayWeek, input.NameOfDayWeek,
                        StringComparison.InvariantCultureIgnoreCase));

                // если указаны номера недель и там указана указанная неделя, то фильтруем по этому параметру 
                // или если не указаны номера недель, то фильтруем по четности.
                var filterByParameters = filterByNameOfWeek
                    .Where(w => (w.NumberWeek != null
                                 && w.NumberWeek.Contains(input.NumberWeek))
                                || (w.NumberWeek == null || w.NumberWeek.Count == 0)
                                && w.ParityWeek == input.ParityWeek).ToArray();

                // фильтруем по группе
                var filterByGroup = filterByParameters.Where(w => string.Equals(w.GroupName, input.GroupName));

                var result = new List<string>();

                // добавляем в модель с расписанием комбинированные пары с другими группами
                foreach (var courseScheduleDatabaseModel in filterByGroup)
                {
                    foreach (var scheduleDatabaseModel in filterByParameters)
                    {
                        if (courseScheduleDatabaseModel.CoursePlace == scheduleDatabaseModel.CoursePlace
                            && courseScheduleDatabaseModel.CourseNumber == scheduleDatabaseModel.CourseNumber
                            && courseScheduleDatabaseModel.GroupName != scheduleDatabaseModel.GroupName)
                        {
                            result.Add(scheduleDatabaseModel.GroupName);

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