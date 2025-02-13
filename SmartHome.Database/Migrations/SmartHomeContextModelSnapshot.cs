﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.Database;

#nullable disable

namespace SmartHome.Database.Migrations
{
    [DbContext(typeof(SmartHomeContext))]
    partial class SmartHomeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("JsonObjectConfig")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.DeviceAccess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SmartUserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("SmartUserId");

                    b.ToTable("DeviceAccesses");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.DeviceAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("char(36)");

                    b.Property<string>("JsonObjectConfig")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<Guid>("RoutineId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("RoutineId");

                    b.ToTable("DeviceAction");
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

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("CreateOn")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("SmartHomeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.HasKey("Id");

                    b.HasIndex("SmartHomeId");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<Guid>("SmartHomeId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("SmartHomeId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Routine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<byte[]>("RepeatDays")
                        .IsRequired()
                        .HasColumnType("binary(7)");

                    b.Property<Guid>("SmartHomeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("SmartHomeId");

                    b.ToTable("Routine");
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

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Device", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.DeviceAccess", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Common.Models.Entities.SmartUser", "SmartUser")
                        .WithMany()
                        .HasForeignKey("SmartUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("SmartUser");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.DeviceAction", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Common.Models.Entities.Routine", "Routine")
                        .WithMany()
                        .HasForeignKey("RoutineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Routine");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Log", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Home", "SmartHome")
                        .WithMany()
                        .HasForeignKey("SmartHomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SmartHome");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Room", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Home", "SmartHome")
                        .WithMany()
                        .HasForeignKey("SmartHomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SmartHome");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.Routine", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Home", "SmartHome")
                        .WithMany()
                        .HasForeignKey("SmartHomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SmartHome");
                });

            modelBuilder.Entity("SmartHome.Common.Models.Entities.SmartUser", b =>
                {
                    b.HasOne("SmartHome.Common.Models.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHome.Common.Models.Entities.Home", "SmartHome")
                        .WithMany()
                        .HasForeignKey("SmartHomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("SmartHome");
                });
#pragma warning restore 612, 618
        }
    }
}
