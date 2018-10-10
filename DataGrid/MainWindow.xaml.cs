using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Seance> listSeance;
        public MainWindow()
        {
            try
            {

                RequestToServer();
            }
            catch(Exception)
            {
                //нет соединения
                listSeance = new List<Seance>();
            }
            InitializeComponent();
        }

        //Загрузка содержимого таблицы
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource = listSeance;
        }

        //Получаем данные из таблицы
        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Seance path = grid.SelectedItem as Seance;
            MessageBox.Show(" ID: " + path.Id + "\n Название фильма: " + path.Name + "\n Начало: " + path.Start
                );
        }

        public void RequestToServer()
        {
            string responseText = "";
            
            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("text/xml"));//не пашет???
                    var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44333/api/seance/");
                    responseText = client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
                }

            }
            listSeance = JsonConvert.DeserializeObject<IEnumerable<Seance>>(responseText).ToList();//не работает

        }
    }
        public class Seance
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Start { get; set; }

             public Seance(int id, string name, DateTime start)
             {
                 this.Id = id; 
                 this.Name = name;
                 this.Start = start;      
            }

        }


    
}
