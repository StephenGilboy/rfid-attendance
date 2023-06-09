﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RfidAttendance.Web.Data;

#nullable disable

namespace RfidAttendance.Web.Migrations
{
    [DbContext(typeof(AttendanceContext))]
    [Migration("20230326021524_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("RfidAttendance.Web.Data.RfidTag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnName("id");

                    b.Property<bool>("IsCurrenltyInAttendance")
                        .HasColumnType("INTEGER")
                        .HasColumnName("is_currently_in_attendance");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("TEXT")
                        .HasColumnName("last_seen");

                    b.HasKey("Id");

                    b.ToTable("rfid_tags");
                });

            modelBuilder.Entity("RfidAttendance.Web.Data.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("last_name");

                    b.Property<string>("RfidTagId")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("rfid_tag_id");

                    b.HasKey("Id");

                    b.HasIndex("RfidTagId")
                        .IsUnique();

                    b.ToTable("students");
                });

            modelBuilder.Entity("RfidAttendance.Web.Data.Student", b =>
                {
                    b.HasOne("RfidAttendance.Web.Data.RfidTag", "RfidTag")
                        .WithOne("Student")
                        .HasForeignKey("RfidAttendance.Web.Data.Student", "RfidTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RfidTag");
                });

            modelBuilder.Entity("RfidAttendance.Web.Data.RfidTag", b =>
                {
                    b.Navigation("Student")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
