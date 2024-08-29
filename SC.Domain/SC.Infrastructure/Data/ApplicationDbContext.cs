using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SC.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentEnrollment>().HasKey(sc => new { sc.StudentId, sc.ClassId });

            modelBuilder.Entity<StudentEnrollment>()
                .HasOne<Student>(sc => sc.Student)
                .WithMany(s => s.StudentEnrollments)
                .HasForeignKey(sc => sc.StudentId);


            modelBuilder.Entity<StudentEnrollment>()
                .HasOne<Class>(sc => sc.Class)
                .WithMany(s => s.StudentEnrollments)
                .HasForeignKey(sc => sc.ClassId);

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<StudentEnrollment> StudentEnrollments { get; set; } = null!;

    }
}
