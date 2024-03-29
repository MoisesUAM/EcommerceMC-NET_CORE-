﻿using System.Reflection;
using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<StoreModel> Stores { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> UserModels {  get; set; }
        public DbSet<StoreProductModel> StoresProducts { get; set; }
        public DbSet<InventoryModel> Inventories { get; set; }
        public DbSet<DetailsInventoryModels> DetailsInventories { get; set; }
        public DbSet<TransactionsModel> Transactions { get; set; }
        public DbSet< CompanyModel> Companies { get; set; }
        public DbSet<ShoppingCartModel> ShoppingCarts { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailsModel> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
