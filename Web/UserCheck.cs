using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace diagnostic
{
    public class UserCheck
    {
        public static async Task Run(IServiceProvider services, string email)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                Console.WriteLine($"User {email} not found.");
                return;
            }

            Console.WriteLine($"User: {user.Email}");
            Console.WriteLine($"UserName: {user.UserName}");
            Console.WriteLine($"EmailConfirmed: {user.EmailConfirmed}");
            Console.WriteLine($"LockoutEnabled: {user.LockoutEnabled}");
            Console.WriteLine($"LockoutEnd: {user.LockoutEnd}");
            Console.WriteLine($"AccessFailedCount: {user.AccessFailedCount}");
        }
    }
}
