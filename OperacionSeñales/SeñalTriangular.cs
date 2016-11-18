using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacionSeñales
{
    class SeñalTriangular : Señal
    {
        private double periodo;
        public double Periodo
        {
            get
            {
                return periodo;
            }
            set
            {
                periodo = Math.Abs(value);
            }
        }

   
        override public double evaluar(double t)
        {
            double resultado;
            if (Math.Abs(t) < periodo/2)
            {
                resultado =  1 - (Math.Abs(t) / (periodo/2));
            }
            else
            {
                resultado = 0;
            }
            return resultado;
        }
        
    }
}
