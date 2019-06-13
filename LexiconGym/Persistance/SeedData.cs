﻿using LexiconGym.Core.Models;
using LexiconGym.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Persistance
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services, string adminPW)
        {
            using(var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                if(userManager == null || roleManager == null)
                {
                    throw new Exception("UserManager or RoleManager is null");
                }

                var roleNames = new[] { "Admin", "Member" };

                foreach (var name in roleNames)
                {
                    if (await roleManager.RoleExistsAsync(name)) continue;
                    var role = new IdentityRole { Name = name };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }

                var emails = new[] { "adminPW@gym.se" };

                foreach (var email in emails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    var user = new ApplicationUser { UserName = email, Email = email };
                    var addUserResult = await userManager.CreateAsync(user, adminPW);
                    if (!addUserResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addUserResult.Errors));
                    }
                }

                var adminUser = await userManager.FindByEmailAsync(emails[0]);
                foreach (var role in roleNames)
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, role);
                    if (!addToRoleResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addToRoleResult.Errors));
                    }
                }






            }
        }
    }
}
