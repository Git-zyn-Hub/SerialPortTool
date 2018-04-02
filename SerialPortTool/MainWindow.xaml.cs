using SerialPortTool.Classes;
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
using System.Windows.Threading;

namespace SerialPortTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort _serialPort1 = new SerialPort();
        private Window _container;
        private bool _hexSend = false;
        private SplitEnum _splitEnum;
        private string _msgLast;
        private int _sameMsgCount = 0;
        private bool _setTimer = false;
        private DispatcherTimer timer = new DispatcherTimer();
        private byte[] sendData;
        private SolidColorBrush _saveMenuColor;
        private byte[] _saveFirstRecv;
        private int _hitCount = 0;
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
                    MessageBox.Show("串口打开！");
                    messageCenter("串口打开", DataLevel.Normal);
                    SettingWinClose();
                    return true;
                }
                catch (Exception ee)
                {
                    MessageBox.Show("串口无法打开! " + ee.Message);
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

                //int checkSum;
                byte[] receivedData = new byte[bytesCount];
                for (int i = 0; i < bytesCount; i++)
                {
                    receivedData[i] = buffer[i];
                }

                //this.Dispatcher.Invoke(new Action(() =>
                //{
                //this.txtTestReceive.Text = strTest;
                //显示原始数据
                //this._originData.AddReceiveData(receivedData, receivedData.Length);
                //}));

                this.Dispatcher.Invoke(new Action(() =>
                {
                    if (handlePinJie(ref receivedData))
                    {

                    }
                    else
                    {
                        handleRecvData(receivedData);
                    }
                }));

                //计算校验和
                //checkSum = CheckSum.CalcCheckSum(receivedData, receivedData.Length - 2);
                //if (!checkCheckSum(checkSum, receivedData))
                //{
                //    MessageBox.Show("校验和出错！");
                //    return;
                //}
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private bool handlePinJie(ref byte[] receivedData)
        {
            if (receivedData.Length - 45 < 0)
            {
                messageCenter("接收数据长度小于45,拼接", DataLevel.Warning);
                _hitCount++;
                if (_hitCount == 2)
                {
                    List<byte> byteSource = new List<byte>();
                    if (_saveFirstRecv.Length == 1)
                    {
                        byteSource.Add(_saveFirstRecv[0]);
                    }
                    else
                    {
                        byteSource.AddRange(_saveFirstRecv);
                    }
                    if (receivedData.Length == 1)
                    {
                        byteSource.Add(receivedData[0]);
                    }
                    else
                    {
                        byteSource.AddRange(receivedData);
                    }
                    receivedData = byteSource.ToArray();
                    _hitCount = 0;
                    return false;
                }
                else
                {
                    _saveFirstRecv = receivedData;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void handleRecvData(byte[] receivedData)
        {
            if (receivedData.Length < 45)
            {
                messageCenter("接收长度小于45字节", DataLevel.Warning);
                return;
            }
            if (receivedData[0] != 0x47)
            {
                messageCenter("接收到的数据帧头不为0x47", DataLevel.Warning);
                return;
            }
            dataShow.DatasFloat.保温瓶内温度 = Bytes2SingleIEEE754.Bytes2Single(receivedData, 1);
            dataShow.DatasFloat.直流马达 = Bytes2SingleIEEE754.Bytes2Single(receivedData, 5);
            dataShow.DatasFloat.张力 = Bytes2SingleIEEE754.Bytes2Single(receivedData, 9);
            dataShow.DatasFloat.温度 = Bytes2SingleIEEE754.Bytes2Single(receivedData, 13);
            dataShow.DatasFloat.CCL = Bytes2SingleIEEE754.Bytes2Single(receivedData, 17);
            dataShow.DatasFloat.SP = Bytes2SingleIEEE754.Bytes2Single(receivedData, 21);
            dataShow.DatasFloat.缆头电压 = Bytes2SingleIEEE754.Bytes2Single(receivedData, 25);
            dataShow.DatasFloat.马达电压 = Bytes2SingleIEEE754.Bytes2Single(receivedData, 29);
            dataShow.DatasFloat.泥浆电阻率 = Bytes2SingleIEEE754.Bytes2Single(receivedData, 33);

            dataShow.DatasReal.保温瓶内温度 = getRealValueString(receivedData, 1);
            dataShow.DatasReal.直流马达 = getRealValueString(receivedData, 5);
            dataShow.DatasReal.张力 = getRealValueString(receivedData, 9);
            dataShow.DatasReal.温度 = getRealValueString(receivedData, 13);
            dataShow.DatasReal.CCL = getRealValueString(receivedData, 17);
            dataShow.DatasReal.SP = getRealValueString(receivedData, 21);
            dataShow.DatasReal.缆头电压 = getRealValueString(receivedData, 25);
            dataShow.DatasReal.马达电压 = getRealValueString(receivedData, 29);
            dataShow.DatasReal.泥浆电阻率 = getRealValueString(receivedData, 33);
            dataShow.DatasReal.继电器状态 = getRealValueString(receivedData, 37);
            dataShow.OnPropertyChanged("DatasFloat");
            dataShow.OnPropertyChanged("DatasReal");
        }

        private string getRealValueString(byte[] data, int startIndex)
        {
            if (data.Length < startIndex + 4)
            {
                throw new Exception("索引超出数组界限");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(data[startIndex].ToString("X2"));
                sb.Append(" ");
                sb.Append(data[startIndex + 1].ToString("X2"));
                sb.Append(" ");
                sb.Append(data[startIndex + 2].ToString("X2"));
                sb.Append(" ");
                sb.Append(data[startIndex + 3].ToString("X2"));
                return sb.ToString();
            }
        }

        public void EnablePortConnect()
        {
            this.miConnect.IsEnabled = false;
            this.miConnect.Background = new SolidColorBrush(Colors.LightGreen);
        }
        public void DisablePortConnect()
        {
            this.miConnect.IsEnabled = true;
            this.miConnect.Background = _saveMenuColor;
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

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_serialPort1.IsOpen)
                {
                    messageCenter("串口未打开！", DataLevel.Error);
                }
                else
                {
                    if (_hexSend)
                    {
                        sendData = getSendBytesHex();
                    }
                    else
                    {
                        string strInput = this.txtSend.Text.Trim();
                        sendData = Encoding.UTF8.GetBytes(strInput);
                    }
                    if (_setTimer)
                    {
                        int interval;
                        if (!string.IsNullOrEmpty(txtTimerInterval.Text.Trim()))
                        {
                            if (int.TryParse(txtTimerInterval.Text.Trim(), out interval))
                            {
                                timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
                                timer.Start();
                            }
                            else
                            {
                                messageCenter("定时发送，时间间隔转换失败", DataLevel.Error);
                            }
                        }
                        else
                        {
                            messageCenter("定时发送，时间间隔不能为空", DataLevel.Error);
                        }
                    }
                    else
                    {
                        sendWithMsg(sendData);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("发送异常:" + ee.Message);
            }
        }

        private void sendWithMsg(byte[] sendData)
        {
            if (Send(sendData))
            {
                messageCenter("发送成功", DataLevel.Normal);
            }
            else
            {
                messageCenter("发送失败，串口已经关闭", DataLevel.Error);
            }
        }

        public void messageCenter(string msg, DataLevel level)
        {
            string addCount = string.Empty;
            if (_msgLast == msg)
            {
                _sameMsgCount++;
                addCount = " +" + _sameMsgCount;
            }
            else
            {
                _sameMsgCount = 0;
            }
            AddDataInfo(msg + addCount, level);
            _msgLast = msg;
        }

        public void AddDataInfo(string info, DataLevel dataLevel)
        {
            try
            {
                switch (dataLevel)
                {
                    case DataLevel.Default:
                        lblMessage.Foreground = new SolidColorBrush(Colors.Black);
                        break;
                    case DataLevel.Normal:
                        lblMessage.Foreground = new SolidColorBrush(Colors.Green);
                        break;
                    case DataLevel.Warning:
                        lblMessage.Foreground = new SolidColorBrush(Colors.Orange);
                        break;
                    case DataLevel.Error:
                        lblMessage.Foreground = new SolidColorBrush(Colors.Red);
                        break;
                    default:
                        lblMessage.Foreground = new SolidColorBrush(Colors.Black);
                        break;
                }
                lblMessage.Content = info;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void cbHex_Checked(object sender, RoutedEventArgs e)
        {
            _hexSend = true;
            this.grpSplitor.IsEnabled = true;
        }

        private void cbTimer_Checked(object sender, RoutedEventArgs e)
        {
            _setTimer = true;
        }

        private void cbHex_Unchecked(object sender, RoutedEventArgs e)
        {
            _hexSend = false;
            this.grpSplitor.IsEnabled = false;
        }

        private void cbTimer_Unchecked(object sender, RoutedEventArgs e)
        {
            _setTimer = false;
            timer.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbHex.IsChecked = true;
            timer.Tick += (a, b) => { sendWithMsg(sendData); };
            _saveMenuColor = this.miConnect.Background as SolidColorBrush;
        }

        private void rbVoidSplit_Checked(object sender, RoutedEventArgs e)
        {
            _splitEnum = SplitEnum.Void;
        }

        private void rbBlankSplit_Checked(object sender, RoutedEventArgs e)
        {
            _splitEnum = SplitEnum.Blank;
        }

        private void rbDashSplit_Checked(object sender, RoutedEventArgs e)
        {
            _splitEnum = SplitEnum.Dash;
        }

        private byte[] getSendBytesHex()
        {
            string strInput = this.txtSend.Text.Trim();
            int length = strInput.Length;
            switch (_splitEnum)
            {
                case SplitEnum.Void:
                    {
                        if (length % 2 != 0)
                        {
                            MessageBox.Show("字符个数必须能被2整除", "出错了", MessageBoxButton.OK, MessageBoxImage.Error);
                            return null;
                        }
                        int count = length / 2;
                        string[] strInputs = new string[count];
                        byte[] byteInput = new byte[count];
                        for (int i = 0; i < length; i++, i++)
                        {
                            strInputs[i / 2] = strInput.Substring(i, 2);
                            byteInput[i / 2] = Convert.ToByte(strInputs[i / 2], 16);
                        }
                        return byteInput;
                    }
                case SplitEnum.Blank:
                case SplitEnum.Dash:
                    {
                        int _length = strInput.Length + 1;
                        int _count = length / 3;
                        string[] strInputs = new string[_count];

                        if (_splitEnum == SplitEnum.Blank)
                        {
                            strInputs = strInput.Split(' ');
                        }
                        else if (_splitEnum == SplitEnum.Dash)
                        {
                            strInputs = strInput.Split('-');
                        }
                        _count = strInputs.Length;
                        byte[] _byteInput = new byte[_count];
                        for (int i = 0; i < _count; i++)
                        {
                            _byteInput[i] = Convert.ToByte(strInputs[i], 16);
                        }
                        return _byteInput;
                    }
                default:
                    return null;
            }
        }

        private bool Send(byte[] sendData)
        {
            if (_serialPort1.IsOpen)
            {
                _serialPort1.Write(sendData, 0, sendData.Length);
                return true;
            }
            else
            {
                return false;
            }
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*e.Key != Key.Back && e.Key != Key.Decimal && e.Key != Key.OemPeriod && e.Key != Key.Return
            * Key.Back 退格键
            * Key.Return 回车
            * Key.Tab 键
            */
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Return || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //屏蔽中文输入和非法字符粘贴输入
            try
            {
                TextBox textBox = sender as TextBox;
                TextChange[] change = new TextChange[e.Changes.Count];
                e.Changes.CopyTo(change, 0);

                int offset = change[0].Offset;
                if (change[0].AddedLength > 0)
                {
                    //这里只做Double类型转换的检测，如果是Int或者其他类型需要改变num的类型，和TryParse前面类型。
                    int num = 0;
                    if (!int.TryParse(textBox.Text, out num))
                    {
                        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                        textBox.Select(offset, 0);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void miClosePort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_serialPort1.IsOpen)
                {
                    _serialPort1.Close();
                    this.cbTimer.IsChecked = false;
                    DisablePortConnect();
                    messageCenter("串口成功关闭", DataLevel.Error);
                    _serialPort1 = null;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtSend.Text = string.Empty;
            this.dataShow.DatasFloat = new DataFloat();
            this.dataShow.DatasReal = new DataString();
        }
    }

    public enum SplitEnum
    {
        Void, //无分隔符
        Blank,//空格分隔
        Dash  //中划线分隔
    }

    public enum DataLevel
    {
        Default,
        Normal,
        Warning,
        Error
    }
}
