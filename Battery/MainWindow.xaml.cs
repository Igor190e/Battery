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
        string arduinoCom; // "COM3";
        bool find = false;
        int InInt;
        string InString;
        SerialPort Com; //=new SerialPort("COM3", 9600);        

        public MainWindow()
        {
            InitializeComponent();
        }


        private void SearchArduinoCom_Click(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            
            
            string tmp = "";
            foreach (string a in ports)
            {
                Com = new SerialPort(a, 9600);
                Com.ReadTimeout = 3000;
                Com.Open();

                System.Threading.Thread.Sleep(1000);
                try
                {
                    tmp = Com.ReadLine();
                    System.Threading.Thread.Sleep(100);
                    if (tmp.Trim() == "ARDUINO")
                    {
                        arduinoCom = a;
                        find = true;
                        Com.Write("INIT");                        
                        ArduinoCom.Content = arduinoCom;
                        
                    }
                }
                catch
                {
                    if (find == false)
                    { Com.Close(); }
                }
                if (find == true)
                {
                    break;
                }
             
            }
            Com.DiscardInBuffer();
        }

        private void ReadDataButton_Click(object sender, RoutedEventArgs e)
        {

            if (ByteRadioButton.IsChecked==true)
            {

                System.Threading.Thread.Sleep(500);
                Com.Write("ReadByte");

                while (Com.BytesToRead <= 0)
                    System.Threading.Thread.Sleep(1000);

                InInt = int.Parse(Com.ReadLine());

                HexData.Text = Convert.ToString(InInt, 16);
                DecData.Text = Convert.ToString(InInt);
                BinData.Text = Convert.ToString(InInt, 2);
                MainText.Text += $"Receive byte\n";
                MainText.Text += $"HEX =  {HexData.Text}\n";
                MainText.Text += $"DEC =  {DecData.Text}\n";
                MainText.Text += $"BIN =  {BinData.Text}\n";

            }
            else if (Int16RadioButton.IsChecked == true)
            {

                System.Threading.Thread.Sleep(1000);
                Com.Write("ReadInt16");
                while (Com.BytesToRead <= 0)
                    System.Threading.Thread.Sleep(1000);

                InInt = int.Parse(Com.ReadLine());
                HexData.Text = Convert.ToString(InInt, 16);
                DecData.Text = Convert.ToString(InInt);
                BinData.Text = Convert.ToString(InInt, 2);
                MainText.Text += $"Receive word\n";
                MainText.Text += $"HEX =  {HexData.Text}\n";
                MainText.Text += $"DEC =  {DecData.Text}\n";
                MainText.Text += $"BIN =  {BinData.Text}\n";

            }
            else if (StringRadioButton.IsChecked == true)
            {
                InString = "";
                int buf;
   
                Com.Write("ReadString");

                while (Com.BytesToRead<=0)
                System.Threading.Thread.Sleep(1000);

                byte[] b = new byte[int.Parse(StringLength.Text)];
                Com.Read(b, 0, int.Parse(StringLength.Text));
                string s = Encoding.ASCII.GetString(b);

                InString = s;


                MainText.Text += $"Receive string\n {InString}\n";
                StringData.Text = InString;
             
            }
        }

        private void SendByteButton_Click(object sender, RoutedEventArgs e)
        {
           
            Com.Write("WriteByte");
            System.Threading.Thread.Sleep(400);
            int buf = int.Parse(SendTextByte.Text, System.Globalization.NumberStyles.HexNumber);          
            Com.Write(Convert.ToString(buf));
            MainText.Text += $"Send    0x{SendTextByte.Text}\n";//InString;
            System.Threading.Thread.Sleep(500);

          
        }

        private void SendInt16Button_Click(object sender, RoutedEventArgs e)
        {           
            Com.WriteLine("WriteInt16");           
            Com.WriteLine($"0x{SendTextInt16}");
            System.Threading.Thread.Sleep(400);
            MainText.Text += $"Send    0x{SendTextInt16.Text}\n";//InString;
            System.Threading.Thread.Sleep(500);
        }
    }
}
