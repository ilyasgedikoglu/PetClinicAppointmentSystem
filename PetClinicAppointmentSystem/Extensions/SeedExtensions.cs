using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem.Extensions
{
    public static class SeedExtensions
    {
        public static IWebHost Seed(this IWebHost webhost)
        {
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                // alternatively resolve UserManager instead and pass that if only think you want to seed are the users     
                using (var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                {
                    SeedData.SeedAsync(dbContext).GetAwaiter().GetResult();
                }
            }
            return null;
        }
        public static class SeedData
        {
            public static async Task SeedAsync(DatabaseContext dbContext)
            {
                // dbContext.Users.Add(new User { Id = 1, Username = "admin", PasswordHash = ... });
            }
        }
    }
}
