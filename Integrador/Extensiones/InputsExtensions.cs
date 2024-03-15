using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integrador.Extensiones
{
    public static class InputsExtensions
    {
        public static int InputBoxNumeric(this string title)
        {
            var valor = Interaction.InputBox(title);
            if (!int.TryParse(valor, out int valorsal))
                throw new Exception("El valor ingresado debe ser numérico.");

            return valorsal;
        }

        public static DateTime InputBoxDateNumeric(this string title)
        {
            var fechaStr = Interaction.InputBox(title);
            DateTime fechaVencimiento;
            if (!DateTime.TryParseExact(fechaStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaVencimiento))
                throw new Exception("Formato de fecha incorrecto. Use DD/MM/AAAA.");

            return fechaVencimiento;
        }
    }

}
