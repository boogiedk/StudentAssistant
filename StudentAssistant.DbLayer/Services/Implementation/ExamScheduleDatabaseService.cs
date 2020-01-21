using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class ExamScheduleDatabaseService : IExamScheduleDatabaseService
    {
        private readonly ApplicationDbContext _context;

        public ExamScheduleDatabaseService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(List<ExamScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    foreach (var examScheduleDatabaseModel in input)
                    {
                        _context.ExamScheduleDatabaseModels.Add(examScheduleDatabaseModel);

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
        public async Task<List<ExamScheduleDatabaseModel>> GetByParameters(ExamScheduleParametersModel parameters)
#pragma warning restore 1998
        {
            try
            {
                if (parameters == null)
                {
                    throw new NotSupportedException();
                }

                var examScheduleDatabaseModels = _context.ExamScheduleDatabaseModels
                    .Include(i => i.TeacherModel)
                    .Include(d => d.StudyGroupModel)
                    .ToList();

                var result = examScheduleDatabaseModels.Where(f =>
                        (f.CourseType == parameters.CourseTypeExam
                         || f.CourseType == parameters.CourseTypeConsultation)
                        && string.Equals(f.StudyGroupModel?.Name, parameters.StudyGroupModel.Name)
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
            var notDeletedList = _context.ExamScheduleDatabaseModels
                .Where(d => d.IsDeleted == false
                            & (d.CourseType ==
                               CourseType.ExamCourse
                               || d.CourseType ==
                               CourseType.СonsultationCourse)
                )
                .ToList();

            notDeletedList.ForEach(s => s.IsDeleted = true);

            _context.ExamScheduleDatabaseModels.UpdateRange(notDeletedList);
        }

        public async Task UpdateAsync(List<ExamScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var csd = _context.ExamScheduleDatabaseModels.Where(w => w.CourseType == CourseType.ExamCourse);

                _context.ExamScheduleDatabaseModels
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