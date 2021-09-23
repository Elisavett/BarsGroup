using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarsGroup.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Organization)
                .WithMany(o => o.Students)
                .HasForeignKey(s => s.OrganizationId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Organization>()
                .HasOne(o => o.Teacher)
                .WithMany(t => t.Organizations)
                .HasForeignKey(o => o.TeacherId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Teacher)
                .WithMany(t => t.Groups)
                .HasForeignKey(g => g.TeacherId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Group>()
               .HasOne(g => g.Course)
               .WithMany(c => c.Groups)
               .HasForeignKey(g => g.CourseId)
               .OnDelete(DeleteBehavior.ClientCascade);
            

            int id = 0;
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = ++id, Name = "Курс" + id },
                new Course { Id = ++id, Name = "Курс" + id },
                new Course { Id = ++id, Name = "Курс" + id },
                new Course { Id = ++id, Name = "Курс" + id },
                new Course { Id = ++id, Name = "Курс" + id }
             );
            id = 0;
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = ++id, FullName = "Иванов И. И.", Email = id + "teacher@mail.com" },
                new Teacher { Id = ++id, FullName = "Сидоров В. В.", Email = id + "teacher@mail.com" },
                new Teacher { Id = ++id, FullName = "Грачев М. Э.", Email = id + "teacher@mail.com" },
                new Teacher { Id = ++id, FullName = "Носонов А. Р.", Email = id + "teacher@mail.com" },
                new Teacher { Id = ++id, FullName = "Федоров Д. С.", Email = id + "teacher@mail.com" }
             );
            id = 0;
            modelBuilder.Entity<Organization>().HasData(
                new Organization { Id = ++id, Inn = 1234567891, Name = "РЖД", TeacherId = 1 },
                new Organization { Id = ++id, Inn = 9876543211, Name = "Аэрофлот", TeacherId = 1 },
                new Organization { Id = ++id, Inn = 1230067001, Name = "ABtest", TeacherId = 2 },
                new Organization { Id = ++id, Inn = 1000067891, Name = "Технологика", TeacherId = 3 },
                new Organization { Id = ++id, Inn = 1234500000, Name = "1С", TeacherId = 4 },
                new Organization { Id = ++id, Inn = 9992267891, Name = "Лукойл", TeacherId = 5 },
                new Organization { Id = ++id, Inn = 1230099112, Name = "Газпром", TeacherId = 5 }
             );
            id = 0;
            List<Student> students1 = new List<Student>()
            {
                new Student { Id = ++id, FullName = "Макарова Т. И.", OrganizationId = 1 },
               new Student { Id = ++id, FullName = "Орликова П. А.", OrganizationId = 2 },
               new Student { Id = ++id, FullName = "Мирошниченко Т. В.", OrganizationId = 3 }
            };
            List<Student> students2 = new List<Student>()
            {
                 new Student { Id = ++id, FullName = "Маликов Д. В.", OrganizationId = 4 },
               new Student { Id = ++id, FullName = "Карпов П. П.", OrganizationId = 5 }
            };
            List<Student> students3 = new List<Student>()
            {
                new Student { Id = ++id, FullName = "Русскин К. С.", OrganizationId = 3 },
               new Student { Id = ++id, FullName = "Зуев П. А.", OrganizationId = 6 },
               new Student { Id = ++id, FullName = "Шилов Е. С.", OrganizationId = 2 }
            };
            id = 0;
            modelBuilder.Entity<Student>().HasData(
               new Student { Id = ++id, FullName = "Макарова Т. И.", OrganizationId = 1 },
               new Student { Id = ++id, FullName = "Орликова П. А.", OrganizationId = 2 },
               new Student { Id = ++id, FullName = "Мирошниченко Т. В.", OrganizationId = 2 },
               new Student { Id = ++id, FullName = "Маликов Д. В.", OrganizationId = 3 },
               new Student { Id = ++id, FullName = "Карпов П. П.", OrganizationId = 3 },
               new Student { Id = ++id, FullName = "Русскин К. С.", OrganizationId = 3 },
               new Student { Id = ++id, FullName = "Зуев П. А.", OrganizationId = 4 },
               new Student { Id = ++id, FullName = "Шилов Е. С.", OrganizationId = 4 }
            );
            id = 0;
            modelBuilder.Entity<Group>().HasData(
                new Group { Id = ++id, Name = "Группа" + id, TeacherId = 1, CourseId = 1},
                new Group { Id = ++id, Name = "Группа" + id, TeacherId = 2, CourseId = 2},
                new Group { Id = ++id, Name = "Группа" + id, TeacherId = 3, CourseId = 3}
            );

            modelBuilder.Entity<Group>()
                .HasMany(s => s.Students)
                .WithMany(g => g.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentGroup",
                    r => r.HasOne<Student>().WithMany().HasForeignKey("StudentId"),
                    l => l.HasOne<Group>().WithMany().HasForeignKey("GroupId"),
                    je =>
                    {
                        je.HasKey("StudentId", "GroupId");
                        je.HasData(
                            new { StudentId = 1, GroupId = 1 },
                            new { StudentId = 2, GroupId = 1 },
                            new { StudentId = 3, GroupId = 1 },
                            new { StudentId = 4, GroupId = 2 },
                            new { StudentId = 5, GroupId = 2 },
                            new { StudentId = 6, GroupId = 3 },
                            new { StudentId = 7, GroupId = 3 },
                            new { StudentId = 8, GroupId = 3 }
                         );
                    });


        }
    }
}
