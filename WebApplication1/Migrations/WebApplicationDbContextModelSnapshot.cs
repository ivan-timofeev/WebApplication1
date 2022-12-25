﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication1.Data;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(WebApplicationDbContext))]
    partial class WebApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Models.Manufacturer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("WebApplication1.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebApplication1.Models.ProductCharacteristic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductCharacteristic");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ProductCharacteristic");
                });

            modelBuilder.Entity("WebApplication1.Models.SaleItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("PurchasePrice")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SalePointId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("SellingPrice")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("UpdatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalePointId");

                    b.ToTable("SaleItem");
                });

            modelBuilder.Entity("WebApplication1.Models.SalePoint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDateTimeUtc")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("SalePoints");
                });

            modelBuilder.Entity("WebApplication1.Models.NumberProductCharacteristic", b =>
                {
                    b.HasBaseType("WebApplication1.Models.ProductCharacteristic");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasDiscriminator().HasValue("NumberProductCharacteristic");
                });

            modelBuilder.Entity("WebApplication1.Models.StringProductCharacteristic", b =>
                {
                    b.HasBaseType("WebApplication1.Models.ProductCharacteristic");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("StringProductCharacteristic_Value");

                    b.HasDiscriminator().HasValue("StringProductCharacteristic");
                });

            modelBuilder.Entity("WebApplication1.Models.ProductCharacteristic", b =>
                {
                    b.HasOne("WebApplication1.Models.Product", "Product")
                        .WithMany("ProductCharacteristics")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebApplication1.Models.SaleItem", b =>
                {
                    b.HasOne("WebApplication1.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Models.SalePoint", "SalePoint")
                        .WithMany("SaleItems")
                        .HasForeignKey("SalePointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("SalePoint");
                });

            modelBuilder.Entity("WebApplication1.Models.Product", b =>
                {
                    b.Navigation("ProductCharacteristics");
                });

            modelBuilder.Entity("WebApplication1.Models.SalePoint", b =>
                {
                    b.Navigation("SaleItems");
                });
#pragma warning restore 612, 618
        }
    }
}
