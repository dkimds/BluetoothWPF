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
using DarrenLee.Bluetooth;

namespace WpfApp5
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bluetooth_Client client;
        private Random randomNumber;
        private System.Windows.Threading.DispatcherTimer timer;
        private System.Int32 voltage;
        //private System.Int32 currency;
        //private string vol_curr;
        public MainWindow()
        {
            InitializeComponent();
            this.client = new Bluetooth_Client(serverName: $"HP_04");
            this.Button_Device_Connection_Data.IsEnabled = false;
            this.timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += (sender, e) => { UI_Update(); };
            timer.Interval = new TimeSpan(hours: 0, minutes: 0, seconds: 1);

            this.voltage = 0;
            //this.currency = 0;
            this.randomNumber = new Random();
        }

        private void UI_Update()
        {
            this.voltage = randomNumber.Next(maxValue: 6);
            this.textBlock_voltage.Text = this.voltage.ToString();
            this.client.SyncMessage(message: $"{this.voltage.ToString()}");

            //this.currency = randomNumber.Next(maxValue: 20);
            //this.textBlock_currency.Text = this.currency.ToString();
            ////this.client.SyncMessage(message: $"{this.currency.ToString()}");

            //vol_curr = voltage.ToString() + "," + currency.ToString();
            //this.client.SyncMessage(message: $"{vol_curr}");
        }

        private void Click_on_Button(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(messageBoxText: $"블루투스 서버에 연결중입니다.");
            if (client.Start())
            {
                this.client.IsConnected += Client_IsConnected;
            }
            else
            {
                MessageBox.Show(messageBoxText: $"통신에 문제가 발생했습니다.");
                this.client.Disconnect();
            }
        }

        private void Client_IsConnected(object sender, EventArgs e)
        {
            MessageBox.Show(messageBoxText: $"블루투스 서버에 성공적으로 연결되었습니다.");
            this.Dispatcher.Invoke(callback: () => {
                this.Button_Device_Connection_Data.IsEnabled = true;
                this.Button_Device_Connection.IsEnabled = false;
            });

        }

        private void Click_on_Button_Data(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(callback: () => {
                timer.Start();
                this.Button_Device_Connection_Data.IsEnabled = false;
            });

        }

        private void Click_off_Button_Data(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.InvokeShutdown();
            this.timer.Stop();
        }
    }
}
