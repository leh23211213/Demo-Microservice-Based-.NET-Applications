﻿// <auto-generated />
using App.Services.AuthAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.Services.ProductAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240904123559_InitProductTable")]
    partial class InitProductTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("App.Services.ProductAPI.Models.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrandId"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("BrandId");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            BrandId = 1,
                            BrandName = "Apple",
                            ImageUrl = "https://placehold.co/603x403"
                        });
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Smartphone"
                        });
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Color", b =>
                {
                    b.Property<int>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ColorId"));

                    b.Property<string>("ColorName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ColorId");

                    b.ToTable("Colors");

                    b.HasData(
                        new
                        {
                            ColorId = 1,
                            ColorName = "Black"
                        },
                        new
                        {
                            ColorId = 2,
                            ColorName = "Pink"
                        },
                        new
                        {
                            ColorId = 3,
                            ColorName = "Red"
                        },
                        new
                        {
                            ColorId = 4,
                            ColorName = "Blue"
                        },
                        new
                        {
                            ColorId = 5,
                            ColorName = "Green"
                        });
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Product", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<string>("ImageLocalPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ColorId");

                    b.HasIndex("SizeId");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            ProductId = "IP13MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 13 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP14MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 14 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP15MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 15 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP16MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 16 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP17MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 17 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP18MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 18 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP19MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 19 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP20MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 20 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP21MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 21 Mini",
                            SizeId = 1
                        },
                        new
                        {
                            ProductId = "IP22MiniBK128GB",
                            BrandId = 1,
                            CategoryId = 1,
                            ColorId = 1,
                            ImageUrl = "https://placehold.co/603x403",
                            Price = 999m,
                            ProductName = "iPhone 22 Mini",
                            SizeId = 1
                        });
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"));

                    b.Property<string>("RAM")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("SizeId");

                    b.ToTable("Sizes");

                    b.HasData(
                        new
                        {
                            SizeId = 1,
                            RAM = "128GB"
                        },
                        new
                        {
                            SizeId = 2,
                            RAM = "256GB"
                        },
                        new
                        {
                            SizeId = 3,
                            RAM = "512GB"
                        },
                        new
                        {
                            SizeId = 4,
                            RAM = "1T"
                        });
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Product", b =>
                {
                    b.HasOne("App.Services.ProductAPI.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.Services.ProductAPI.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.Services.ProductAPI.Models.Color", "Color")
                        .WithMany("Products")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.Services.ProductAPI.Models.Size", "Size")
                        .WithMany("Products")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Category");

                    b.Navigation("Color");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Color", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("App.Services.ProductAPI.Models.Size", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
