using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;

namespace StudentAssistant.DbLayer
{
    public sealed class ApplicationDbContext : IdentityDbContext
    {
          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                   : base(options)
               {
               }
          
          public DbSet<CourseScheduleDatabaseModel> CourseScheduleDatabaseModels { get; set; }
          
          public DbSet<StudyGroupModel> StudyGroupDatabaseModels { get; set; }
          
          public DbSet<NumberWeekModel> NumberWeekModels { get; set; }
          
          public DbSet<TeacherModel> TeacherDatabaseModels { get; set; }

          public DbSet<ExamScheduleDatabaseModel> ExamScheduleDatabaseModels { get; set; }
          
          public DbSet<StudentModel> StudentModels { get; set; }
          
          public DbSet<UserModel> UserModels { get; set; }
    }
}
