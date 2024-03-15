using Integrador.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Integrador.Extensiones
{
    public static class DataGridViewExtensions
    {

        public static T VerificarYRetornarSeleccion<T>(this DataGridView grid) where T : class
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid), "El DataGridView no puede ser nulo.");
            if (grid.Rows.Count == 0)
                throw new Exception("El DataGridView está vacío.");
            if (grid.SelectedRows.Count <= 0)
                throw new Exception("Debe seleccionar un elemento para continuar.");
            if (grid.SelectedRows[0].DataBoundItem == null)
                throw new Exception("No se ha vinculado ningún elemento a la fila seleccionada.");

            return grid.SelectedRows[0].DataBoundItem as T;
        }

        public static void VerificarSeleccion(this DataGridView grid)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid), "La lista no puede ser nula.");
            if (grid.Rows.Count == 0)
                throw new Exception("La lista está vacía.");
            if (grid.SelectedRows.Count <= 0)
                throw new Exception("Debe seleccionar un item para continuar.");
            if (grid.SelectedRows[0].DataBoundItem == null)
                throw new Exception("No se ha vinculado ningún item a la fila seleccionada.");
        }

        public static void ConfigurarGrids(this DataGridView dataGridView)
        {
            dataGridView.MultiSelect = false;
            //dataGridView.AutoGenerateColumns = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.DefaultCellStyle.Font = new Font("Calibri", 8);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 8, FontStyle.Bold);
            dataGridView.RowsDefaultCellStyle.Font = new Font("Calibri", 8);
        }

        public static void Mostrar<T>(this DataGridView dataGridView, List<T> listaDeItems)
        {
            // Asigna la lista como fuente de datos
            dataGridView.DataSource = null;
            dataGridView.DataSource = listaDeItems;
            dataGridView.AutoResizeColumns();
        }

        public static void CargarGrid<T>(this DataGridView dataGridView, List<(string header, string field)> campos, List<T> listaDeItems)
        {
            // Agrega las columnas al DataGridView si no existen
            foreach (var (header, field) in campos)
            {
                if (!dataGridView.Columns.Contains(field))
                {
                    var columna = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = field,
                        HeaderText = header,
                        Name = field
                    };
                    dataGridView.Columns.Add(columna);
                }
            }

            // Asigna la lista como fuente de datos
            dataGridView.DataSource = null;
            dataGridView.DataSource = listaDeItems;
            dataGridView.AutoResizeColumns();
        }
    }
}
