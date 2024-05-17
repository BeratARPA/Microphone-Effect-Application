using NAudio.Gui;
using NAudio.Wave;
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

namespace WpfAppUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WaveIn waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider waveProvider;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAudio();
        }

        private void InitializeAudio()
        {
            // Mikrofon sesini almak için WaveIn sınıfını kullanıyoruz
            waveIn = new WaveIn();
            waveIn.DataAvailable += WaveIn_DataAvailable;

            // WaveIn ayarları
            waveIn.WaveFormat = new WaveFormat(44100, 1); // Örnek oranı: 44.1kHz, 1 kanal (mono)

            // WaveOut sınıfı ile çıkış yapacağız
            waveOut = new WaveOut();

            // Ses efektlerini işlemek için BufferedWaveProvider kullanıyoruz
            waveProvider = new BufferedWaveProvider(waveIn.WaveFormat);

            // WaveOut'a BufferedWaveProvider'ı bağlıyoruz
            waveOut.Init(waveProvider);

            // Mikrofonu başlat
            waveIn.StartRecording();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            WaveBuffer waveBuffer = new WaveBuffer(buffer);

            // Gelen ses verisinin ortalamasını alarak slider'ı güncelle
            float total = 0;
            for (int i = 0; i < bytesRecorded / 2; i++)
            {
                short sample = waveBuffer.ShortBuffer[i];
                total += Math.Abs(sample); // Ses seviyesini mutlak değer olarak al
            }
            float average = total / (bytesRecorded / 2);
            Dispatcher.Invoke(() => volumeSlider.Value = average); // Slider'ı GUI thread'inde güncelle
        }       
    }
}
