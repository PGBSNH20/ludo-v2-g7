﻿// <auto-generated />
using System;
using LudoV2Api.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LudoV2Api.Migrations
{
    [DbContext(typeof(LudoContext))]
    [Migration("20210518105745_updated-games-table")]
    partial class updatedgamestable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LudoV2Api.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CurrentTurn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FirstPlace")
                        .HasColumnType("int");

                    b.Property<int?>("FourthPlace")
                        .HasColumnType("int");

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastSaved")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfPlayers")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerGameGameId")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerGamePlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("SecondPlace")
                        .HasColumnType("int");

                    b.Property<int?>("ThirdPlace")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerGameGameId", "PlayerGamePlayerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("LudoV2Api.Models.Pawn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EligibleForWin")
                        .HasColumnType("bit");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Pawns");
                });

            modelBuilder.Entity("LudoV2Api.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PlayerGameGameId")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerGamePlayerId")
                        .HasColumnType("int");

                    b.Property<string>("PlayerName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PlayerGameGameId", "PlayerGamePlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("LudoV2Api.Models.PlayerGame", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("GameId", "PlayerId");

                    b.ToTable("GamePlayers");
                });

            modelBuilder.Entity("LudoV2Api.Models.Game", b =>
                {
                    b.HasOne("LudoV2Api.Models.PlayerGame", null)
                        .WithMany("Game")
                        .HasForeignKey("PlayerGameGameId", "PlayerGamePlayerId");
                });

            modelBuilder.Entity("LudoV2Api.Models.Pawn", b =>
                {
                    b.HasOne("LudoV2Api.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("LudoV2Api.Models.Player", b =>
                {
                    b.HasOne("LudoV2Api.Models.PlayerGame", null)
                        .WithMany("Player")
                        .HasForeignKey("PlayerGameGameId", "PlayerGamePlayerId");
                });

            modelBuilder.Entity("LudoV2Api.Models.PlayerGame", b =>
                {
                    b.Navigation("Game");

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
