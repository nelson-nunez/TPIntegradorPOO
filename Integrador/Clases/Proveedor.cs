using Integrador.BaseClas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Integrador.Clases
{
    public class Proveedor: ClaseBase
    {
        public int Legajo {  get; set; }
        
        public string Nombre { get; set; }

        public List<Pago> Pagos { get; set; }

        public Proveedor(int legajo, string nombre)
        {
            Legajo = legajo;
            Nombre = nombre;
            Pagos = new List<Pago>();
        }

        public void AgregarPago(Pago pago)
        {
            if (Pagos != null && Pagos.Any())
            {
                if (Pagos.Contains(pago))
                    throw new Exception("El pago ya se encuentra asignado al vendedor");
                if (Pagos.Count(p => p.Estado == Constantes.Pendiente) > 2)
                    throw new Exception("No puede tener mas de dos Pago pendientes");
            }
            Pagos.Add(pago);
        }
      
        public void SobreescribirPago(List<Pago> pago)
        {
            Pagos.Clear();
            Pagos.AddRange(pago);
        }
        
        public void EliminarPago(Pago pago)
        {
            var pendientes = Pagos.Any(x=>x.Codigo == pago.Codigo);
            if (!pendientes)
                throw new Exception("El pago que desea eliminar no existe");
            else
                Pagos.Remove(pago);
        }
    }
}
