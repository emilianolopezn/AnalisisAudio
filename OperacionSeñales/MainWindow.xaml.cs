using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;

namespace OperacionSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Señal primeraSeñal, terceraSeñal;
        public MainWindow()
        {
            InitializeComponent();
            panelSeñal1.Children.Add(new ConfiguracionAudio());



        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (scrConfigSeñal1 != null)
            {
                panelSeñal1.Children.Clear();
                switch (cbSeñal1.SelectedIndex)
                {
                    case 0:
                        panelSeñal1.Children.Add(new ConfiguracionAudio());
                        break;
                    default:
                        break;
                }
            }
        }

        private double obtenerFrecuenciaFundamental(List<Point> señal)
        {
            double valorMaximo = 0;
            double frecuenciaFundamental = 0;
            foreach (Point punto in señal)
            {
                if (Math.Abs(punto.Y) > valorMaximo)
                {
                    valorMaximo = Math.Abs(punto.Y);
                    frecuenciaFundamental = punto.X;
                }
            }

            return frecuenciaFundamental;
        }
        private double obtenerAmplitudMaxima(List<Point> señal)
        {
            double valorMaximo = 0;

            foreach (Point punto in señal)
            {
                if (Math.Abs(punto.Y) > valorMaximo)
                {
                    valorMaximo = Math.Abs(punto.Y);
                }
            }

            return valorMaximo;
        }



        private void graficar(Señal señal, Polyline grafica, double amplitudMayor)
        {
            double escalaY = scrGraficas.Height / 2.25;
            double escalaX = scrGraficas.Width / (señal.TiempoFinal - señal.TiempoInicial);

            PointCollection puntosGrafica = new PointCollection();
            foreach (Point punto in señal.señalDigitalizada)
            {
                puntosGrafica.Add(new Point(
                    (punto.X - señal.TiempoInicial) * escalaX,
                    ((punto.Y * -escalaY) / amplitudMayor) + escalaY
                    ));
            }

            grafica.Points = puntosGrafica;

        }

        private void graficarEjes(double ti, double tf, double amplitudMayor, Polyline ejeX, Polyline ejeY, Grid etiquetas, int saltoX)
        {
            double escalaY = scrGraficas.Height / 2.25;
            double escalaX = scrGraficas.Width / (tf - ti);

            PointCollection puntosOrigenX = new PointCollection();
            puntosOrigenX.Add(new Point(0, escalaY + 0));
            puntosOrigenX.Add(new Point((tf - ti) * escalaX, escalaY + 0));
            ejeX.Points = puntosOrigenX;

            PointCollection puntosOrigenY = new PointCollection();
            puntosOrigenY.Add(new Point((0 - ti) * escalaX, escalaY + (amplitudMayor * -escalaY) / amplitudMayor));
            puntosOrigenY.Add(new Point((0 - ti) * escalaX, escalaY + (-amplitudMayor * -escalaY) / amplitudMayor));
            ejeY.Points = puntosOrigenY;

            etiquetas.Children.Clear();
            for (int i = Convert.ToInt32(ti); i <= tf; i += saltoX)
            {
                var tb = new TextBlock();
                //left top right bottom
                tb.Margin = new Thickness((i - ti) * escalaX, escalaY, 0, 0);
                tb.Text = i.ToString();
                tb.Opacity = 0.5;
                etiquetas.Children.Add(tb);
            }

            for (int i = Convert.ToInt32(-amplitudMayor); i <= amplitudMayor; i++)
            {
                var tb = new TextBlock();
                //left top right bottom
                tb.Margin = new Thickness((0 - ti) * escalaX, (escalaY + (i * -escalaY) / amplitudMayor) - 9, 0, 0);
                tb.Text = i.ToString();
                tb.Opacity = 0.5;
                if (i != 0)
                {
                    etiquetas.Children.Add(tb);
                }


            }
        }
        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {


            primeraSeñal = new SeñalPersonalizada();
            primeraSeñal.TiempoInicial = 0;
            primeraSeñal.señalDigitalizada = new List<Point>();
            var file = ((ConfiguracionAudio)panelSeñal1.Children[0]).txtRuta.Text;
            using (var reader = new AudioFileReader(file))
            {
                var samplesPerSecond = reader.WaveFormat.SampleRate;
                double intervaloX = 1.0 / (double)reader.WaveFormat.SampleRate;
                primeraSeñal.IntervaloMuestreo = intervaloX;
                double xActual = 0;
                var readBuffer = new float[reader.WaveFormat.Channels];
                int samplesRead = 1;
                primeraSeñal.TiempoFinal = reader.TotalTime.TotalSeconds;
                txtIntervaloMuestreo.Text = reader.WaveFormat.SampleRate.ToString();
                txtTiempoInicial.Text = primeraSeñal.TiempoInicial.ToString();
                txtTiempoFInal.Text = primeraSeñal.TiempoFinal.ToString();
                do
                {
                    samplesRead = reader.Read(readBuffer, 0, reader.WaveFormat.Channels);
                    if (samplesRead > 0)
                    {
                        double max = readBuffer.Take(samplesRead).Max();
                        primeraSeñal.señalDigitalizada.Add(new Point(xActual, max));
                        xActual += intervaloX;
                    }
                } while (samplesRead > 0);
            }

            double amplitudMaxima = obtenerAmplitudMaxima(primeraSeñal.señalDigitalizada);
            graficar(primeraSeñal, plnSeñal1, amplitudMaxima);
            graficarEjes(primeraSeñal.TiempoInicial, primeraSeñal.TiempoFinal, amplitudMaxima, plnOrigenX1, plnOrigenY1, grdEtiquetasEjes1, 1);


            terceraSeñal = new SeñalPersonalizada();
            terceraSeñal.IntervaloMuestreo = primeraSeñal.IntervaloMuestreo;



            terceraSeñal.señalDigitalizada =
           primeraSeñal.transformadaFourier();
            terceraSeñal.TiempoInicial =
                terceraSeñal.señalDigitalizada[0].X;
            terceraSeñal.TiempoFinal =
                terceraSeñal.señalDigitalizada[terceraSeñal.señalDigitalizada.Count - 1].X;
            double freqFundamental = obtenerFrecuenciaFundamental(terceraSeñal.señalDigitalizada);
            lblFreq.Content = freqFundamental.ToString();




            if (obtenerAmplitudMaxima(terceraSeñal.señalDigitalizada)
                > amplitudMaxima)
            {
                amplitudMaxima =
                    obtenerAmplitudMaxima(terceraSeñal.señalDigitalizada);
            }

            graficar(terceraSeñal, plnSeñal3, amplitudMaxima);

            
            graficarEjes(terceraSeñal.TiempoInicial, terceraSeñal.TiempoFinal, amplitudMaxima, plnOrigenX2, plnOrigenY2, grdEtiquetasEjes2,1000);

        }
    }
}
