using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            entity.HasKey(e => e.CodigoPedido).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.CodigoCliente, "FK_pedido_codigo_cliente_cliente_codigo_cliente");

            entity.Property(e => e.CodigoPedido)
                .ValueGeneratedNever()
                .HasColumnName("codigo_pedido");
            entity.Property(e => e.CodigoCliente).HasColumnName("codigo_cliente");
            entity.Property(e => e.Comentarios)
                .HasColumnType("text")
                .HasColumnName("comentarios");
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .HasColumnName("estado");
            entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaEsperada).HasColumnName("fecha_esperada");
            entity.Property(e => e.FechaPedido).HasColumnName("fecha_pedido");

            entity.HasOne(d => d.CodigoClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.CodigoCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pedido_codigo_cliente_cliente_codigo_cliente");
        }
    }
}