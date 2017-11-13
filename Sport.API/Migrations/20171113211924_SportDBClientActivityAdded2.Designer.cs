using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sport.API.Entities;

namespace Sport.API.Migrations
{
    [DbContext(typeof(SportContext))]
    [Migration("20171113211924_SportDBClientActivityAdded2")]
    partial class SportDBClientActivityAdded2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sport.API.Entities.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Beginning");

                    b.Property<DateTime>("Ending");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TrainerId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Sport.API.Entities.ClientActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityId");

                    b.Property<string>("ClientId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("ClientActivities");
                });

            modelBuilder.Entity("Sport.API.Entities.ClientActivity", b =>
                {
                    b.HasOne("Sport.API.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
