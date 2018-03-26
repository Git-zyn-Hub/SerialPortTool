using SerialPortTool.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SerialPortTool.UserControls
{
    /// <summary>
    /// Interaction logic for DataShowUserControl.xaml
    /// </summary>
    public partial class DataShowUserControl : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        private DataFloat _datasFloat = new DataFloat();
        private DataString _datasReal = new DataString();
        public DataShowUserControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public DataFloat DatasFloat
        {
            get
            {
                return _datasFloat;
            }

            set
            {
                if (_datasFloat != value)
                {
                    _datasFloat = value;
                    OnPropertyChanged("DatasFloat");
                }
            }
        }

        public DataString DatasReal
        {
            get
            {
                return _datasReal;
            }

            set
            {
                if (_datasReal != value)
                {
                    _datasReal = value;
                    OnPropertyChanged("DatasReal");
                }
            }
        }
    }
}
