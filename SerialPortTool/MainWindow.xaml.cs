using SerialPortTool.UserControls;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SerialPortTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort _serialPort1 = new SerialPort();
        private Window _container;
        public SerialPort SerialPort
        {
            get { return _serialPort1; }
            set
            {
                if (_serialPort1 != value)
                {
                    _serialPort1 = value;
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool SerialPortInitialize()
        {
            if (_serialPort1.IsOpen)
            {
                MessageBox.Show("串口早就打开了有木有!");
                return false;
            }
            else
            {
                try
                {
                    _serialPort1.Open();
                    _serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    MessageBox.Show("端口打开！");
                    SettingWinClose();
                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("端口无法打开! " + ee.Message);
                    return false;
                }
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(800);
                byte[] buffer = new byte[4096];
                int bytesCount = _serialPort1.Read(buffer, 0, 4096);

                int checkSum;
                byte[] receivedData = new byte[bytesCount];
                for (int i = 0; i < bytesCount; i++)
                {
                    receivedData[i] = buffer[i];
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    //this.txtTestReceive.Text = strTest;
                    //显示原始数据
                    //this._originData.AddReceiveData(receivedData, receivedData.Length);
                }));

                if (receivedData.Length - 2 < 0)
                {
                    MessageBox.Show("接收数据长度小于2");
                    return;
                }
                //计算校验和
                //checkSum = CheckSum.CalcCheckSum(receivedData, receivedData.Length - 2);
                //if (!checkCheckSum(checkSum, receivedData))
                //{
                //    MessageBox.Show("校验和出错！");
                //    return;
                //}
                this.Dispatcher.Invoke(new Action(() =>
                {
                    handleRecvData(receivedData);
                }));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void handleRecvData(byte[] receivedData)
        {
            throw new NotImplementedException();
        }

        public void EnablePortConnect()
        {
            this.miConnect.IsEnabled = false;
            this.miConnect.Background = new SolidColorBrush(Colors.LightGreen);
        }

        public void SettingWinClose()
        {
            this._container.Close();
            this._container = null;
        }
        private void miConnect_Click(object sender, RoutedEventArgs e)
        {
            PortSettingUserControl newSettings = new PortSettingUserControl(this);
            if (_container == null)
            {
                _container = new Window();
                _container.Height = 300;
                _container.Width = 300;
                _container.Owner = this;
                _container.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                _container.Closed += container_Closed;

                _container.Content = newSettings;
                _container.Show();
            }
        }
        private void container_Closed(object sender, EventArgs e)
        {
            SettingWinClose();
            this.Activate();
        }
    }
}
