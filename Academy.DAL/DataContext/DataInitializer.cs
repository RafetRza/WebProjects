using Academy.DAL.DataContext.Entities;
using Core.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext
{
    public class DataInitializer
    {
        private readonly AcademyDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public DataInitializer(AcademyDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedData()
        {
            await _context.Database.MigrateAsync();

            var roles = new[] { "Teacher", "Student", "Admin" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = new AppUser { UserName = "admin", Email = "admin@gmail.com" };

            if (await _userManager.FindByNameAsync(adminUser.UserName) == null)
            {
                var result = await _userManager.CreateAsync(adminUser, "1234");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var normalUser = new AppUser { UserName = "user", Email = "user@gmail.com" };

            if (await _userManager.FindByNameAsync(normalUser.UserName) == null)
            {
                var result = await _userManager.CreateAsync(normalUser, "1234");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(normalUser, "User");
                }
            }

            var teacherUser = new AppUser { UserName = "teacher", Email = "teacher@gmail.com" };

            if (await _userManager.FindByNameAsync(teacherUser.UserName) == null)
            {
                var result = await _userManager.CreateAsync(teacherUser, "1234");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(teacherUser, "Teacher");
                }
            }

            var studentUser = new AppUser { UserName = "student", Email = "student@gmail.com" };

            if (await _userManager.FindByNameAsync(studentUser.UserName) == null)
            {
                var result = await _userManager.CreateAsync(studentUser, "1234");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(studentUser, "Student");
                }
            }

            // Seed Teachers
            var teacherNames = new[] { "Teacher 1", "Teacher 2" };
            foreach (var tName in teacherNames)
            {
                if (!await _context.Teachers.AnyAsync(t => t.Name == tName))
                {
                    var userName = tName.Replace(" ", "").ToLower();
                    var user = await _userManager.FindByNameAsync(userName);
                    if (user == null)
                    {
                        user = new AppUser { UserName = userName, Email = $"{userName}@gmail.com" };
                        var result = await _userManager.CreateAsync(user, "1234");
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Teacher");
                        }
                        else continue;
                    }
                    
                    await _context.Teachers.AddAsync(new Teacher { Name = tName, AppUserId = user.Id });
                    await _context.SaveChangesAsync();
                }
            }

            // Seed Groups
            var groupsToSeed = new[] { "P301", "P302", "P303" };
            var allTeachers = await _context.Teachers.ToListAsync();
            if (allTeachers.Any())
            {
                for (int i = 0; i < groupsToSeed.Length; i++)
                {
                    var gName = groupsToSeed[i];
                    if (!await _context.Groups.AnyAsync(g => g.Name == gName))
                    {
                        var teacher = allTeachers[i % allTeachers.Count];
                        await _context.Groups.AddAsync(new Group { Name = gName, TeacherId = teacher.Id });
                        await _context.SaveChangesAsync();
                    }
                }
            }

            // Seed Students
            var allGroups = await _context.Groups.ToListAsync();
            foreach (var group in allGroups)
            {
                var studentCount = await _context.Students.CountAsync(s => s.GroupId == group.Id);
                if (studentCount < 10)
                {
                    for (int i = studentCount + 1; i <= 10; i++)
                    {
                        var studentName = $"Student-{group.Name}-{i}";
                        var studentUserName = studentName.ToLower().Replace("-", "");
                        var studentUserEntity = await _userManager.FindByNameAsync(studentUserName);
                        
                        if (studentUserEntity == null)
                        {
                            studentUserEntity = new AppUser { UserName = studentUserName, Email = $"{studentUserName}@gmail.com" };
                            var result = await _userManager.CreateAsync(studentUserEntity, "1234");
                            if (result.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(studentUserEntity, "Student");
                            }
                            else continue;
                        }

                        if (!await _context.Students.AnyAsync(s => s.AppUserId == studentUserEntity.Id))
                        {
                            await _context.Students.AddAsync(new Student 
                            { 
                                Name = studentName, 
                                GroupId = group.Id,
                                AppUserId = studentUserEntity.Id
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
