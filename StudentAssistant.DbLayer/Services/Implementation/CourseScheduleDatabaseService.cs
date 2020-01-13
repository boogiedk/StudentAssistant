using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleDatabaseService : ICourseScheduleDatabaseService
    {
        private readonly ApplicationDbContext _context;

        public CourseScheduleDatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    foreach (var courseScheduleDatabaseModel in input)
                    {
                        _context.CourseScheduleDatabaseModels.Add(courseScheduleDatabaseModel);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public void RemoveAllAsync()
        {
            _context.CourseScheduleDatabaseModels.RemoveRange(_context.CourseScheduleDatabaseModels);
        }

        public async Task<List<CourseScheduleDatabaseModel>> GetByParameters(CourseScheduleParameters parameters)
        {
            try
            {
                if (parameters == null)
                {
                    throw new NotSupportedException();
                }

                var result = await _context.CourseScheduleDatabaseModels.Where(f =>
                    f.NameOfDayWeek == parameters.NameOfDayWeek
                    && (f.NumberWeek != null
                        && f.NumberWeek.Contains(new NumberWeekModel {NumberWeek = parameters.NumberWeek})
                        || f.NumberWeek == null
                        || f.NumberWeek.Count == 0)
                    && f.ParityWeek == parameters.ParityWeek
                    && f.StudyGroupModel.Name == parameters.GroupName).ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }

        public async Task UpdateAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _context.CourseScheduleDatabaseModels.RemoveRange(_context.CourseScheduleDatabaseModels);

            await InsertAsync(input, cancellationToken);
        }
    }
}