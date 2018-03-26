using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTool.Classes
{
    public class DataFloat : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        private float _保温瓶内温度;
        private float _直流马达;
        private float _张力;
        private float _温度;
        private float _CCL;
        private float _SP;
        private float _缆头电压;
        private float _马达电压;
        private float _泥浆电阻率;

        public float 保温瓶内温度
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

        public float 直流马达
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

        public float 张力
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

        public float 温度
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

        public float CCL
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

        public float SP
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

        public float 缆头电压
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

        public float 马达电压
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

        public float 泥浆电阻率
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
    }
}
