using AccessToWebApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AccessToWebApi.Entities;
using System.Windows;
using System.Windows.Threading;
using WpfClient.Model;

namespace WpfClient.ViewModel
{
    public class SeancesViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private WpfClient.Model.Seance _selectedSeance;
        private HTTPForSeances _seanceHttpClient;
        private HTTPForOrder _orderHttpClient;

        public ObservableCollection<WpfClient.Model.Seance> Seances { get; set; }

        public WpfClient.Model.Seance SelectedSeance
        {
            get { return _selectedSeance; }
            set
            {              
                    _selectedSeance = value;
                    OnPropertyChanged("SelectedSeance");                               
            }
        }

        public SeancesViewModel(HTTPForSeances seanceHttpClient, HTTPForOrder orderHttpClient)
        {
            Seances = new ObservableCollection<WpfClient.Model.Seance>();
            _seanceHttpClient = seanceHttpClient;
            _orderHttpClient = orderHttpClient;

            _timer = new System.Windows.Threading.DispatcherTimer();

            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

        }

        private void TimerTick(object sender, EventArgs e)
        {
            List<AccessToWebApi.Entities.Seance > seances = _seanceHttpClient.GetSeances().ToList();

            
            /*
             * Необходимо проверить текущий список киносеансов и полученный от сервера на расхождения.
             * Если Списки разные,то текущий список обновляется.
             */
            bool flagUpdateSeances = false;//Изначально обновлять не надо

            if (Seances.Count() == seances.Count())
            {
                for(int i=0;i< Seances.Count; i++)
                {
                    if(Seances[i].Id!= seances[i].Id)//есть расхождения
                    {
                        flagUpdateSeances = true;//обновляем
                        break;
                    }
                }
            }
            else
            {
                flagUpdateSeances = true;//обновляем
            }

            //Обновление текущего списка
            if (flagUpdateSeances)
            {
                Seances.Clear();
                foreach (var seance in seances)
                    Seances.Add(new Model.Seance() { Id = seance.Id, Name = seance.Name, Start = seance.Start });
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        //не пригодилось, на удаление
        /// <summary>
        /// Метод проверяет наличие выделенного киносеанса в текущем списке.
        /// </summary>
        /// <returns></returns>
        public bool SelectedSeanceIsInSeances()
        {
            if (SelectedSeance == null)
                return false;
            // return Seances.Where(x => x.Id == SelectedSeance.Id).Count() > 0 ? true : false;
            if (Seances.Where(x => x.Id == SelectedSeance.Id).Count() > 0)
            {
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //-------------------------------------------------------------------------------------------------

        
        private AccessToWebApi.Entities.Order _newOrder;
        private WpfClient.Model.Order _newOrderFromServer;

        public WpfClient.Model.Order NewOrderFromServer
        {
            get { return _newOrderFromServer; }
            set
            {
                _newOrderFromServer = value;
                OnPropertyChanged("NewOrderFromServer");
            }
        }


        private int _countPlace;

        public int CountPlace
        {
            get { return _countPlace; }
            set
            {
                _countPlace = value;
                OnPropertyChanged("CountPlace");
            }
        }

        private CommandHandler _createNewOrder;
        public CommandHandler CreateNewOrder
        {
            get
            {
                return _createNewOrder ??
            (_createNewOrder = new CommandHandler(obj =>
            {
                WpfClient.Model.Seance selectSeance = obj as WpfClient.Model.Seance;
                if (selectSeance != null)
                {
                    _newOrder = new AccessToWebApi.Entities.Order() { CountPlace = _countPlace, IdSeance = selectSeance.Id };
                    AccessToWebApi.Entities.Order newSimpleOrderFromServer = GetResponseFromServer();
                    if (_newOrderFromServer == null)
                    {
                        NewOrderFromServer = new Model.Order();
                    }
                    NewOrderFromServer.Id = newSimpleOrderFromServer.Id;
                    NewOrderFromServer.IdSeance = newSimpleOrderFromServer.IdSeance;
                    NewOrderFromServer.CountPlace = newSimpleOrderFromServer.CountPlace;
                    NewOrderFromServer.TicketSales = newSimpleOrderFromServer.TicketSales;
                }
            },
            (obj) => 
                {
                    bool res1 = CountPlace > 0;
                    MessageBox.Show(res1.ToString());
                    return (obj as WpfClient.Model.Seance) != null && res1;
                }
            ));
            }


        }

        private AccessToWebApi.Entities.Order GetResponseFromServer()
        {
            return _orderHttpClient.PostOrder(_newOrder);
        }




        
    }
}
