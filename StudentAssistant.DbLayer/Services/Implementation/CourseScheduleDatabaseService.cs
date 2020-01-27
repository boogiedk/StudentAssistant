using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleDatabaseService : ICourseScheduleDatabaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImportDataExcelService _importDataExcelService;

        public CourseScheduleDatabaseService(
            ApplicationDbContext context,
            IImportDataExcelService importDataExcelService)
        {
            _context = context;
            _importDataExcelService = importDataExcelService;
        }

        public async Task InsertAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _context.CourseScheduleDatabaseModels.AddRange(input);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

#pragma warning disable 1998
        public async Task<List<CourseScheduleDatabaseModel>> GetByParameters(CourseScheduleParameters parameters)
#pragma warning restore 1998
        {
            try
            {
                if (parameters == null)
                {
                    throw new NotSupportedException();
                }

                var courseScheduleDatabaseModel = _context.CourseScheduleDatabaseModels
                    .Include(i => i.TeacherModel)
                    .Include(d => d.StudyGroupModel)
                    .ToList();

                var list = new List<CourseScheduleDatabaseModel>();

                foreach (var scheduleDatabaseModel in courseScheduleDatabaseModel)
                {
                    var model = scheduleDatabaseModel;

                    model.NumberWeek = _importDataExcelService.ParseNumberWeek(scheduleDatabaseModel.NumberWeekString);

                    list.Add(model);
                }

                var result = list.Where(f =>
                        f.NameOfDayWeek == parameters.NameOfDayWeek
                        && (f.NumberWeek.Any(a => a == parameters.NumberWeek) || f.NumberWeek.Count == 0)
                        && f.ParityWeek == parameters.ParityWeek
                        && f.StudyGroupModel?.Name == parameters.GroupName
                        && f.IsDeleted == false)
                    .ToList();

                return result;
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }

        public void MarkLikeDeleted()
        {
            var notDeletedList = _context.CourseScheduleDatabaseModels
                .Where(d => d.IsDeleted == false &&
                            (d.CourseType == CourseType.Lecture
                             || d.CourseType == CourseType.Practicte
                             || d.CourseType == CourseType.LaboratoryWork
                             || d.CourseType == CourseType.Other))
                .ToList();

            notDeletedList.ForEach(s => s.IsDeleted = true);

            _context.CourseScheduleDatabaseModels.UpdateRange(notDeletedList);
        }

        public async Task UpdateAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var csd = _context.CourseScheduleDatabaseModels.Where
                (w => w.CourseType == CourseType.Lecture
                      || w.CourseType == CourseType.Practicte
                      || w.CourseType == CourseType.LaboratoryWork
                      || w.CourseType == CourseType.Other);

                _context.CourseScheduleDatabaseModels
                    .RemoveRange(csd);

                _context.SaveChanges();

                // берем из бд всех преподов
                var teachersDbAll = _context.TeacherDatabaseModels.ToList();

                // изменяем модели преподов во входящем списке
                foreach (var model in input)
                {
                    model.TeacherModel = teachersDbAll
                        .FirstOrDefault(s => string.Equals(s.FullName, model.TeacherModel.FullName));
                }

                // группы
                var studyGroupDbAll = _context.StudyGroupDatabaseModels.ToList();

                // изменяем модели групп во входящем списке
                foreach (var model in input)
                {
                    model.StudyGroupModel = studyGroupDbAll
                        .FirstOrDefault(s => string.Equals(s.Name, model.StudyGroupModel?.Name));
                }


                await InsertAsync(input, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}