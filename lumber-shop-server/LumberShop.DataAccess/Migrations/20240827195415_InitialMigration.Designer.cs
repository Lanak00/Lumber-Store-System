﻿// <auto-generated />
using System;
using LumberStoreSystem.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LumberStoreSystem.DataAccess.Migrations
{
    [DbContext(typeof(LumberStoreSystemDbContext))]
    [Migration("20240827195415_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.CuttingList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("CuttingLists");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.CuttingListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("CuttingListId")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CuttingListId");

                    b.ToTable("CuttingListItems");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Dimensions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Dimensions");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("DimensionsId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DimensionsId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Users", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Client", b =>
                {
                    b.HasBaseType("LumberStoreSystem.DataAccess.Model.User");

                    b.ToTable("Clients", (string)null);
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Employee", b =>
                {
                    b.HasBaseType("LumberStoreSystem.DataAccess.Model.User");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UMCN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.CuttingList", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.Order", null)
                        .WithMany("CuttingLists")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LumberStoreSystem.DataAccess.Model.Product", "Product")
                        .WithMany("CuttingLists")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.CuttingListItem", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.CuttingList", "CuttingList")
                        .WithMany("cuttingListItems")
                        .HasForeignKey("CuttingListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CuttingList");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Order", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.OrderItem", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LumberStoreSystem.DataAccess.Model.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Product", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.Dimensions", "Dimensions")
                        .WithMany()
                        .HasForeignKey("DimensionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dimensions");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.User", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.Address", "Address")
                        .WithMany("Users")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Client", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.User", null)
                        .WithOne()
                        .HasForeignKey("LumberStoreSystem.DataAccess.Model.Client", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Employee", b =>
                {
                    b.HasOne("LumberStoreSystem.DataAccess.Model.User", null)
                        .WithOne()
                        .HasForeignKey("LumberStoreSystem.DataAccess.Model.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Address", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.CuttingList", b =>
                {
                    b.Navigation("cuttingListItems");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Order", b =>
                {
                    b.Navigation("CuttingLists");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Product", b =>
                {
                    b.Navigation("CuttingLists");

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("LumberStoreSystem.DataAccess.Model.Client", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}