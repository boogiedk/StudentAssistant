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
    public class ControlWeekDatabaseService : IControlWeekDatabaseService
    {
        private readonly ApplicationDbContext _context;

        public ControlWeekDatabaseService(ApplicationDbContext context)
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

#pragma warning disable 1998
        public async Task<List<CourseScheduleDatabaseModel>> Get(CancellationToken cancellationToken)
#pragma warning restore 1998
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _context.CourseScheduleDatabaseModels
                    .Include(d => d.StudyGroupModel)
                    .Include(d => d.TeacherModel)
                    .Where(w => w.CourseType == CourseType.ControlCourse
                                && w.IsDeleted == false)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }

        public async Task UpdateAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var csd = _context.CourseScheduleDatabaseModels.Where
                    (w => w.CourseType == CourseType.ControlCourse);

                _context.CourseScheduleDatabaseModels
                    .RemoveRange(csd);

                _context.SaveChanges();

                // преподы
                var teachersDb = _context.TeacherDatabaseModels.ToList();

                // сравниваем список из бд и входящих,
                // чтобы найти преподов, которых нет в бд
                var teachersNew = teachersDb
                    .Where(p => input
                        .Select(s => s.TeacherModel)
                        .All(f => !string.Equals(f.FullName, p.FullName)));

                // добавляем новых преподов в бд
                foreach (var teacherModel in teachersNew)
                {
                    _context.TeacherDatabaseModels.Add(new TeacherModel
                    {
                        Id = Guid.NewGuid(),
                        FullName = teacherModel.FullName
                    });
                }

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

        public void MarkLikeDeleted()
        {
            var notDeletedList = _context.CourseScheduleDatabaseModels
                .Where(d => d.IsDeleted == false
                            && d.CourseType == CourseType.ControlCourse)
                .ToList();

            notDeletedList.ForEach(s => s.IsDeleted = true);
            
            _context.CourseScheduleDatabaseModels.UpdateRange(notDeletedList);
        }
    }
}