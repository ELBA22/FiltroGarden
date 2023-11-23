using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Data;

public partial class GardenContext : DbContext
{
    public GardenContext()
    {
    }

    public GardenContext(DbContextOptions<GardenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<GamaProducto> GamaProductos { get; set; }

    public virtual DbSet<Oficina> Oficinas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cliente>(entity =>
        {

        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {

        });

        modelBuilder.Entity<Empleado>(entity =>
        {

        });

        modelBuilder.Entity<GamaProducto>(entity =>
        {

        });

        modelBuilder.Entity<Oficina>(entity =>
        {

        });

        modelBuilder.Entity<Pago>(entity =>
        {

        });

        modelBuilder.Entity<Pedido>(entity =>
        {

        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.CodigoProducto).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.Gama, "FK_producto_gama_gama_producto_gama");

            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(15)
                .HasColumnName("codigo_producto");
            entity.Property(e => e.CantidadEnStock).HasColumnName("cantidad_en_stock");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Dimensiones)
                .HasMaxLength(25)
                .HasColumnName("dimensiones");
            entity.Property(e => e.Gama)
                .HasMaxLength(50)
                .HasColumnName("gama");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioProveedor)
                .HasPrecision(15, 2)
                .HasColumnName("precio_proveedor");
            entity.Property(e => e.PrecioVenta)
                .HasPrecision(15, 2)
                .HasColumnName("precio_venta");
            entity.Property(e => e.Proveedor)
                .HasMaxLength(50)
                .HasColumnName("proveedor");

            entity.HasOne(d => d.GamaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Gama)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_producto_gama_gama_producto_gama");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
