﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SurfsUp.Data;

#nullable disable

namespace SurfsUp.Migrations
{
    [DbContext(typeof(SurfsUpContext))]
    partial class SurfsUpContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SurfsUp.Models.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Equipments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Length")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<float>("Thickness")
                        .HasColumnType("real");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<float>("Volume")
                        .HasColumnType("real");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Board");
                });

            modelBuilder.Entity("SurfsUp.Models.Rent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndRent")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartRent")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BoardId")
                        .IsUnique();

                    b.ToTable("Rent");
                });

            modelBuilder.Entity("SurfsUp.Models.Rent", b =>
                {
                    b.HasOne("SurfsUp.Models.Board", "Board")
                        .WithOne("Rent")
                        .HasForeignKey("SurfsUp.Models.Rent", "BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("SurfsUp.Models.Board", b =>
                {
                    b.Navigation("Rent");
                });
#pragma warning restore 612, 618
        }
    }
}
