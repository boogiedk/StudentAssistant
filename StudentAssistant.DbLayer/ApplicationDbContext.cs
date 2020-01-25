using System.IO;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
        public DbSet<ExamScheduleDatabaseModel> ExamScheduleDatabaseModels { get; set; }
        public DbSet<StudyGroupModel> StudyGroupDatabaseModels { get; set; }
        public DbSet<TeacherModel> TeacherDatabaseModels { get; set; }

        public DbSet<StudentModel> StudentModels { get; set; }

        public DbSet<UserModel> UserModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var pathDb = Path.Combine(@"..\", "StudentAssistant.Backend", "StudentAssistantDb.Db");
            // optionsBuilder.UseSqlServer(
            //    "Data Source=DESKTOP-G847LFJ;Initial Catalog=StudentAssistantDb;MultipleActiveResultSets=true;Integrated Security=True");
            optionsBuilder.UseSqlite($"Filename={pathDb}");
            base.OnConfiguring(optionsBuilder);
        }
    }


    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var context = new ApplicationDbContext(builder.Options);
            return context;
        }
    }
}