using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sport.API.Entities;

namespace Sport.API.Migrations
{
    [DbContext(typeof(SportContext))]
    [Migration("20171113211609_SportDBClientActivityEntityAdded")]
    partial class SportDBClientActivityEntityAdded
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
        }
    }
}
