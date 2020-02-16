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
        public ApplicationDbContext() => Database.EnsureCreated();
        public DbSet<CourseScheduleDatabaseModel> CourseSchedules { get; set; }
        public DbSet<ExamScheduleDatabaseModel> ExamSchedules { get; set; }
        public DbSet<StudyGroupModel> StudyGroups { get; set; }
        public DbSet<TeacherModel> Teachers { get; set; }
        
        public DbSet<StudentModel> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var pathDb = Path.Combine(@"Infrastructure", "StudentAssistantDb.Db");
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
            var context = new ApplicationDbContext();
            return context;
        }
    }
}