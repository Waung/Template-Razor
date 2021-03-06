﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using appglobal.models;

namespace gls_core_skeleton.Migrations
{
    [DbContext(typeof(gls_model))]
    partial class gls_modelModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("appglobal.models.feature_map", b =>
                {
                    b.Property<int>("feature_map_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("feature_map_id");

                    b.Property<int>("m_feature_id")
                        .HasColumnName("m_feature_id");

                    b.Property<int>("m_user_group_id")
                        .HasColumnName("m_user_group_id");

                    b.HasKey("feature_map_id");

                    b.HasIndex("m_feature_id");

                    b.HasIndex("m_user_group_id");

                    b.ToTable("feature_map");
                });

            modelBuilder.Entity("appglobal.models.m_feature", b =>
                {
                    b.Property<int>("m_feature_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("m_feature_id");

                    b.Property<string>("feature_icon")
                        .IsRequired()
                        .HasColumnName("feature_icon")
                        .HasMaxLength(50);

                    b.Property<string>("feature_name")
                        .IsRequired()
                        .HasColumnName("feature_name")
                        .HasMaxLength(100);

                    b.Property<bool>("feature_private")
                        .HasColumnName("feature_private");

                    b.Property<int>("feature_sequence")
                        .HasColumnName("feature_sequence");

                    b.Property<string>("feature_url")
                        .IsRequired()
                        .HasColumnName("feature_url")
                        .HasMaxLength(255);

                    b.Property<int>("m_feature_group_id")
                        .HasColumnName("m_feature_group_id");

                    b.HasKey("m_feature_id");

                    b.HasIndex("m_feature_group_id");

                    b.ToTable("m_feature");
                });

            modelBuilder.Entity("appglobal.models.m_feature_group", b =>
                {
                    b.Property<int>("m_feature_group_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("m_feature_group_id");

                    b.Property<string>("feature_group_name")
                        .IsRequired()
                        .HasColumnName("feature_group_name")
                        .HasMaxLength(100);

                    b.HasKey("m_feature_group_id");

                    b.ToTable("m_feature_group");
                });

            modelBuilder.Entity("appglobal.models.m_parameter", b =>
                {
                    b.Property<int>("m_parameter_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("m_parameter_id");

                    b.Property<string>("parameter_group")
                        .IsRequired()
                        .HasColumnName("parameter_group");

                    b.Property<string>("parameter_key")
                        .IsRequired()
                        .HasColumnName("parameter_key");

                    b.Property<string>("parameter_value")
                        .IsRequired()
                        .HasColumnName("parameter_value");

                    b.HasKey("m_parameter_id");

                    b.ToTable("m_parameters");
                });

            modelBuilder.Entity("appglobal.models.m_user", b =>
                {
                    b.Property<int>("m_user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("m_user_id");

                    b.Property<int>("m_user_group_id")
                        .HasColumnName("m_user_group_id");

                    b.Property<bool>("user_active")
                        .HasColumnName("user_active");

                    b.Property<string>("user_email")
                        .HasColumnName("user_email")
                        .HasMaxLength(50);

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnName("user_name")
                        .HasMaxLength(50);

                    b.Property<string>("user_password")
                        .IsRequired()
                        .HasColumnName("user_password")
                        .HasMaxLength(100);

                    b.HasKey("m_user_id");

                    b.HasIndex("m_user_group_id");

                    b.ToTable("m_user");
                });

            modelBuilder.Entity("appglobal.models.m_user_group", b =>
                {
                    b.Property<int>("m_user_group_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("m_user_group_id");

                    b.Property<bool>("user_group_default")
                        .HasColumnName("user_group_default");

                    b.Property<string>("user_group_name")
                        .IsRequired()
                        .HasColumnName("user_group_name")
                        .HasMaxLength(100);

                    b.HasKey("m_user_group_id");

                    b.ToTable("m_user_group");
                });

            modelBuilder.Entity("appglobal.models.feature_map", b =>
                {
                    b.HasOne("appglobal.models.m_feature", "m_feature")
                        .WithMany("feature_map")
                        .HasForeignKey("m_feature_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("appglobal.models.m_user_group", "m_user_group")
                        .WithMany("feature_map")
                        .HasForeignKey("m_user_group_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("appglobal.models.m_feature", b =>
                {
                    b.HasOne("appglobal.models.m_feature_group", "m_feature_group")
                        .WithMany("m_feature")
                        .HasForeignKey("m_feature_group_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("appglobal.models.m_user", b =>
                {
                    b.HasOne("appglobal.models.m_user_group", "m_user_group")
                        .WithMany("m_user")
                        .HasForeignKey("m_user_group_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
