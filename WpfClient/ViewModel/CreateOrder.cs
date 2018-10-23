using AccessToWebApi;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AccessToWebApi.Entities;
using WpfClient.Model;

namespace WpfClient.ViewModel
{
    public class CreateOrder : INotifyPropertyChanged
    {
        private HTTPForOrder _httpClient;
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
                AccessToWebApi.Entities.Seance selectSeance = obj as AccessToWebApi.Entities.Seance;
                if (selectSeance != null)
                {
                    _newOrder = new AccessToWebApi.Entities.Order() { CountPlace = _countPlace, IdSeance = selectSeance.Id };
                    AccessToWebApi.Entities.Order newSimpleOrderFromServer=GetResponseFromServer();
                    NewOrderFromServer.Id = newSimpleOrderFromServer.Id;
                    NewOrderFromServer.IdSeance = newSimpleOrderFromServer.IdSeance;
                    NewOrderFromServer.CountPlace = newSimpleOrderFromServer.CountPlace;
                    NewOrderFromServer.TicketSales= newSimpleOrderFromServer.TicketSales;
                }
            },
            (obj) => (obj as AccessToWebApi.Entities.Seance) !=null));
            }

            
        }

        private AccessToWebApi.Entities.Order GetResponseFromServer()
        {
            return _httpClient.PostOrder(_newOrder);
        }




        public CreateOrder(HTTPForOrder httpClient)
        {
            _httpClient = httpClient;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
