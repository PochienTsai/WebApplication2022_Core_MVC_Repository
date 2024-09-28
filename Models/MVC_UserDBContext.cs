﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApplication2022_Core_MVC_Repository.Models
{
    public partial class MVC_UserDBContext : DbContext
    {
        public MVC_UserDBContext()
        {
        }

        public MVC_UserDBContext(DbContextOptions<MVC_UserDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserTable> UserTables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=MVC_UserDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<UserTable>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserTable");

                entity.Property(e => e.UserBirthDay).HasColumnType("datetime");

                entity.Property(e => e.UserMobilePhone).HasMaxLength(15);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserSex)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'M')")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
