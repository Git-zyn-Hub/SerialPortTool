using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTool.Classes
{
   public class DataString:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        private string _保温瓶内温度;
        private string _直流马达;
        private string _张力;
        private string _温度;
        private string _CCL;
        private string _SP;
        private string _缆头电压;
        private string _马达电压;
        private string _泥浆电阻率;
        private string _继电器状态;

        public string 保温瓶内温度
        {
            get
            {
                return _保温瓶内温度;
            }

            set
            {
                _保温瓶内温度 = value;
            }
        }

        public string 直流马达
        {
            get
            {
                return _直流马达;
            }

            set
            {
                _直流马达 = value;
            }
        }

        public string 张力
        {
            get
            {
                return _张力;
            }

            set
            {
                _张力 = value;
            }
        }

        public string 温度
        {
            get
            {
                return _温度;
            }

            set
            {
                _温度 = value;
            }
        }

        public string CCL
        {
            get
            {
                return _CCL;
            }

            set
            {
                _CCL = value;
            }
        }

        public string SP
        {
            get
            {
                return _SP;
            }

            set
            {
                _SP = value;
            }
        }

        public string 缆头电压
        {
            get
            {
                return _缆头电压;
            }

            set
            {
                _缆头电压 = value;
            }
        }

        public string 马达电压
        {
            get
            {
                return _马达电压;
            }

            set
            {
                _马达电压 = value;
            }
        }

        public string 泥浆电阻率
        {
            get
            {
                return _泥浆电阻率;
            }

            set
            {
                _泥浆电阻率 = value;
            }
        }

        public string 继电器状态
        {
            get
            {
                return _继电器状态;
            }

            set
            {
                _继电器状态 = value;
            }
        }
    }
}
