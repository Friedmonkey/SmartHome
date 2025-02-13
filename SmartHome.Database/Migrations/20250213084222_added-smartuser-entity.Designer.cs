﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.Database;

#nullable disable

namespace SmartHome.Database.Migrations
{
    [DbContext(typeof(SmartHomeContext))]
    [Migration("20250213084222_added-smartuser-entity")]
    partial class addedsmartuserentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PasswordHashed")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Home", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("SSID")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("SSPassword")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Home");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.SmartUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<Guid>("SmartHomeId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("SmartHomeId");

                    b.ToTable("SmartUser");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.SmartUser", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SmartHome.Common.Models.Entities.Home", "SmartHome")
                        .WithMany()
                        .HasForeignKey("SmartHomeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("SmartHome");
                });
#pragma warning restore 612, 618
        }
    }
}
