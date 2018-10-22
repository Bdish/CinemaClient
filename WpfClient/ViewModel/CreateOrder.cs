using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.ViewModel
{
    public class CreateOrder : INotifyPropertyChanged
    {
        private int _countPlace;

        public int CountPlace
        {
            get { return _countPlace; }
            set
            {
                /* if (!SelectedSeanceIsInSeances())
                 {*/
                _countPlace = value;
                OnPropertyChanged("CountPlace");
                // }

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
