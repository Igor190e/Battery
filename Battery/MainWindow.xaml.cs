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
        string arduinoCom = "COM3";
        byte InByte;
        int InInt;
        string InString;
        SerialPort Com;//=new SerialPort("COM3", 9600);
        

        public MainWindow()
        {
            Com= new SerialPort("COM3", 9600);
           
            System.Threading.Thread.Sleep(1000);

         

            Com.Open();



            Com.ReadTimeout = 3000;
            InitializeComponent();

        }

        private void SearchArduinoCom_Click(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            
            
            string tmp = "";
            foreach (string a in ports)
            {
                Com = new SerialPort(a, 9600);
                Com.Open();
                Com.ReadTimeout=3000;
                System.Threading.Thread.Sleep(1000);
                try
                {
                    tmp = Com.ReadLine();
                    System.Threading.Thread.Sleep(100);
                    if (tmp.Trim() == "ARDUINO")
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

            if (ByteRadioButton.IsChecked==true)
            {
                // Com = new SerialPort(arduinoCom, 9600);
                //Com.Open();
                //Com.ReadTimeout = 3000;
                System.Threading.Thread.Sleep(500);
                Com.Write("ReadByte");
                System.Threading.Thread.Sleep(500);
                try
                {
                    InString = Com.ReadLine();
                }
                catch { }
                //InInt = int.Parse(InString);
                HexData.Text = InString;
                //HexData.Text = Convert.ToString(InInt, 16);
                //DecData.Text = Convert.ToString(InInt);
                //BinData.Text = Convert.ToString(InInt, 2);
                //Com.Close();
            }
            else if (Int16RadioButton.IsChecked == true)
            {
                //Com = new SerialPort(arduinoCom, 9600);
                //Com.Open();
                //Com.ReadTimeout = 3000;
                System.Threading.Thread.Sleep(1000);
                Com.Write("ReadInt16");
                System.Threading.Thread.Sleep(10000);
                //try
                //{
                //    InString = Com.ReadLine();
                //}
                //catch { }
                //InInt = Int16.Parse(InString);
                //InString = Com.ReadLine();
                //InInt = Com.ReadByte();
                //InInt = InInt << 8;
                //System.Threading.Thread.Sleep(500);
                //InInt += Com.ReadByte();
                InInt = int.Parse(Com.ReadLine());
                HexData.Text = Convert.ToString(InInt, 16);
                DecData.Text = Convert.ToString(InInt);
                BinData.Text = Convert.ToString(InInt, 2);
                //Com.Close();
            }
            else if (StringRadioButton.IsChecked == true)
            {
                InString = "";
                int buf;
                byte[] bt=new byte[5];

                
                Com.Write("ReadString");

                
                while (Com.BytesToRead<=0)
                System.Threading.Thread.Sleep(1000);


                byte[] b = new byte[5];
                Com.Read(b, 0, 5);
                string s = Encoding.ASCII.GetString(b);

                InString = s;// asciiEncoding.GetString(bt);




                MainText.Text = InString;
                StringData.Text = InString;
                //Com.Close();
            }
        }

        private void SendByteButton_Click(object sender, RoutedEventArgs e)
        {
            //Com = new SerialPort(arduinoCom, 9600);
            //Com.Open();
            //Com.ReadTimeout = 3000;
            //System.Threading.Thread.Sleep(1000);
            Com.Write("WriteByte");
            System.Threading.Thread.Sleep(1000);
            int buf = Convert.ToInt32(SendTextByte.Text,16);
            Com.Write(Convert.ToString(buf));
            System.Threading.Thread.Sleep(1000);
            //Com.Close();
        }

        private void SendInt16Button_Click(object sender, RoutedEventArgs e)
        {
            //Com = new SerialPort(arduinoCom, 9600);
            //Com.Open();
            //Com.ReadTimeout = 3000;
            Com.WriteLine("WriteInt16");
            Com.WriteLine($"0x{SendTextInt16}");
            //Com.Close();
        }
    }
}
