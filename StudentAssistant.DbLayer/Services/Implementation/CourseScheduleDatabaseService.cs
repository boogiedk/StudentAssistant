using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleDatabaseService : ICourseScheduleDatabaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImportDataExcelService _importDataExcelService;

        public CourseScheduleDatabaseService(ApplicationDbContext context, IImportDataExcelService importDataExcelService)
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
                    foreach (var courseScheduleDatabaseModel in input)
                    {
                        _context.CourseScheduleDatabaseModels.Add(courseScheduleDatabaseModel);

                        await _context.SaveChangesAsync(cancellationToken);
                    }

//                    _context.CourseScheduleDatabaseModels.AddRange(input);
//
//                    await _context.SaveChangesAsync(cancellationToken);

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
                    .Include(i=>i.TeacherModel)
                    .Include(d=>d.StudyGroupModel)
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
                    && f.StudyGroupModel?.Name == parameters.GroupName)
                    .ToList();

                return result;
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

                //   foreach (var entity in _context.CourseScheduleDatabaseModels)
                //    {
                //        _context.CourseScheduleDatabaseModels
                //           .Remove(entity);
                //       _context.SaveChanges();
                //    }


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
                var studyGroupDb = _context.StudyGroupDatabaseModels.ToList();

                // сравниваем список из бд и входящих,
                // чтобы найти группы, которых нет в бд
                var studyGroupNew = studyGroupDb
                    .Where(p => input
                        .Select(s => s.StudyGroupModel)
                        .All(f => !string.Equals(f.Name, p.Name)));

                // добавляем новые группы в бд
                foreach (var studyGroupModel in studyGroupNew)
                {
                    _context.StudyGroupDatabaseModels.Add(new StudyGroupModel
                    {
                        Id = Guid.NewGuid(),
                        Name = studyGroupModel.Name
                    });
                }

                var studyGroupDbAll = _context.StudyGroupDatabaseModels.ToList();

                // изменяем модели групп во входящем списке
                foreach (var model in input)
                {
                    model.StudyGroupModel = studyGroupDbAll
                        .FirstOrDefault(s => string.Equals(s.Name, model.StudyGroupModel.Name));
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