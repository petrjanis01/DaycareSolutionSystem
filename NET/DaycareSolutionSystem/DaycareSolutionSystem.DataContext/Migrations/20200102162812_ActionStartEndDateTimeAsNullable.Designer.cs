﻿// <auto-generated />
using System;
using DaycareSolutionSystem.Database.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DaycareSolutionSystem.Database.DataContext.Migrations
{
    [DbContext(typeof(DssDataContext))]
    [Migration("20200102162812_ActionStartEndDateTimeAsNullable")]
    partial class ActionStartEndDateTimeAsNullable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.Action", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("GeneralDescription")
                        .IsRequired()
                        .HasColumnType("character varying(5000)")
                        .HasMaxLength(5000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(512)")
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.ToTable("Action");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BuildingNumber")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("City")
                        .HasColumnType("character varying(512)")
                        .HasMaxLength(512);

                    b.Property<string>("GpsCoordinates")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PostCode")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Street")
                        .HasColumnType("character varying(512)")
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.AgreedClientAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ActionId")
                        .HasColumnType("uuid");

                    b.Property<string>("ClientActionSpecificDescription")
                        .HasColumnType("text");

                    b.Property<int>("Day")
                        .HasColumnType("integer");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<int>("EstimatedDurationMinutes")
                        .HasColumnType("integer");

                    b.Property<Guid>("IndividualPlanId")
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("PlannedStartTime")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("IndividualPlanId");

                    b.ToTable("AgreedClientAction");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ProfilePictureId")
                        .HasColumnType("uuid");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("ProfilePictureId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EmployeePosition")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ProfilePictureId")
                        .HasColumnType("uuid");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProfilePictureId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.IndividualPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ValidFromDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ValidUntilDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("IndividualPlan");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("BinaryData")
                        .HasColumnType("bytea");

                    b.Property<string>("MimeType")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.RegisteredClientAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ActionFinishedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("ActionStartedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("AgreedClientActionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("character varying(5000)")
                        .HasMaxLength(5000);

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("PhotoId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PlannedStartDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("AgreedClientActionId");

                    b.HasIndex("ClientId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PhotoId");

                    b.ToTable("RegisteredClientAction");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginName")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Password")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LoginName")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.AgreedClientAction", b =>
                {
                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Action", "Action")
                        .WithMany("AgreedClientActions")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Employee", "Employee")
                        .WithMany("AgreedClientActions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.IndividualPlan", "IndividualPlan")
                        .WithMany("AgreedClientActions")
                        .HasForeignKey("IndividualPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.Client", b =>
                {
                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Picture", "ProfilePicture")
                        .WithMany()
                        .HasForeignKey("ProfilePictureId");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.Employee", b =>
                {
                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Picture", "ProfilePicture")
                        .WithMany()
                        .HasForeignKey("ProfilePictureId");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.IndividualPlan", b =>
                {
                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Client", "Client")
                        .WithMany("IndividualPlans")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.RegisteredClientAction", b =>
                {
                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.AgreedClientAction", "AgreedClientAction")
                        .WithMany("RegisteredClientActions")
                        .HasForeignKey("AgreedClientActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Picture", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("DaycareSolutionSystem.Database.Entities.Entities.User", b =>
                {
                    b.HasOne("DaycareSolutionSystem.Database.Entities.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
