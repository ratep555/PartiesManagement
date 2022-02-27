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
                if (!context.Accounts.Any())
                {
                    var accountsData = File.ReadAllText("../Infrastructure/Data/SeedData/accounts.json");
                    var accounts = JsonSerializer.Deserialize<List<Account>>(accountsData);

                    foreach (var item in accounts)
                    {
                        context.Accounts.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

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

                if (!context.Categories.Any())
                {
                    var categoriesData = File.ReadAllText("../Infrastructure/Data/SeedData/categories.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                    foreach (var item in categories)
                    {
                        context.Categories.Add(item);
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

                if (!context.Discounts.Any())
                {
                    var discountsData = File.ReadAllText("../Infrastructure/Data/SeedData/discounts.json");
                    var discounts = JsonSerializer.Deserialize<List<Discount>>(discountsData);

                    foreach (var item in discounts)
                    {
                        context.Discounts.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Manufacturers.Any())
                {
                    var manufacturersData = File.ReadAllText("../Infrastructure/Data/SeedData/manufacturers.json");
                    var manufacturers = JsonSerializer.Deserialize<List<Manufacturer>>(manufacturersData);

                    foreach (var item in manufacturers)
                    {
                        context.Manufacturers.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Manufacturers1.Any())
                {
                    var manufacturers1Data = File.ReadAllText("../Infrastructure/Data/SeedData/manufacturers1.json");
                    var manufacturers1 = JsonSerializer.Deserialize<List<Manufacturer1>>(manufacturers1Data);

                    foreach (var item in manufacturers1)
                    {
                        context.Manufacturers1.Add(item);
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
                
                if (!context.PaymentStatuses1.Any())
                {
                    var paymentstatusData = File.ReadAllText("../Infrastructure/Data/SeedData/paymentstatuses1.json");
                    var paymenstatuses = JsonSerializer.Deserialize<List<PaymentStatus1>>(paymentstatusData);

                    foreach (var item in paymenstatuses)
                    {
                        context.PaymentStatuses1.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.OrderStatus1.Any())
                {
                    var orderstatusData = File.ReadAllText("../Infrastructure/Data/SeedData/orderstatuses1.json");
                    var orderstatuses = JsonSerializer.Deserialize<List<OrderStatus1>>(orderstatusData);

                    foreach (var item in orderstatuses)
                    {
                        context.OrderStatus1.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Tags.Any())
                {
                    var tagsData = File.ReadAllText("../Infrastructure/Data/SeedData/tags.json");
                    var tags = JsonSerializer.Deserialize<List<Tag>>(tagsData);

                    foreach (var item in tags)
                    {
                        context.Tags.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Warehouses.Any())
                {
                    var warehousesData = File.ReadAllText("../Infrastructure/Data/SeedData/warehouses.json");
                    var warehouses = JsonSerializer.Deserialize<List<Warehouse>>(warehousesData);

                    foreach (var item in warehouses)
                    {
                        context.Warehouses.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Locations.Any())
                {
                    var locationsData = File.ReadAllText("../Infrastructure/Data/SeedData/locations.json");
                    var locations = JsonSerializer.Deserialize<List<Location1>>(locationsData);

                    foreach (var item in locations)
                    {
                        context.Locations.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.ServicesIncluded.Any())
                {
                    var servicesincludedData = File.ReadAllText("../Infrastructure/Data/SeedData/servicesincluded.json");
                    var servicesincluded = JsonSerializer.Deserialize<List<ServiceIncluded>>(servicesincludedData);

                    foreach (var item in servicesincluded)
                    {
                        context.ServicesIncluded.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.BirthdayPackages.Any())
                {
                    var birthdaypackagesData = File.ReadAllText("../Infrastructure/Data/SeedData/birthdaypackages.json");
                    var birthdaypackages = JsonSerializer.Deserialize<List<BirthdayPackage>>(birthdaypackagesData);

                    foreach (var item in birthdaypackages)
                    {
                        context.BirthdayPackages.Add(item);
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