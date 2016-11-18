using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Numerics;

namespace OperacionSeñales
{
    abstract class Señal
    {
        public List<Point> señalDigitalizada;

        protected double tiempoInicial;
        public double TiempoInicial
        {
            get
            {
                return tiempoInicial;
            }
            set
            {
                tiempoInicial = value;
            }
        }

        protected double tiempoFinal;
        public double TiempoFinal
        {
            get
            {
                return tiempoFinal;
            }
            set
            {
                tiempoFinal = value;
            }
        }

        protected double intervaloMuestreo;
        public double IntervaloMuestreo
        {
            get
            {
                return intervaloMuestreo;
            }
            set
            {
                intervaloMuestreo = value;
            }
        }

        public void desplazarEnTiempo(double td)
        {
            señalDigitalizada = new List<Point>();
            for (double i = tiempoInicial;
                i <= tiempoFinal;
                i += intervaloMuestreo)
            {
                señalDigitalizada.Add(
                    new Point(i, evaluar(i + td)));

            }
        }


        public void escalarAmplitud(double factorEscala)
        {
            if (señalDigitalizada != null)
            {
                List<Point> nuevaSeñal = new List<Point>();
                foreach (Point punto in señalDigitalizada)
                {
                    nuevaSeñal.Add(new Point(
                        punto.X, punto.Y * factorEscala));
                }
                señalDigitalizada = nuevaSeñal;
            }
        }

        public void desplazarY(double desplazamiento)
        {
            if (señalDigitalizada != null)
            {
                List<Point> nuevaSeñal = new List<Point>();
                foreach (Point punto in señalDigitalizada)
                {
                    nuevaSeñal.Add(new Point(
                        punto.X, punto.Y + desplazamiento));
                }
                señalDigitalizada = nuevaSeñal;
            }
        }

        public void construirSeñalDigital()
        {
            desplazarEnTiempo(0);
        }

        public List<Point> sumar(List<Point> señalSumando)
        {
            if (señalDigitalizada.Count == señalSumando.Count)
            {
                int indice = 0;
                List<Point> señalResultado = new List<Point>();
                foreach (Point muestra in señalDigitalizada)
                {
                    Point muestraSuma = new Point();
                    muestraSuma.X = muestra.X;
                    muestraSuma.Y = muestra.Y + señalSumando[indice].Y;
                    señalResultado.Add(muestraSuma);


                    indice++;
                }
                return señalResultado;
            }

            return null;
        }

        public List<Point> restar(List<Point> señalRestando)
        {
            if (señalDigitalizada.Count == señalRestando.Count)
            {
                int indice = 0;
                List<Point> señalResultado = new List<Point>();
                foreach (Point muestra in señalDigitalizada)
                {
                    Point muestraResta = new Point();
                    muestraResta.X = muestra.X;
                    muestraResta.Y = muestra.Y - señalRestando[indice].Y;
                    señalResultado.Add(muestraResta);


                    indice++;
                }
                return señalResultado;
            }

            return null;
        }

        public List<Point> convolucionar(List<Point> señalConvolucion)
        {
            List<Point> señalResultante = new List<Point>();
            if (señalDigitalizada != null && señalConvolucion != null)
            {
                //Suma de los tiempos iniciales originales
                double tiempoInicialResultante =
                    señalDigitalizada[0].X +
                    señalConvolucion[0].X;
                double tiempoFinalResultante =
                    señalDigitalizada[señalDigitalizada.Count - 1].X +
                    señalConvolucion[señalConvolucion.Count - 1].X;
                double muestrasPorSegundo = 1 / intervaloMuestreo;
                double duracion = Math.Abs(tiempoFinalResultante - tiempoInicialResultante);
                double muestrasTotales = muestrasPorSegundo * duracion;

                double tiempoActual = tiempoInicial;

                for (int n = 0; n < muestrasTotales; n++)
                {
                    double muestraActual = 0;
                    for(int k=0; k<señalConvolucion.Count; k++)
                    {
                        if ( (n-k) >= 0 && (n-k) < señalConvolucion.Count)
                        {
                            muestraActual += (señalDigitalizada[k].Y * señalConvolucion[n - k].Y) * intervaloMuestreo;
                        }
                    }
                    señalResultante.Add(new Point(tiempoActual, muestraActual));
                    tiempoActual += intervaloMuestreo;
                }
            }
            return señalResultante;
        }

        public List<Point> correlacionar(List<Point> señalCorrelacion)
        {
            List<Point> señalResultante = new List<Point>();
            if (señalDigitalizada != null && señalCorrelacion != null)
            {
                //Suma de los tiempos iniciales originales

                double tiempoInicialResultante =
                    señalDigitalizada[0].X - Math.Abs(señalCorrelacion[señalCorrelacion.Count-1].X -
                    señalCorrelacion[0].X);

                double tiempoFinalResultante =
                    señalDigitalizada[señalDigitalizada.Count - 1].X;


                double muestrasPorSegundo = 1 / intervaloMuestreo;
                double duracion = Math.Abs(tiempoFinalResultante - tiempoInicialResultante);
                double muestrasTotales = muestrasPorSegundo * duracion;

                double tiempoActual = tiempoInicialResultante;

                for (int n = 0; n < muestrasTotales; n++) //n = tiempo actual (t)
                {
                    double muestraActual = 0;
                    for (int k = 0; k < señalCorrelacion.Count; k++) //k = instante de la señal a correlacionar (tao)
                    {

                        int indiceFactor = (señalCorrelacion.Count - 1 - n) + k - (int)(muestrasPorSegundo * tiempoInicial);
                        //Validación que los indices sean válidos
                        if (indiceFactor >= 0 && indiceFactor < señalCorrelacion.Count)
                        {
                            muestraActual += 
                                (señalDigitalizada[k].Y * señalCorrelacion[indiceFactor].Y) * 
                                intervaloMuestreo;
                        }
                    }
                    señalResultante.Add(new Point(tiempoActual, muestraActual));
                    tiempoActual += intervaloMuestreo;
                }
            }
            return señalResultante;
        }

        public List<Point> multiplicar(List<Point> señalFactor)
        {
            if (señalDigitalizada.Count == señalFactor.Count)
            {
                int indice = 0;
                List<Point> señalResultado = new List<Point>();
                foreach (Point muestra in señalDigitalizada)
                {
                    Point muestraProducto = new Point();
                    muestraProducto.X = muestra.X;
                    muestraProducto.Y = muestra.Y * señalFactor[indice].Y;
                    señalResultado.Add(muestraProducto);


                    indice++;
                }
                return señalResultado;
            }

            return null;
        }

        public List<Point> transformadaFourier()
        {
            List<Point> nuevaSeñal = new List<Point>();
            double frecuenciaMuestreo = 1 / intervaloMuestreo;
            double N = señalDigitalizada.Count;

            for(int k= 0; k < N/2; k++)
            {
                Complex muestraCompleja = new Complex(0.0, 0.0);
                for (int n=0; n < N; n++)
                {
                    double velocidadAngular = (2 * Math.PI) / N;
                    Complex exponente =  
                        Complex.ImaginaryOne * velocidadAngular 
                        * n * k * (-1);
                    muestraCompleja += (señalDigitalizada[n].Y * Complex.Exp(exponente)) 
                        * intervaloMuestreo; 
                }
                double xActual = k * frecuenciaMuestreo / N;
                nuevaSeñal.Add(new Point(xActual,muestraCompleja.Magnitude));
            }

            return nuevaSeñal;
        }
        


        abstract public double evaluar(double t);
    }
}

