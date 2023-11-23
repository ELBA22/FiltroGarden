using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.DTOS
{
    public class PagoDto
    {
        public int CodigoCliente { get; set; }

        public string FormaPago { get; set; } = null!;

        public string IdTransaccion { get; set; } = null!;

        public DateOnly FechaPago { get; set; }

        public decimal Total { get; set; }

        public virtual Cliente CodigoClienteNavigation { get; set; } = null!;
    }
}