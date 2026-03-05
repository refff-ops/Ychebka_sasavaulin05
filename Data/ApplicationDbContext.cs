using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using vaulin_up.Models;

namespace vaulin_up.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductWorkshop> ProductWorkshops { get; set; }

    public virtual DbSet<Workshop> Workshops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.MaterialTypeId).HasName("material_type_pkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.HasOne(d => d.MainMaterialType).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_main_material_type_id_fkey");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_product_type_id_fkey");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("product_type_pkey");
        });

        modelBuilder.Entity<ProductWorkshop>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.WorkshopId }).HasName("product_workshop_pkey");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductWorkshops).HasConstraintName("product_workshop_product_id_fkey");

            entity.HasOne(d => d.Workshop).WithMany(p => p.ProductWorkshops)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_workshop_workshop_id_fkey");
        });

        modelBuilder.Entity<Workshop>(entity =>
        {
            entity.HasKey(e => e.WorkshopId).HasName("workshop_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
