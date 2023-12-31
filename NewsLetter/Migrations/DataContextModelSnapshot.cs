﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsLetter.Models.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewsLetter.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NewsLetter.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("NewsId")
                        .HasColumnType("bigint");

                    b.Property<string>("Owner")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("NewsLetter.Models.News", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("NewsLetter.Models.Topic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<long?>("NewsId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ParentTopicId")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ParentTopicId");

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("NewsLetter.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AuthToken")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<DateTime>("TokenExpire")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NewsLetter.Models.Comment", b =>
                {
                    b.HasOne("NewsLetter.Models.News", null)
                        .WithMany("Comments")
                        .HasForeignKey("NewsId");
                });

            modelBuilder.Entity("NewsLetter.Models.News", b =>
                {
                    b.HasOne("NewsLetter.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("NewsLetter.Models.Topic", b =>
                {
                    b.HasOne("NewsLetter.Models.News", null)
                        .WithMany("topic")
                        .HasForeignKey("NewsId");

                    b.HasOne("NewsLetter.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("NewsLetter.Models.Topic", "ParentTopic")
                        .WithMany()
                        .HasForeignKey("ParentTopicId");

                    b.Navigation("Owner");

                    b.Navigation("ParentTopic");
                });

            modelBuilder.Entity("NewsLetter.Models.News", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("topic");
                });
#pragma warning restore 612, 618
        }
    }
}
