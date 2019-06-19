using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleDatabaseService : ICourseScheduleDatabaseService
    {
        public Task UpdateAsync(
            List<CourseScheduleDatabaseModel> courseScheduleDatabaseModel,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            throw new NotImplementedException();
        }
    }
}
