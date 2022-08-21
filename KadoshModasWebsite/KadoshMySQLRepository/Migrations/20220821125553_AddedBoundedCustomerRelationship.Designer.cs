﻿// <auto-generated />
using System;
using KadoshRepository.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KadoshRepository.Migrations
{
    [DbContext(typeof(StoreDataContext))]
    [Migration("20220821125553_AddedBoundedCustomerRelationship")]
    partial class AddedBoundedCustomerRelationship
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("KadoshDomain.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BoundedToCustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob");

                    b.Property<int?>("PasswordSaltIterations")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("BoundedToCustomerId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("KadoshDomain.Entities.CustomerPosting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("PostingDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("SaleId");

                    b.ToTable("CustomersPostings");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Installment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("MaturityDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SettlementDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Situation")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("SaleId");

                    b.ToTable("Installments");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BarCode")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("BarCode")
                        .IsUnique();

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscountInPercentage")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("DownPayment")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("FormOfPayment")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SettlementDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Situation")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SellerId");

                    b.HasIndex("StoreId");

                    b.ToTable("Sales");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Sale");
                });

            modelBuilder.Entity("KadoshDomain.Entities.SaleItem", b =>
                {
                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Situation")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("DiscountInPercentage")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("SaleId", "ProductId", "Situation");

                    b.HasIndex("ProductId");

                    b.ToTable("SalesItems");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AmountInStock")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MinimumAmountBeforeLowStock")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Store");
                });

            modelBuilder.Entity("KadoshDomain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("PasswordSaltIterations")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KadoshDomain.Entities.SaleInCash", b =>
                {
                    b.HasBaseType("KadoshDomain.Entities.Sale");

                    b.HasDiscriminator().HasValue("SaleInCash");
                });

            modelBuilder.Entity("KadoshDomain.Entities.SaleInInstallments", b =>
                {
                    b.HasBaseType("KadoshDomain.Entities.Sale");

                    b.Property<decimal>("InterestOnTheTotalSaleInPercentage")
                        .HasColumnType("decimal(65,30)");

                    b.HasDiscriminator().HasValue("SaleInInstallments");
                });

            modelBuilder.Entity("KadoshDomain.Entities.SaleOnCredit", b =>
                {
                    b.HasBaseType("KadoshDomain.Entities.Sale");

                    b.HasDiscriminator().HasValue("SaleOnCredit");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Customer", b =>
                {
                    b.HasOne("KadoshDomain.Entities.Customer", "IsBoundedTo")
                        .WithMany("BoundedCustomers")
                        .HasForeignKey("BoundedToCustomerId");

                    b.OwnsOne("KadoshDomain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasMaxLength(50)
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("Complement")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("Neighborhood")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("Number")
                                .HasMaxLength(20)
                                .HasColumnType("varchar(20)");

                            b1.Property<string>("State")
                                .HasMaxLength(20)
                                .HasColumnType("varchar(20)");

                            b1.Property<string>("Street")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("ZipCode")
                                .HasMaxLength(10)
                                .HasColumnType("varchar(10)");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("KadoshDomain.ValueObjects.Document", "Document", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(18)
                                .HasColumnType("varchar(18)");

                            b1.Property<int>("Type")
                                .HasColumnType("int");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("KadoshDomain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<string>("EmailAddress")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsMany("KadoshDomain.ValueObjects.Phone", "Phones", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<int?>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<string>("AreaCode")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("varchar(3)");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("varchar(10)");

                            b1.Property<string>("TalkTo")
                                .HasColumnType("longtext");

                            b1.Property<int>("Type")
                                .HasColumnType("int");

                            b1.HasKey("CustomerId", "Id");

                            b1.ToTable("Phone");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Address");

                    b.Navigation("Document");

                    b.Navigation("Email");

                    b.Navigation("IsBoundedTo");

                    b.Navigation("Phones");
                });

            modelBuilder.Entity("KadoshDomain.Entities.CustomerPosting", b =>
                {
                    b.HasOne("KadoshDomain.Entities.Sale", "Sale")
                        .WithMany("Postings")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Installment", b =>
                {
                    b.HasOne("KadoshDomain.Entities.SaleInInstallments", "Sale")
                        .WithMany("Installments")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Product", b =>
                {
                    b.HasOne("KadoshDomain.Entities.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KadoshDomain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Sale", b =>
                {
                    b.HasOne("KadoshDomain.Entities.Customer", "Customer")
                        .WithMany("Sales")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KadoshDomain.Entities.User", "Seller")
                        .WithMany("Sales")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KadoshDomain.Entities.Store", "Store")
                        .WithMany("Sales")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Seller");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("KadoshDomain.Entities.SaleItem", b =>
                {
                    b.HasOne("KadoshDomain.Entities.Product", "Product")
                        .WithMany("SaleItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KadoshDomain.Entities.Sale", "Sale")
                        .WithMany("SaleItems")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Stock", b =>
                {
                    b.HasOne("KadoshDomain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KadoshDomain.Entities.Store", "Store")
                        .WithMany("Stocks")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Store", b =>
                {
                    b.OwnsOne("KadoshDomain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<int>("StoreId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasMaxLength(50)
                                .HasColumnType("varchar(50)");

                            b1.Property<string>("Complement")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("Neighborhood")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("Number")
                                .HasMaxLength(20)
                                .HasColumnType("varchar(20)");

                            b1.Property<string>("State")
                                .HasMaxLength(20)
                                .HasColumnType("varchar(20)");

                            b1.Property<string>("Street")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("ZipCode")
                                .HasMaxLength(10)
                                .HasColumnType("varchar(10)");

                            b1.HasKey("StoreId");

                            b1.ToTable("Store");

                            b1.WithOwner()
                                .HasForeignKey("StoreId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("KadoshDomain.Entities.User", b =>
                {
                    b.HasOne("KadoshDomain.Entities.Store", "Store")
                        .WithMany("Users")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Customer", b =>
                {
                    b.Navigation("BoundedCustomers");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Product", b =>
                {
                    b.Navigation("SaleItems");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Sale", b =>
                {
                    b.Navigation("Postings");

                    b.Navigation("SaleItems");
                });

            modelBuilder.Entity("KadoshDomain.Entities.Store", b =>
                {
                    b.Navigation("Sales");

                    b.Navigation("Stocks");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("KadoshDomain.Entities.User", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("KadoshDomain.Entities.SaleInInstallments", b =>
                {
                    b.Navigation("Installments");
                });
#pragma warning restore 612, 618
        }
    }
}