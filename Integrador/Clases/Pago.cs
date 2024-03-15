using Integrador.BaseClas;
using System;

namespace Integrador.Clases
{
    //Se usa abstracta para que no se pueda instanciar directamente
    public abstract class Pago : ClaseBase
    {
        public Pago(){ }
        
        public Pago(int codigo, DateTime vencimiento, decimal importe, int estado)
        {
            Codigo = codigo;
            Vencimiento = vencimiento;
            Importe = importe;
            Estado = estado;
        }
        
        public int Codigo { get; set; }
        
        public DateTime Vencimiento { get; set; }

        //Defino en el set una condicion para que dispare un evento
        public decimal _importe;
        public decimal Importe
        {
            get 
            { 
                return _importe; 
            }
            set 
            { 
                _importe = value;
                if (Importe > Constantes.ImporteMaximo)
                {
                    Monto_Elevado?.Invoke(null, null); ;
                }
            }
        }

        public int Estado  { get; set;}

        public Proveedor Proveedor { get; set;}

        //Evento que se dispara en la clase
        public event EventHandler Monto_Elevado;

        public decimal PorcentajeRecargo { get; set; }

        //Uso una funcion abstracta para que las hijas deban instanciarla si o si
        public abstract decimal Recargo();

    }

    public class Efectivo : Pago
    {
        public Efectivo(int codigo, DateTime vencimiento, decimal importe, int estado) : base(codigo, vencimiento, importe, estado)
        {
        }

        public override decimal Recargo()
        {
            return 0.01m; // El recargo para el pago en efectivo es del 1%
        }
    }

    public class Cheque : Pago
    {
        public Cheque(int codigo, DateTime vencimiento, decimal importe, int estado) : base(codigo, vencimiento, importe, estado)
        {
        }

        public override decimal Recargo()
        {
            return 0.1m; // El recargo para el pago con cheque es del 10%
        }
    }

}
