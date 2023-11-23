using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallePedido>
    {
        public void Configure(EntityTypeBuilder<DetallePedido> builder)
        {
            builder.HasKey(e => new { e.CodigoPedido, e.CodigoProducto })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            builder.ToTable("detalle_pedido");

            builder.HasIndex(e => e.CodigoProducto, "FK_detalle_pedido_codigo_producto_producto_codigo_producto");

            builder.Property(e => e.CodigoPedido).HasColumnName("codigo_pedido");
            builder.Property(e => e.CodigoProducto)
                .HasMaxLength(15)
                .HasColumnName("codigo_producto");
            builder.Property(e => e.Cantidad).HasColumnName("cantidad");
            builder.Property(e => e.NumeroLinea).HasColumnName("numero_linea");
            builder.Property(e => e.PrecioUnidad)
                .HasPrecision(15, 2)
                .HasColumnName("precio_unidad");

            builder.HasOne(d => d.CodigoPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.CodigoPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_detalle_pedido_codigo_pedido_pedido_codigo_pedido");

            builder.HasOne(d => d.CodigoProductoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.CodigoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_detalle_pedido_codigo_producto_producto_codigo_producto");
        }
    }
}