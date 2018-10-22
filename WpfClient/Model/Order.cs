using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Model
{
    public class Order : INotifyPropertyChanged
    {
        private int _id;
        private int _idSeance;
        private int _countPlace;
        private string _ticketSales;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public int IdSeance
        {
            get
            {
                return _idSeance;
            }
            set
            {
                _idSeance = value;
                OnPropertyChanged("IdSeance");
            }
        }

        public int CountPlace
        {
            get
            {
                return _countPlace;
            }
            set
            {
                _countPlace = value;
                OnPropertyChanged("CountPlace");
            }
        }

        public string TicketSales
        {
            get
            {
                return _ticketSales;
            }
            set
            {
                _ticketSales = value;
                OnPropertyChanged("TicketSales");
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
