﻿using LumberStoreSystem.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;


namespace LumberStoreSystem.DataAccess
{
    public class LumberStoreSystemDbContext : DbContext
    {
        public LumberStoreSystemDbContext() { }

        public LumberStoreSystemDbContext(DbContextOptions<LumberStoreSystemDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CuttingList> CuttingLists { get; set; }
        public DbSet<CuttingListItem> CuttingListItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dimension> Dimensions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Employee>().ToTable("Employees");

            modelBuilder.Entity<User>()
            .HasOne(e => e.Address)
            .WithMany(e => e.users)
            .HasForeignKey(e => e.AddressId);

            modelBuilder.Entity<Product>()
           .HasOne(e => e.Dimension)
           .WithMany(e => e.products)
           .HasForeignKey(e => e.DimensionId);

            modelBuilder.Entity<OrderItem>()
           .HasOne(e => e.Order)
           .WithMany(e => e.items)
           .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<OrderItem>()
            .HasOne(e => e.Product)
            .WithMany(e => e.orderItems)
            .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<Order>()
            .HasOne(e => e.Client)
            .WithMany(e => e.orders)
            .HasForeignKey(e => e.ClientId);

            modelBuilder.Entity<CuttingListItem>()
            .HasOne(e => e.CuttingList)
            .WithMany(e => e.cuttingListItems)
            .HasForeignKey(e => e.CuttingListId);

        }
    }
}
