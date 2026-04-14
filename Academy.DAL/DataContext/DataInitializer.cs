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

            var roles = new[] { "Admin", "User" };
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
        }
    }
}
