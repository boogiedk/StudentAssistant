using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleMongoDbService : ICourseScheduleMongoDbService
    {
        private readonly IMongoCollection<CourseScheduleDatabaseModel> _courseScheduleDatabaseModelCollection;

        public CourseScheduleMongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.Database);

            _courseScheduleDatabaseModelCollection =
                database.GetCollection<CourseScheduleDatabaseModel>(mongoDbSettings.Value.CourseScheduleCollectionName);
        }

        public async void InsertAsync(List<CourseScheduleDatabaseModel> input)
        {
            if (input == null || !input.Any())
            {
                throw new NotImplementedException();
            }

            try
            {
                await _courseScheduleDatabaseModelCollection
                    .InsertManyAsync(input);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async void RemoveAllAsync()
        {
            try
            {
                await _courseScheduleDatabaseModelCollection.DeleteManyAsync(d => true);
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }

        public async Task<List<CourseScheduleDatabaseModel>> GetByParameters(
            CourseScheduleParameters parameters)
        {
            try
            {
                if (parameters == null)
                {
                    throw new NotSupportedException();
                }

                var result = await _courseScheduleDatabaseModelCollection.Find(f =>
                    f.NameOfDayWeek == parameters.NameOfDayWeek
                    && (f.NumberWeek != null
                        && f.NumberWeek.Contains( new NumberWeekModel() {NumberWeek = parameters.NumberWeek})
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

        public async Task UpdateAsync(
            List<CourseScheduleDatabaseModel> input,
            CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _courseScheduleDatabaseModelCollection.DeleteManyAsync(d => true,
                    cancellationToken: cancellationToken);

                await _courseScheduleDatabaseModelCollection
                    .InsertManyAsync(input, cancellationToken: cancellationToken);
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }
    }
}