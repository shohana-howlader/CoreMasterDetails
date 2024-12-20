using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoreMasterDetails.Models;

public partial class CoreDbContext : DbContext
{
    public CoreDbContext()
    {
    }

    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PerformanceReview> PerformanceReviews { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=CoreDB;TrustServerCertificate=true; Trusted_Connection=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PerformanceReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__PerformanceReviews__C92D71A707928021");

            entity.Property(e => e.ReviewNotes)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departments__2B7477A7F6E4BE3F");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.Departments)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Departments__Employees__29572725");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employees__32C52B999596CBD4");

            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.PerformanceReview).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK__Employees__PerformanceReview__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
