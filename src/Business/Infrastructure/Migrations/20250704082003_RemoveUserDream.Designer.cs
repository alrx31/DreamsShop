﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250704082003_RemoveUserDream")]
    partial class RemoveUserDream
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entity.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Domain.Entity.Dream", b =>
                {
                    b.Property<Guid>("DreamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("ImageFileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ProducerId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Rating")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("DreamId");

                    b.ToTable("Dream");
                });

            modelBuilder.Entity("Domain.Entity.DreamCategory", b =>
                {
                    b.Property<Guid>("DreamId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("DreamId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("DreamCategory");
                });

            modelBuilder.Entity("Domain.Entity.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Entity.OrderDream", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DreamId")
                        .HasColumnType("uuid");

                    b.HasKey("OrderId", "DreamId");

                    b.HasIndex("DreamId");

                    b.ToTable("OrderDreams");
                });

            modelBuilder.Entity("Domain.Entity.DreamCategory", b =>
                {
                    b.HasOne("Domain.Entity.Category", "Category")
                        .WithMany("DreamCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entity.Dream", "Dream")
                        .WithMany("DreamCategories")
                        .HasForeignKey("DreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Dream");
                });

            modelBuilder.Entity("Domain.Entity.OrderDream", b =>
                {
                    b.HasOne("Domain.Entity.Dream", "Dream")
                        .WithMany("OrderDreams")
                        .HasForeignKey("DreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entity.Order", "Order")
                        .WithMany("OrderDreams")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dream");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Domain.Entity.Category", b =>
                {
                    b.Navigation("DreamCategories");
                });

            modelBuilder.Entity("Domain.Entity.Dream", b =>
                {
                    b.Navigation("DreamCategories");

                    b.Navigation("OrderDreams");
                });

            modelBuilder.Entity("Domain.Entity.Order", b =>
                {
                    b.Navigation("OrderDreams");
                });
#pragma warning restore 612, 618
        }
    }
}
