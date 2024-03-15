using Integrador.BaseClas;
using Integrador.Clases;
using Integrador.Dto;
using Integrador.Extensiones;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Integrador
{
    public partial class Form1 : Form
    {
        Gestion g;

        public Form1()
        {
            InitializeComponent();

            g = new Gestion();
            //Cargo preferencias
            dataGridView1.ConfigurarGrids();
            dataGridView2.ConfigurarGrids();
            dataGridView3.ConfigurarGrids();
            dataGridView4.ConfigurarGrids();
            dataGridView5.ConfigurarGrids();

            #region Ejemplos
            // Crear un proveedor de ejemplo
            Proveedor proveedorEjemplo = new Proveedor(12345678, "Proveedor de Ejemplo");
            g.Create_Proveedor(proveedorEjemplo);
            // Crear un pago de ejemplo
            Pago pagoEjemplo = new Efectivo(1, DateTime.Now.AddDays(-7), 1500, Constantes.Pendiente);
            g.Create_Pago(pagoEjemplo);
            // Crear un pago de ejemplo
            Pago pagoEjemplo2 = new Efectivo(2, DateTime.Now.AddDays(-7), 1500, Constantes.Pendiente);
            g.Create_Pago(pagoEjemplo2);

            //Proveedores
            dataGridView1.Mostrar(g.ListaProveedoresCustom(g.ListaProveedores));
            //Pagos
            dataGridView2.Mostrar(g.ListaPagosCustom(g.ListaPagos));
            
            ActualizarGrids();
            #endregion
        }

        #region Proveedores

        //Agregar proveedor
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var legajo = Interaction.InputBox("Nro de Legajo (Max 8 dígitos numéricos): ");
                if (!Information.IsNumeric(legajo) || legajo.Length > 8)
                    throw new Exception("El Legajo debe ser numérico y debe tener como máx 8 dígitos");
                var nombre = Interaction.InputBox("Razón Social: ");

                g.Create_Proveedor(new Proveedor(Convert.ToInt32(legajo), nombre));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dataGridView1.Mostrar(g.ListaProveedoresCustom(g.ListaProveedores));
            }
        }
   
        //Modificar proveedor
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var itemseleccionado = g.VerificaryRetornarProveedor(dataGridView1);
                itemseleccionado.Nombre = Interaction.InputBox("Razón Social: ");
                g.Update_Proveedor(itemseleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dataGridView1.Mostrar(g.ListaProveedoresCustom(g.ListaProveedores));
            }
        }
     
        //Delete proveedor
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var itemseleccionado = g.VerificaryRetornarProveedor(dataGridView1);
                g.Delete_Proveedor(itemseleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dataGridView1.Mostrar(g.ListaProveedoresCustom(g.ListaProveedores));
                ActualizarGrids();
            }
        }

        #endregion

        #region Pagos
        //Creacion pago
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                var codigo = InputsExtensions.InputBoxNumeric("Código de Pago");
                var importePagar = InputsExtensions.InputBoxNumeric("Importe a pagar");
                var fechaVencimiento = InputsExtensions.InputBoxDateNumeric("Fecha de Vencimiento (Formato: DD/MM/AAAA)"); 

                var tipoPagoMap = new Dictionary<string, Func<Pago>>
                {
                    { "efectivo", () => new Efectivo(Convert.ToInt32(codigo), fechaVencimiento, importePagar, Constantes.Pendiente) },
                    { "cheque", () => new Cheque(Convert.ToInt32(codigo), fechaVencimiento, importePagar, Constantes.Pendiente) }
                };
                var tipoStr = Interaction.InputBox("Tipo de Pago (Efectivo/Cheque)");
                if (tipoPagoMap.ContainsKey(tipoStr))
                {
                    var nuevoPago = tipoPagoMap[tipoStr]();
                    nuevoPago.Monto_Elevado += AlertadeMontoElevado;
                    g.Create_Pago(nuevoPago);
                }
                else
                    throw new Exception("Tipo de pago no válido. Use Efectivo o Cheque.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dataGridView2.Mostrar(g.ListaPagosCustom(g.ListaPagos));
            }
        }

        //Modificacion
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var itemseleccionado = g.VerificaryRetornarPago(dataGridView2);
                if (itemseleccionado.Estado == Constantes.Pagado)
                    throw new Exception("No se puede modificar un pago ya abonado");
                itemseleccionado.Monto_Elevado += AlertadeMontoElevado;
                itemseleccionado.Codigo = InputsExtensions.InputBoxNumeric("Código de Pago");
                itemseleccionado.Importe = InputsExtensions.InputBoxNumeric("Importe a pagar");
                itemseleccionado.Vencimiento = InputsExtensions.InputBoxDateNumeric("Fecha de Vencimiento (Formato: DD/MM/AAAA)");
                g.Update_Pago(itemseleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dataGridView2.Mostrar(g.ListaPagosCustom(g.ListaPagos));
            }
        }

        //Asignar
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                var proveedor_seleccionado = g.VerificaryRetornarProveedor(dataGridView1);
                var pago_eleccionado = g.VerificaryRetornarPago(dataGridView2);

                pago_eleccionado.Proveedor = proveedor_seleccionado;
                g.Asignate_Pago(pago_eleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ActualizarGrids();
            }
        }

        //Pagar
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                var proveedor_seleccionado = g.VerificaryRetornarProveedor(dataGridView1);
                var pago_eleccionado = g.VerificaryRetornarPago(dataGridView4);

                g.Pagar_Pago(pago_eleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ActualizarGrids();
            }
        }

        //Borrar
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var itemseleccionado = g.VerificaryRetornarPago(dataGridView2);
                g.Delete_Pago(itemseleccionado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                dataGridView2.Mostrar(g.ListaPagosCustom(g.ListaPagos));
                ActualizarGrids();
            }
        }

        private void AlertadeMontoElevado(object sender, EventArgs e)
        {
            MessageBox.Show("El importe a pagar excede los 10000. ¿Desea continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            DialogResult response = MessageBox.Show("¿Quieres continuar?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (response == DialogResult.No)
                throw new Exception("Se cancelará la operación.");
        }
        
        #endregion

        #region Grillas

        private void ActualizarGrids()
        {
            try
            {
                //Actualizo en base al seleccionado
                label2.Text = $"";
                label3.Text = $"";
                label4.Text = $"";
                dataGridView3.DataSource = null;
                dataGridView4.DataSource = null;
                dataGridView5.DataSource = null;

                if (dataGridView1.SelectedRows.Count <= 0)
                    return;
                var p = dataGridView1.SelectedRows[0].DataBoundItem as ProveedorDTO;
                if (p == null) 
                    return;

                label2.Text = $"Pagos realizados Leg: {p.Legajo}";
                label3.Text = $"Pagos pendientes Leg: {p.Legajo}";
                label4.Text = $"Pagos realizados Leg: {p.Legajo}";
                var proveedorModif = g.Read_Proveedor(p.Legajo);
                dataGridView4.Mostrar(g.ListaPagosCustom(proveedorModif.Pagos.Where(x => x.Estado == Constantes.Pendiente).ToList()));
                dataGridView3.Mostrar(g.ListaPagosCustom(proveedorModif.Pagos.Where(x => x.Estado == Constantes.Pagado).ToList()));
                dataGridView5.Mostrar(g.ListaPagosCustom(proveedorModif.Pagos.Where(x => x.Estado == Constantes.Pagado).ToList()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ActualizarGrids();
        }

        #endregion
    }
}
