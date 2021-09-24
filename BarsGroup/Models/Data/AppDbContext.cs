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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            modelBuilder.Entity<Employee>()
                .HasOne(s => s.Organization)
                .WithMany(o => o.Employees)
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
                new Organization { Id = ++id, Inn = 1000067891, Name = "Технологика", TeacherId = 2 },
                new Organization { Id = ++id, Inn = 1234500000, Name = "1С", TeacherId = 2 },
                new Organization { Id = ++id, Inn = 9992267891, Name = "Лукойл", TeacherId = 3 },
                new Organization { Id = ++id, Inn = 1230099112, Name = "Газпром", TeacherId = 4 },
                new Organization { Id = ++id, Inn = 9992267891, Name = "АСУ ТП", TeacherId = 5 }
             );
            id = 0;
            modelBuilder.Entity<Employee>().HasData(
               new Employee { Id = ++id, FullName = "Макарова Т. И.", OrganizationId = 1 },
               new Employee { Id = ++id, FullName = "Орликова П. А.", OrganizationId = 2 },
               new Employee { Id = ++id, FullName = "Мирошниченко Т. В.", OrganizationId = 2 },
               new Employee { Id = ++id, FullName = "Маликов Д. В.", OrganizationId = 3 },
               new Employee { Id = ++id, FullName = "Карпов П. П.", OrganizationId = 3 },
               new Employee { Id = ++id, FullName = "Русскин К. С.", OrganizationId = 3 },
               new Employee { Id = ++id, FullName = "Зуев П. А.", OrganizationId = 4 },
               new Employee { Id = ++id, FullName = "Шилов Е. С.", OrganizationId = 4 },
               new Employee { Id = ++id, FullName = "Семенов Д. В.", OrganizationId = 5 },
               new Employee { Id = ++id, FullName = "Дудкина П. П.", OrganizationId = 6 },
               new Employee { Id = ++id, FullName = "Репин К. С.", OrganizationId = 6 },
               new Employee { Id = ++id, FullName = "Комков П. А.", OrganizationId = 7 }
            );
            id = 0;
            modelBuilder.Entity<Group>().HasData(
                new Group { Id = ++id, Name = "Группа" + id, TeacherId = 1, CourseId = 1},
                new Group { Id = ++id, Name = "Группа" + id, TeacherId = 2, CourseId = 2},
                new Group { Id = ++id, Name = "Группа" + id, TeacherId = 3, CourseId = 3}
            );

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Employees)
                .WithMany(s => s.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeGroup",
                    r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                    l => l.HasOne<Group>().WithMany().HasForeignKey("GroupId"),
                    je =>
                    {
                        je.HasKey("EmployeeId", "GroupId");
                        je.HasData(
                            new { EmployeeId = 1, GroupId = 1 },
                            new { EmployeeId = 2, GroupId = 1 },
                            new { EmployeeId = 3, GroupId = 1 },
                            new { EmployeeId = 4, GroupId = 2 },
                            new { EmployeeId = 5, GroupId = 2 },
                            new { EmployeeId = 6, GroupId = 3 },
                            new { EmployeeId = 7, GroupId = 3 },
                            new { EmployeeId = 8, GroupId = 3 }
                         );
                    });


        }
    }
}
