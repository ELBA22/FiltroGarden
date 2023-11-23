using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository
{
    public class GamaProductoRepository : GenericRepository<GamaProducto>, IGamaProducto
    {
        private readonly GardenContext _context;
        public GamaProductoRepository(GardenContext context) : base(context)
        {
            _context = context;
        }
    }
}