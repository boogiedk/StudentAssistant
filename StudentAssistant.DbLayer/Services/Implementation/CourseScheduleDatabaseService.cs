using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;

namespace StudentAssistant.DbLayer.Services.Implementation
{
    public class CourseScheduleDatabaseService : ICourseScheduleDatabaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImportDataExcelService _importDataExcelService;
        private readonly ILogger<CourseScheduleDatabaseService> _logger;

        public CourseScheduleDatabaseService(
            ApplicationDbContext context,
            IImportDataExcelService importDataExcelService,
            ILogger<CourseScheduleDatabaseService> logger)
        {
            _context = context;
            _importDataExcelService = importDataExcelService;
            _logger = logger;
        }

        public async Task InsertAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _context.CourseSchedules.AddRange(input);

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

                var courseSchedules = _context.CourseSchedules
                    .Include(i => i.TeacherModel)
                    .Include(d => d.StudyGroupModel)
                    .Where(d => (d.CourseType == CourseType.Lecture
                                 || d.CourseType == CourseType.Practicte
                                 || d.CourseType == CourseType.LaboratoryWork
                                 || d.CourseType == CourseType.Other))
                    .ToList();

                var list = new List<CourseScheduleDatabaseModel>();

                foreach (var scheduleDatabaseModel in courseSchedules)
                {
                    var model = scheduleDatabaseModel;

                    model.NumberWeek = _importDataExcelService.ParseNumberWeek(scheduleDatabaseModel.NumberWeekString);

                    list.Add(model);
                }

                var result = list.Where(f =>
                        string.Equals(f.NameOfDayWeek, parameters.NameOfDayWeek, StringComparison.OrdinalIgnoreCase)
                        && (f.NumberWeek.Any(a => a == parameters.NumberWeek) || f.NumberWeek.Count == 0)
                        && f.ParityWeek == parameters.ParityWeek
                        && f.StudyGroupModel?.Name == parameters.GroupName
                        && f.IsDeleted == false)
                    .ToList();
                
                var studyGroupCombined = new List<StudyGroupModel>();
                
                var filteredCourseSchedules = courseSchedules.Where(f =>
                        string.Equals(f.NameOfDayWeek, parameters.NameOfDayWeek, StringComparison.OrdinalIgnoreCase)
                        && (f.NumberWeek.Any(a => a == parameters.NumberWeek) || f.NumberWeek.Count == 0)
                        && f.ParityWeek == parameters.ParityWeek
                        && f.IsDeleted == false)
                    .ToList();
                
                foreach (var model in result)
                {
                    foreach (var scheduleDatabaseModel in filteredCourseSchedules
                        .Where(scheduleDatabaseModel => model.CoursePlace == scheduleDatabaseModel.CoursePlace 
                                                        && model.CourseNumber == scheduleDatabaseModel.CourseNumber 
                                                        && model.StudyGroupModel.Name != scheduleDatabaseModel.StudyGroupModel.Name))
                    {
                        studyGroupCombined.Add(scheduleDatabaseModel.StudyGroupModel);

                        model.CombinedGroup = studyGroupCombined.Distinct().ToList();
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }

        public void MarkLikeDeleted()
        {
            var notDeletedList = _context.CourseSchedules
                .Where(d => d.IsDeleted == false &&
                            (d.CourseType == CourseType.Lecture
                             || d.CourseType == CourseType.Practicte
                             || d.CourseType == CourseType.LaboratoryWork
                             || d.CourseType == CourseType.Other))
                .ToList();

            notDeletedList.ForEach(s => s.IsDeleted = true);

            _context.CourseSchedules.UpdateRange(notDeletedList);
        }

        public async Task UpdateAsync(List<CourseScheduleDatabaseModel> input, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var csd = _context.CourseSchedules.Where
                (w => w.CourseType == CourseType.Lecture
                      || w.CourseType == CourseType.Practicte
                      || w.CourseType == CourseType.LaboratoryWork
                      || w.CourseType == CourseType.Other);

                _context.CourseSchedules
                    .RemoveRange(csd);

                _context.SaveChanges();

                // берем из бд всех преподов
                var teachersDbAll = _context.Teachers.ToList();

                // изменяем модели преподов во входящем списке
                foreach (var model in input)
                {
                    model.TeacherModel = teachersDbAll
                        .FirstOrDefault(s => string.Equals(s.FullName, model.TeacherModel.FullName));
                }

                // группы
                var studyGroupDbAll = _context.StudyGroups.ToList();

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