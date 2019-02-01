using Microsoft.AspNetCore.Identity;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Data
{
    public class DbSeeder
    {
        private MyAppContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DbSeeder(MyAppContext context,
                        UserManager<ApplicationUser> userManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDatabase()
        {
            if (!_context.Teachers.Any())
            {
                List<Teacher> teachers = new List<Teacher>()
                {
                    new Teacher() { Name = "세종대왕", Class = "한글" },
                    new Teacher() { Name = "이순신",   Class = "해상전략" },
                    new Teacher() { Name = "제갈량",   Class = "지략" },
                    new Teacher() { Name = "을지문덕", Class = "지상전략" }
                };

                //리스트 = AddRangeAsync
                await _context.AddRangeAsync(teachers);
                //데이터베이스 저장
                await _context.SaveChangesAsync();
                //하나만 = AddAsync
                //await _context.AddAsync(new Teacher() { Name = "세종대왕", Class = "한글" });
            }

            var adminAccount = await _userManager.FindByNameAsync("admin@gmail.com");
            var adminRole = new IdentityRole("Admin");
            await _roleManager.CreateAsync(adminRole);
            await _userManager.AddToRoleAsync(adminAccount, adminRole.Name);
        }
    }
}
