using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentAssistant.Backend.Infrastructure
{
    public sealed class Context : IdentityDbContext<IdentityUser>
    {
        public Context() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=StudentAssistant.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
