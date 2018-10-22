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

namespace WpfClient.ViewModel
{
    public class SeancesViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private Seance _selectedSeance;
        private HTTPForSeances _httpClient;

        public ObservableCollection<Seance> Seances { get; set; }

        public Seance SelectedSeance
        {
            get { return _selectedSeance; }
            set
            {
               /* if (!SelectedSeanceIsInSeances())
                {*/
                    _selectedSeance = value;
                    OnPropertyChanged("SelectedSeance");
               // }
                
            }
        }

        public SeancesViewModel(HTTPForSeances httpClient)
        {
            Seances = new ObservableCollection<Seance>();
            _httpClient = httpClient;

            _timer = new System.Windows.Threading.DispatcherTimer();

            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

        }

        private void TimerTick(object sender, EventArgs e)
        {
            List< Seance > seances = _httpClient.GetSeances().ToList();

            
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
                    Seances.Add(seance);
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
    }
}
