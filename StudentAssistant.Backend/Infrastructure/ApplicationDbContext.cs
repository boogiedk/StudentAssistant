using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using StudentAssistant.DbLayer.Models.Exam;

namespace StudentAssistant.Backend.Infrastructure
{
    public sealed class ApplicationDbContext : IdentityDbContext
    {
          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                   : base(options)
               {
               }
          
          public DbSet<CourseScheduleDatabaseModel> CourseScheduleDatabaseModels { get; set; }

          public DbSet<ExamScheduleDatabaseModel> ExamScheduleDatabaseModels { get; set; }
          
       // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      //  {
      //      base.OnConfiguring(optionsBuilder);
      //  }
    }
}
