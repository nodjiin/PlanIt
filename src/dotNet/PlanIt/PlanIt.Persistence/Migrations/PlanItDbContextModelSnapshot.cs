﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlanIt.Persistence;

#nullable disable

namespace PlanIt.Persistence.Migrations
{
    [DbContext(typeof(PlanItDbContext))]
    partial class PlanItDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PlanIt.Domain.Entities.Availability", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Availabilities");
                });

            modelBuilder.Entity("PlanIt.Domain.Entities.Plan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FirstSchedulableDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastSchedulableDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("PlanIt.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlanId", "Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlanIt.Domain.Entities.Availability", b =>
                {
                    b.HasOne("PlanIt.Domain.Entities.User", null)
                        .WithMany("Availabilities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlanIt.Domain.Entities.User", b =>
                {
                    b.HasOne("PlanIt.Domain.Entities.Plan", null)
                        .WithMany("Users")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlanIt.Domain.Entities.Plan", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("PlanIt.Domain.Entities.User", b =>
                {
                    b.Navigation("Availabilities");
                });
#pragma warning restore 612, 618
        }
    }
}
