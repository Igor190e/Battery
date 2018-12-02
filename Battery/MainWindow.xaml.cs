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
using System.IO;
using System.IO.Ports;

namespace Battery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string arduinoCom = "";
        byte InByte;
        Int16 InInt;
        string InString;


        public MainWindow()
        {

            InitializeComponent();

        }

        private void SearchArduinoCom_Click(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            SerialPort Com;
            
            string tmp = "";
            foreach (string a in ports)
            {
                Com = new SerialPort(a, 9600);
                Com.Open();
                Com.ReadTimeout=500;
                System.Threading.Thread.Sleep(100);
                try
                {
                    tmp = Com.ReadLine();
                   
                    if (Com.ReadLine().Trim() == "ARDUINO")
                    {
                        arduinoCom = a;
                        Com.WriteLine("INIT");
                        ArduinoCom.Content = arduinoCom;                       
                        Com.Close();
                        break;
                    }
                }
                catch { Com.Close(); }
             
            }  
        }

        private void ReadDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (ByteRadioButton.IsEnabled == true)
            { }
            else if (Int16RadioButton.IsEnabled == true)
            { }
            else if (StringRadioButton.IsEnabled == true)
            { }
        }
    }
}
