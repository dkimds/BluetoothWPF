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

namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bluetooth_Server server = new Bluetooth_Server();
        private System.Windows.Threading.DispatcherTimer timer;
        private System.Int32 voltage;
        //private System.Int32 currency;
        //private string vol_curr;

        public MainWindow()
        {
            InitializeComponent();
            this.Button_Device_Connection_Data.IsEnabled = false;
            this.timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += (sender, e) => { UI_Update(); };
            timer.Interval = new TimeSpan(hours: 0, minutes: 0, seconds: 1);
            this.Level_of_Voltage.Value = 0;
            this.voltage = 0;
            //this.Level_of_Currency.Value = 0;
            //this.currency = 0;
            //this.timer.Start();
            this.Button_Server_Disconnect.IsEnabled = false;
            this.Button_Device_Connection_Data.IsEnabled = false;
        }

        private void UI_Update()
        {
            this.Level_of_Voltage.Value = this.voltage;
            //this.Level_of_Currency.Value = this.currency;
        }

        private void Click_on_Button(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(messageBoxText: $"블루투스 연결을 기다리고 있습니다.");
            if (this.server.Start())
            {

                this.Button_Device_Connection_Data.IsEnabled = true;

                //this.server.IsConnected += Server_IsConnected;
                this.server.IsConnected += new EventHandler(this.Server_IsConnected);
            }
            else
            {
                MessageBox.Show(messageBoxText: $"통신에 문제가 발생했습니다.");
                this.server.Disconnect();

            }
        }

        private void Server_DataReceived(object sender, BluetoothServerEventArgs e)
        {
            //throw new NotImplementedException();
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(value: e.Message);
            this.voltage = Int32.Parse(s: stringBuilder.ToString());
           
        }

        private void Server_IsConnected(object sender, EventArgs e)
        {

            MessageBox.Show(messageBoxText: $"블루투스가 연결 되었습니다.");
            this.Button_Device_Connection.Dispatcher.Invoke(callback: () => {
                this.Button_Device_Connection.IsEnabled = false;
                this.Button_Server_Disconnect.IsEnabled = true;
            });

            //throw new NotImplementedException();
        }

        private void Click_on_Button_Data(object sender, RoutedEventArgs e)
        {
            this.server.DataReceived += Server_DataReceived;
            this.timer.Start();
            this.Button_Device_Connection_Data.Dispatcher.Invoke(callback: () =>
            {
                this.Button_Device_Connection_Data.IsEnabled = false;

            });


        }

        private void Click_on_Buttion_Disconnect(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(messageBoxText: $"서버 연결을 종료합니다.");
            this.server.Disconnect();
            //this.Dispatcher.InvokeShutdown();
            this.Button_Device_Connection_Data.Dispatcher.InvokeShutdown();
        }
    }
}
