﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerApp.Data;

#nullable disable

namespace ServerApp.Migrations
{
    [DbContext(typeof(ServerDbContext))]
    [Migration("20231124214020_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SharedLib.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("TableId")
                        .HasColumnType("int")
                        .HasColumnName("Table number");

                    b.HasKey("Id");

                    b.HasIndex("TableId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("SharedLib.Models.Operator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("TableId")
                        .HasColumnType("int")
                        .HasColumnName("Table number");

                    b.HasKey("Id");

                    b.HasIndex("TableId")
                        .IsUnique();

                    b.ToTable("Operators");
                });

            modelBuilder.Entity("SharedLib.Models.Table", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OperatorId")
                        .HasColumnType("int")
                        .HasColumnName("Operator Id");

                    b.Property<int>("TableNumber")
                        .HasColumnType("int")
                        .HasColumnName("Table number");

                    b.HasKey("Id");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("SharedLib.Models.Client", b =>
                {
                    b.HasOne("SharedLib.Models.Table", "Table")
                        .WithMany("Clients")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Table");
                });

            modelBuilder.Entity("SharedLib.Models.Operator", b =>
                {
                    b.HasOne("SharedLib.Models.Table", "Table")
                        .WithOne("Operator")
                        .HasForeignKey("SharedLib.Models.Operator", "TableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Table");
                });

            modelBuilder.Entity("SharedLib.Models.Table", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("Operator")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}