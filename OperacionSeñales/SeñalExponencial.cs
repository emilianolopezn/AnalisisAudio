using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacionSeñales
{
    class SeñalExponencial : Señal
    {

        //Propiedades de la señal
        private double alpha;
        public double Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = value;
            }
        }
        override public double evaluar(double t)
        {
            double resultado;

            resultado = Math.Pow(Math.E, t * alpha);

            return resultado;
        }
    }
}
