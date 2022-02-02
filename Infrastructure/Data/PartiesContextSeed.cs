using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class PartiesContextSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<ApplicationRole>
            {
                new ApplicationRole{Name = "Admin"},
                new ApplicationRole{Name = "Visitor"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            if (await userManager.Users.AnyAsync()) return;
            
            var admin = new ApplicationUser
            {
                DisplayName = "Bob",
                Email = "bob@test.com",
                UserName = "admin",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");               
            await userManager.AddToRolesAsync(admin, new[] {"Admin"});                                        
        }

          public static async Task SeedEntitiesAsync(PartiesContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Items.Any())
                {
                    var itemsData = File.ReadAllText("../Infrastructure/Data/SeedData/items.json");
                    var items = JsonSerializer.Deserialize<List<Item>>(itemsData);

                    foreach (var item in items)
                    {
                        context.Items.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Countries.Any())
                {
                    var countriesData = File.ReadAllText("../Infrastructure/Data/SeedData/countries.json");
                    var countries = JsonSerializer.Deserialize<List<Country>>(countriesData);

                    foreach (var item in countries)
                    {
                        context.Countries.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.ShippingOptions.Any())
                {
                    var shippingData = File.ReadAllText("../Infrastructure/Data/SeedData/shippingoptions.json");
                    var shippingoptions = JsonSerializer.Deserialize<List<ShippingOption>>(shippingData);

                    foreach (var item in shippingoptions)
                    {
                        context.ShippingOptions.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 
                
                if (!context.PaymentOptions.Any())
                {
                    var paymentData = File.ReadAllText("../Infrastructure/Data/SeedData/paymentoptions.json");
                    var paymentoptions = JsonSerializer.Deserialize<List<PaymentOption>>(paymentData);

                    foreach (var item in paymentoptions)
                    {
                        context.PaymentOptions.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 
            
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<PartiesContext>();
                logger.LogError(ex.Message);
            }
        }
  
    }
}