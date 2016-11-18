using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacionSeñales
{
    class SeñalRampa : Señal
    {

        override public double evaluar(double t)
        {
            double resultado = 0;

            if (t >= 0)
            {
                resultado = t;
            }


            return resultado;
        }
    }
}
