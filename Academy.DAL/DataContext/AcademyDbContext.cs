using Academy.DAL.DataContext.Entities;
using Core.Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext
{
    public class AcademyDbContext : IdentityDbContext<AppUser>
    {
        public AcademyDbContext(DbContextOptions<AcademyDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.AppUserId)
                .IsUnique();

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.AppUser)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(t => t.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.AppUserId)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasOne(s => s.AppUser)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
