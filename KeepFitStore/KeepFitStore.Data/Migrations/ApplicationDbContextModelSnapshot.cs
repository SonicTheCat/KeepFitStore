﻿// <auto-generated />
using System;
using KeepFitStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KeepFitStore.Data.Migrations
{
    [DbContext(typeof(KeepFitDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KeepFitStore.Domain.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<string>("StreetName");

                    b.Property<int>("StreetNumber");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Basket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("KeepFitStore.Domain.BasketItem", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("BasketId");

                    b.Property<int>("Quantity");

                    b.HasKey("ProductId", "BasketId");

                    b.HasIndex("BasketId");

                    b.ToTable("BasketItems");
                });

            modelBuilder.Entity("KeepFitStore.Domain.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("PostCode");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("KeepFitStore.Domain.KeepFitUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("AddressId");

                    b.Property<int>("BasketId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("BasketId")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressId");

                    b.Property<DateTime?>("DeliveryDate");

                    b.Property<decimal>("DeliveryPrice");

                    b.Property<int>("DeliveryType");

                    b.Property<string>("KeepFitUserId");

                    b.Property<DateTime?>("OrderDate");

                    b.Property<int>("PaymentType");

                    b.Property<int>("Status");

                    b.Property<decimal>("TotalPrice");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("KeepFitUserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("KeepFitStore.Domain.ProductOrder", b =>
                {
                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<int>("ProductQuantity");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOrders");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsOnSale");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductType");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Product");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<int>("GivenRating");

                    b.Property<string>("KeepFitUserId");

                    b.Property<int>("ProductId");

                    b.Property<DateTime>("PublishedOn");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("KeepFitUserId");

                    b.HasIndex("ProductId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Products.Supplement", b =>
                {
                    b.HasBaseType("KeepFitStore.Domain.Products.Product");

                    b.Property<string>("Directions");

                    b.Property<bool>("IsSuatableForVegans");

                    b.HasDiscriminator().HasValue("Supplement");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Products.AminoAcid", b =>
                {
                    b.HasBaseType("KeepFitStore.Domain.Products.Supplement");

                    b.Property<double>("Carbohydrate")
                        .HasColumnName("AminoAcidCarbohydrate");

                    b.Property<double>("EnergyPerServing")
                        .HasColumnName("AminoAcidEnergyPerServing");

                    b.Property<double>("Fat")
                        .HasColumnName("AminoAcidFat");

                    b.Property<double>("ProteinPerServing")
                        .HasColumnName("AminoAcidProteinPerServing");

                    b.Property<double>("Salt")
                        .HasColumnName("AminoAcidSalt");

                    b.Property<int>("Type")
                        .HasColumnName("AminoAcidType");

                    b.HasDiscriminator().HasValue("AminoAcid");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Products.Creatine", b =>
                {
                    b.HasBaseType("KeepFitStore.Domain.Products.Supplement");

                    b.Property<int>("Type")
                        .HasColumnName("CreatineType");

                    b.HasDiscriminator().HasValue("Creatine");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Products.Protein", b =>
                {
                    b.HasBaseType("KeepFitStore.Domain.Products.Supplement");

                    b.Property<double>("Carbohydrate")
                        .HasColumnName("ProteinCarbohydrate");

                    b.Property<double>("EnergyPerServing")
                        .HasColumnName("ProteinEnergyPerServing");

                    b.Property<double>("Fat")
                        .HasColumnName("ProteinFat");

                    b.Property<double>("Fibre")
                        .HasColumnName("ProteinFibre");

                    b.Property<double>("ProteinPerServing")
                        .HasColumnName("ProteinProteinPerServing");

                    b.Property<double>("Salt")
                        .HasColumnName("ProteinSalt");

                    b.Property<int>("Type")
                        .HasColumnName("ProteinType");

                    b.HasDiscriminator().HasValue("Protein");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Products.Vitamin", b =>
                {
                    b.HasBaseType("KeepFitStore.Domain.Products.Supplement");

                    b.Property<int>("Type")
                        .HasColumnName("VitaminType");

                    b.HasDiscriminator().HasValue("Vitamin");
                });

            modelBuilder.Entity("KeepFitStore.Domain.Address", b =>
                {
                    b.HasOne("KeepFitStore.Domain.City", "City")
                        .WithMany("Addresses")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KeepFitStore.Domain.BasketItem", b =>
                {
                    b.HasOne("KeepFitStore.Domain.Basket", "Basket")
                        .WithMany("BasketItems")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KeepFitStore.Domain.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KeepFitStore.Domain.KeepFitUser", b =>
                {
                    b.HasOne("KeepFitStore.Domain.Address", "Address")
                        .WithMany("KeepFitUsers")
                        .HasForeignKey("AddressId");

                    b.HasOne("KeepFitStore.Domain.Basket", "Basket")
                        .WithOne("KeepFitUser")
                        .HasForeignKey("KeepFitStore.Domain.KeepFitUser", "BasketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KeepFitStore.Domain.Order", b =>
                {
                    b.HasOne("KeepFitStore.Domain.Address", "DeliveryAddress")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("KeepFitStore.Domain.KeepFitUser", "KeepFitUser")
                        .WithMany("Orders")
                        .HasForeignKey("KeepFitUserId");
                });

            modelBuilder.Entity("KeepFitStore.Domain.ProductOrder", b =>
                {
                    b.HasOne("KeepFitStore.Domain.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KeepFitStore.Domain.Products.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KeepFitStore.Domain.Review", b =>
                {
                    b.HasOne("KeepFitStore.Domain.KeepFitUser", "KeepFitUser")
                        .WithMany("Reviews")
                        .HasForeignKey("KeepFitUserId");

                    b.HasOne("KeepFitStore.Domain.Products.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("KeepFitStore.Domain.KeepFitUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("KeepFitStore.Domain.KeepFitUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KeepFitStore.Domain.KeepFitUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("KeepFitStore.Domain.KeepFitUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
