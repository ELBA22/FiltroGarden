using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository
{
    public class PedidoRepository : GenericRepository<Pedido>, IPedido
    {
        private readonly GardenContext _context;
        public PedidoRepository(GardenContext context) : base(context)
        {
            _context = context;
        }


        public Task<IQueryable<string>> getConsulta1()
        {
            var consulta = from pedido in _context.Pedidos
                        where pedido.FechaEsperada < pedido.FechaEntrega
                        select pedido.CodigoPedido + " " + pedido.CodigoCliente + " " + pedido.FechaEsperada + " " + pedido.FechaEntrega;

            return Task.FromResult(consulta);
        }
    }
}