using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository
{
    public class ProductoRepository : GenericRepository<Producto>, IProducto
    {
        private readonly GardenContext _context;
        public ProductoRepository(GardenContext context) : base(context)
        {
            _context = context;
        }

        public Task<int> GetConsulta4()
        {
            var consulta = from producto in _context.Productos
                        join detalle in _context.DetallePedidos
                        on producto.CodigoProducto equals detalle.CodigoProducto
                        into detallesProducto
                        select detallesProducto.Sum(dp => dp.Cantidad);
            int resultado = consulta.FirstOrDefault();
            return Task.FromResult(resultado);
        }

    }
}