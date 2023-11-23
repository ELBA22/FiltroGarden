using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.DTOS
{
    public class DetallePedidoDto
    {
        public int CodigoPedido { get; set; }

        public string CodigoProducto { get; set; } = null!;

        public int Cantidad { get; set; }

        public decimal PrecioUnidad { get; set; }

        public short NumeroLinea { get; set; }

        public virtual Pedido CodigoPedidoNavigation { get; set; } = null!;

        public virtual Producto CodigoProductoNavigation { get; set; } = null!;
    }
}