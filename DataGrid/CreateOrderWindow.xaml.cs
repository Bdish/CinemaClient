using Newtonsoft.Json;
using System;

using System.Net.Http;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;



namespace DataGrid
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
       

        public Seance CurrentSeance { get; set; }

        public Order NewOrder { get; set; }

        public CreateOrderWindow()
        {
            InitializeComponent();

           
        }


        public void ShowCurrentSeance()
        {
            // MessageBox.Show(CurrentSeance.Id.ToString() + "  " + CurrentSeance.Name + " " + CurrentSeance.Start);
            //DisplaySeance=CurrentSeance;
            CurrentSeanceId.Text = CurrentSeance.Id.ToString();
            CurrentSeanceName.Text = CurrentSeance.Name;
            CurrentSeanceStart.Text = CurrentSeance.Start;
        }

        

        private void InputNum_KeyDown(object sender, KeyEventArgs e)
        {
            string inputStr = e.Key.ToString();

            if (!Regex.Match(inputStr, @"[0-9]+").Success)
            {
                e.Handled = true;
            }
        }

         private void Button_Click(object sender, RoutedEventArgs e)
         {
              if (NewOrder == null)
              {
                  NewOrder = new Order(); 
              }

              NewOrder.IdSeance = CurrentSeance.Id;
              NewOrder.CountPlace = int.Parse(InputNum.Text);

            Order order = new Order() {  IdSeance = Int32.Parse(CurrentSeanceId.Text), CountPlace = Int32.Parse(InputNum.Text) };
            

            
            AddNewOrder(order);



        }


        public void AddNewOrder(Order order)
        {
            if (order == null)
            {
                MessageBox.Show("Для оформления заказа не все даннные были введены");
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                var jsonEmp = JsonConvert.SerializeObject(order);

                var url2 = @"https://localhost:44333/api/order"; //Строка по которой производиться обращение к таблице
                StringContent stringC = new StringContent(jsonEmp, Encoding.UTF8, "application/json"); //Строка которая будет передаваться web сервису
                var res = client.PostAsync(url2, stringC).Result; //отправляем 

                var jsonOrder = JsonConvert.DeserializeObject<Order>(res.Content.ReadAsStringAsync().Result); //Получаем полученый объект в ходе выполнения записи в базу данных
                order.Id = jsonOrder.Id; //передаем нашему объекту реальный ID из базы данных

                NewOrderId.Text = jsonOrder.Id.ToString();
                NewOrderIdSeance.Text = jsonOrder.IdSeance.ToString();
                NewOrderCountPlace.Text = jsonOrder.CountPlace.ToString();
            }
        }

    }

    public class Order
    {
        public int Id { get; set; }
       
        public int IdSeance { get; set; }

        
        public int CountPlace { get; set; }
    }
}
