using MediatR.Usage.Api.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace MediatR.Usage.Api.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TeacherEntity> Teachers { get; set; }

        public DbSet<StudentEntity> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherEntity>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<StudentEntity>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<TeacherEntity>().HasData(
            new TeacherEntity
            {
                ID = 1,
                FirstName = "Ramesh",
                LastName = "Kumar",
                Email = "testramesh@gmail.com",
                PhoneNumber = "1234567890",
            },
            new TeacherEntity
            {
                ID = 2,
                FirstName = "Amit ",
                LastName = "Sharma",
                Email = "testamitsharma@gmail.com",
                PhoneNumber = "1234517890",
            });

            modelBuilder.Entity<StudentEntity>().HasData(
            new StudentEntity
            {
                ID = 1,
                FirstName = "Mohit",
                LastName = "Yadav",
                Email = "testmohit@gmail.com",
                DateOfBirth = DateTime.Now,
            },
            new StudentEntity
            {
                ID = 2,
                FirstName = "Ankit",
                LastName = "Sharma",
                Email = "testankitsharma@gmail.com",
                DateOfBirth = DateTime.Now,
            });
        }
    }
}
