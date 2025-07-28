using HR.Model;
using Microsoft.EntityFrameworkCore;

namespace HR
{
    public class HrDbContext : DbContext
    {
        public HrDbContext(DbContextOptions<HrDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Called Parent Method
            //Seed

            modelBuilder.Entity<Lookup>().HasData(
                // Employee Positions (Major Code = 0)
                new Lookup { Id = 1, MajorCode = 0, MinorCode = 0, Name = "Employee Positions" },
                new Lookup { Id = 2, MajorCode = 0, MinorCode = 1, Name = "HR"},
                new Lookup { Id = 3, MajorCode = 0, MinorCode = 2, Name = "Manager" },
                new Lookup { Id = 4, MajorCode = 0, MinorCode = 3, Name = "Developer" },

                // Department Types (Major Code = 1)
                new Lookup { Id = 5, MajorCode = 1, MinorCode = 0, Name = "Department Types" },
                new Lookup { Id = 6, MajorCode = 1, MinorCode = 1, Name = "Finance" },
                new Lookup { Id = 7, MajorCode = 1, MinorCode = 2, Name = "Adminstrative" },
                new Lookup { Id = 8, MajorCode = 1, MinorCode = 3, Name = "Technical" },

                new Lookup { Id = 9, MajorCode = 2, MinorCode = 0, Name = "Vacation Types" },
                new Lookup { Id = 10, MajorCode = 2, MinorCode = 1, Name = "Annual Vacation" },
                new Lookup { Id = 11, MajorCode = 2, MinorCode = 2, Name = "Sick Vacation" },
                new Lookup { Id = 12, MajorCode = 2, MinorCode = 3, Name = "Unpaid Vacation" }

                );
            //                  BCrypt.Net.BCrypt.HashPassword("Admin@123") =  "$2a$11$pkpybQvqeC0S6e1dTqUrwO/jB0AbZC0fKcUrmYJS/1nmW4a2UC7YW"
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "Admin", HashedPassword = "$2a$11$pkpybQvqeC0S6e1dTqUrwO/jB0AbZC0fKcUrmYJS/1nmW4a2UC7YW", IsAdmin = true}
                );

        }

        // Define Tables

        //Employees Table
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
    }
}
