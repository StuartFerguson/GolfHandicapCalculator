﻿// <auto-generated />
using System;
using ManagementAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ManagementAPI.Database.Migrations
{
    [DbContext(typeof(ManagementAPIReadModel))]
    [Migration("20190723195218_PlayersSignUpAndScoresCountInTournament")]
    partial class PlayersSignUpAndScoresCountInTournament
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ManagementAPI.Database.Models.GolfClub", b =>
                {
                    b.Property<Guid>("GolfClubId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("Name");

                    b.Property<string>("PostalCode");

                    b.Property<string>("Region");

                    b.Property<string>("TelephoneNumber");

                    b.Property<string>("Town");

                    b.Property<string>("WebSite");

                    b.HasKey("GolfClubId");

                    b.ToTable("GolfClub");
                });

            modelBuilder.Entity("ManagementAPI.Database.Models.PlayerClubMembership", b =>
                {
                    b.Property<Guid>("PlayerId");

                    b.Property<Guid>("GolfClubId");

                    b.Property<DateTime?>("AcceptedDateTime");

                    b.Property<string>("GolfClubName");

                    b.Property<Guid>("MembershipId");

                    b.Property<string>("MembershipNumber");

                    b.Property<DateTime?>("RejectedDateTime");

                    b.Property<string>("RejectionReason");

                    b.Property<int>("Status");

                    b.HasKey("PlayerId", "GolfClubId");

                    b.ToTable("PlayerClubMembership");
                });

            modelBuilder.Entity("ManagementAPI.Database.Models.Tournament", b =>
                {
                    b.Property<Guid>("TournamentId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Format");

                    b.Property<Guid>("GolfClubId");

                    b.Property<string>("GolfClubName");

                    b.Property<bool>("HasResultBeenProduced");

                    b.Property<Guid>("MeasuredCourseId");

                    b.Property<string>("MeasuredCourseName");

                    b.Property<int>("MeasuredCourseSSS");

                    b.Property<string>("MeasuredCourseTeeColour");

                    b.Property<string>("Name");

                    b.Property<int>("PlayerCategory");

                    b.Property<int>("PlayersScoredRecordedCount");

                    b.Property<int>("PlayersSignedUpCount");

                    b.Property<DateTime>("TournamentDate");

                    b.HasKey("TournamentId");

                    b.ToTable("Tournament");
                });

            modelBuilder.Entity("ManagementAPI.Database.Models.TournamentResultForPlayerScore", b =>
                {
                    b.Property<Guid>("TournamentResultForPlayerId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Division");

                    b.Property<int>("DivisionPosition");

                    b.Property<int>("GrossScore");

                    b.Property<decimal>("Last3Holes");

                    b.Property<decimal>("Last6Holes");

                    b.Property<decimal>("Last9Holes");

                    b.Property<int>("NetScore");

                    b.Property<Guid>("PlayerId");

                    b.Property<int>("PlayingHandicap");

                    b.Property<Guid>("TournamentId");

                    b.HasKey("TournamentResultForPlayerId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentResultForPlayerScore");
                });

            modelBuilder.Entity("ManagementAPI.Database.Models.User", b =>
                {
                    b.Property<Guid>("GolfClubId");

                    b.Property<Guid>("UserId");

                    b.Property<string>("Email");

                    b.Property<string>("FamilyName");

                    b.Property<string>("GivenName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("UserName");

                    b.Property<string>("UserType");

                    b.HasKey("GolfClubId", "UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ManagementAPI.Database.Models.TournamentResultForPlayerScore", b =>
                {
                    b.HasOne("ManagementAPI.Database.Models.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
