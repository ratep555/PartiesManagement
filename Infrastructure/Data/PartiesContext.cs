using System;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PartiesContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, 
        IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public PartiesContext(DbContextOptions<PartiesContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<ApplicationRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            modelBuilder.Entity<CustomerOrder>()
                .HasMany(x => x.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<CustomerOrder>()
                .OwnsOne(s => s.ShippingAddress, a => {a.WithOwner();});
                
            modelBuilder.Entity<CustomerOrder>()
                .Property(s => s.OrderStatus)
                .HasConversion(
                    o => o.ToString(),
                    o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o)
                );

            modelBuilder.Entity<CustomerOrder>()
                .Property(s => s.PaymentStatus)
                .HasConversion(
                    o => o.ToString(),
                    o => (PaymentStatus) Enum.Parse(typeof(PaymentStatus), o)
                );

            modelBuilder.Entity<OrderItem>()
                .OwnsOne(s => s.BasketItemOrdered, io => {io.WithOwner();});
            
            modelBuilder.Entity<ItemCategory>()
                .HasKey(x => new { x.ItemId, x.CategoryId }); 

            modelBuilder.Entity<ItemManufacturer>()
                .HasKey(x => new { x.ItemId, x.ManufacturerId }); 

            modelBuilder.Entity<ItemTag>()
                .HasKey(x => new { x.ItemId, x.TagId }); 

            modelBuilder.Entity<ItemWarehouse>()
                .HasKey(x => new { x.ItemId, x.WarehouseId }); 

        }

            public DbSet<Address> Addresses { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Client> Clients { get; set; }
            public DbSet<Country> Countries { get; set; }
            public DbSet<Item> Items { get; set; }
            public DbSet<ItemCategory> ItemCategories { get; set; }
            public DbSet<ItemManufacturer> ItemManufacturers { get; set; }
            public DbSet<ItemReview> ItemReviews { get; set; }
            public DbSet<ItemTag> ItemTags { get; set; }
            public DbSet<ItemWarehouse> ItemWarehouses { get; set; }
            public DbSet<Manufacturer> Manufacturers { get; set; }
            public DbSet<CustomerOrder> CustomerOrders { get; set; }
            public DbSet<PaymentOption> PaymentOptions { get; set; }
            public DbSet<ShippingOption> ShippingOptions { get; set; }
            public DbSet<Tag> Tags { get; set; }
            public DbSet<Warehouse> Warehouses { get; set; }

    }
}





