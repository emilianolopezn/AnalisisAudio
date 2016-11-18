using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacionSeñales
{
    class SeñalSenoidal : Señal
    {

        private double frecuencia;
        public double Frecuencia
        {
            get
            {
                return frecuencia;
            }
            set
            {
                frecuencia = value;
            }
        }

        private double fase;
        public double Fase
        {
            get
            {
                return fase;
            }
            set
            {
                fase = value;
            }
        }

        private double amplitud;
        public double Amplitud
        {
            get
            {
                return amplitud;
            }
            set
            {
                amplitud = value;
            }
        }

        override public double evaluar(double t)
        {
            double resultado =
                amplitud *
                Math.Sin(2 * Math.PI * frecuencia * (t + fase));
            return resultado;
        }
    }
}
