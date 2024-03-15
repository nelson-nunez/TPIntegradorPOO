using Integrador.BaseClas;
using Integrador.Dto;
using Integrador.Extensiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Integrador.Clases
{
    public class Gestion
    {
        #region MyRegion
        public ListRepository<Proveedor> crudProveedor { get; set; }
        public ListRepository<Pago> crudPagos { get; set; }

        public List<Proveedor> ListaProveedores { get; set; }
        public List<Pago> ListaPagos { get; set; }

        #endregion

        public Gestion()
        {
            crudProveedor = new ListRepository<Proveedor>();
            crudPagos= new ListRepository<Pago>();
            ListaProveedores = new List<Proveedor>();
            ListaPagos = new List<Pago>();
        }

        #region Proveedores
       
        public bool Create_Proveedor(Proveedor item)
        {
            Expression<Func<Proveedor, bool>> filter = p => p.Legajo == item.Legajo;
            return crudProveedor.CreateItem(ListaProveedores, item, filter);
        }
        
        public Proveedor Read_Proveedor(int id)
        {
            Expression<Func<Proveedor, bool>> filter = p => p.Legajo == id;
            return crudProveedor.ReadItem(ListaProveedores, filter);
        }

        public bool Delete_Proveedor(Proveedor item)
        {
            //Borro en pagos
            foreach (var a in item.Pagos)
            {
                a.Proveedor = null;
                a.Estado = Constantes.Pendiente;
                Expression<Func<Pago, bool>> filterP = p => p.Codigo == a.Codigo;
                var ttt = crudPagos.UpdateItem(ListaPagos, a, filterP);
            }
            //Borro en proveedores
            Expression<Func<Proveedor, bool>> filter = p => p.Legajo == item.Legajo;
            crudProveedor.DeleteItem(ListaProveedores, filter);
            return true;
        }
     
        public bool Update_Proveedor(Proveedor item)
        {
            //Actualizo proveedor
            Expression<Func<Proveedor, bool>> filter = p => p.Legajo == item.Legajo;
            var tt = crudProveedor.UpdateItem(ListaProveedores, item, filter);
            //Actualizo en pagos
            var proveedor = Read_Proveedor(item.Legajo);
            foreach (var a in proveedor.Pagos)
            {
                a.Proveedor = proveedor;
                Expression<Func<Pago, bool>> filterP = p => p.Codigo == a.Codigo;
                var ttt = crudPagos.UpdateItem(ListaPagos, a, filterP);
            }

            return true;
        }

        public List<ProveedorDTO> ListaProveedoresCustom(List<Proveedor> proveedores)
        {
            var q = from a in proveedores
                    select new ProveedorDTO
                    {
                        Legajo= a.Legajo,
                        Nombre = a.Nombre,
                    };
            return q.ToList<ProveedorDTO>();
        }

        public Proveedor VerificaryRetornarProveedor(DataGridView grid)
        {
            var item = grid.VerificarYRetornarSeleccion<ProveedorDTO>();
            return Read_Proveedor(item.Legajo);
        }

        #endregion

        #region Pagos

        public bool Create_Pago(Pago item)
        {
            Expression<Func<Pago, bool>> filter = p => p.Codigo == item.Codigo;
            item.PorcentajeRecargo = item.Recargo();
            return crudPagos.CreateItem(ListaPagos, item, filter);
        }

        public Pago Read_Pago(int codigo)
        {
            Expression<Func<Pago, bool>> filter = p => p.Codigo == codigo;
            return crudPagos.ReadItem(ListaPagos, filter);
        }

        public bool Delete_Pago(Pago item)
        {
            //Borro en los proveedores
            if(item.Proveedor != null)
            {
                var proveedor = Read_Proveedor(item.Proveedor.Legajo);
                if (proveedor != null)
                {
                    var pagoProveedor = proveedor.Pagos.Find(p => p.Codigo == item.Codigo);
                    proveedor.Pagos.Remove(pagoProveedor);
                    Update_Proveedor(proveedor);
                }

            }
            //Borro en pagos
            Expression<Func<Pago, bool>> filter = p => p.Codigo == item.Codigo;
            return crudPagos.DeleteItem(ListaPagos, filter);
        }

        public bool Update_Pago(Pago item)
        {
            // Actualizar pago
            Expression<Func<Pago, bool>> filter = p => p.Codigo == item.Codigo;
            var updated = crudPagos.UpdateItem(ListaPagos, item, filter);

            // Actualizar en proveedores
            var pago = Read_Pago(item.Codigo);
            if (pago.Proveedor != null)
            {
                var proveedor = Read_Proveedor(pago.Proveedor.Legajo);
                if (proveedor != null)
                {
                    var pagoProveedor = proveedor.Pagos.Find(p => p.Codigo == pago.Codigo);
                    if (pagoProveedor != null)
                    {
                        pagoProveedor.Importe = item.Importe;
                        pagoProveedor.Vencimiento = item.Vencimiento;
                        pagoProveedor.Estado = item.Estado;
                        pagoProveedor.PorcentajeRecargo = item.Recargo();
                    }
                }
            }

            return updated;
        }

        public bool Asignate_Pago(Pago item)
        {
            // Actualizar pago
            Expression<Func<Pago, bool>> filter = p => p.Codigo == item.Codigo;
            crudPagos.UpdateItem(ListaPagos, item, filter);

            // Actualizar en proveedores
            var proveedor = Read_Proveedor(item.Proveedor.Legajo);
            proveedor.AgregarPago(item);

            return true;
        }
        
        public bool Pagar_Pago(Pago item)
        {
            item.Estado = Constantes.Pagado;
            Update_Pago(item);
            return true;
        }

        public List<PagoDTO> ListaPagosCustom(List<Pago> pagos)
        {
            var q = from a in pagos
                    select new PagoDTO
                    {
                        Codigo = a.Codigo,
                        Vencimiento = a.Vencimiento.Date,
                        Importe = a.Importe,
                        Estado = a.Estado == Constantes.Pagado ? "Pagado" : "Pendiente",
                        Recargo = a.Importe * a.PorcentajeRecargo,
                        NombreProveedor = a.Proveedor == null ? "s/i" : Read_Proveedor(a.Proveedor.Legajo)?.Nombre,
                    };
            return q.ToList<PagoDTO>();
        }

        public Pago VerificaryRetornarPago(DataGridView grid)
        {
            var item = grid.VerificarYRetornarSeleccion<PagoDTO>();
            return Read_Pago(item.Codigo);
        }
      
        #endregion
    }
}

