using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperacionSeñales
{
    class SeñalRectangular : Señal
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
            if (Math.Abs(t) == periodo/2)
            {
                return 0.5;
            } else if (Math.Abs(t) < periodo/2)
            {
                return 1;
            }
            return 0;
        }
    }
}
