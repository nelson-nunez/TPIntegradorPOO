using Integrador.BaseClas;
using Integrador.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integrador.Dto
{
    public class PagoDTO
    {
        public int Codigo { get; set; }

        public DateTime Vencimiento { get; set; }

        public decimal Recargo { get; set; }
   
        public decimal Importe { get; set; }

        public string Estado { get; set; }

        public string NombreProveedor { get; set; }
    }
}
