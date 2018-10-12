using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
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
            timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            InitializeComponent();
            ListSeance.ItemsSource = listSeance;
        }

        

        public void RequestToServer()
        {
            try { 
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
            catch (Exception)
            {
                MessageBox.Show("Ошибка при запросе на сервер списка сеансов");
            }
}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button but = sender as Button;
                if (but == null)
                {
                    return;
                };

                int seanceId = (int)but.Tag;
                Seance seance = listSeance.Find(x => x.Id == seanceId);


                CreateOrderWindow window = new CreateOrderWindow
                {
                    CurrentSeance = seance
                };
                window.Show();
                window.ShowCurrentSeance();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка в обработчике кнопки");
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                RequestToServer();
            }
            catch (Exception)
            {
               
                listSeance = new List<Seance>();
            }
            
            ListSeance.ItemsSource = listSeance;
        }
    }
        public class Seance
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Start { get; set; }

            

        }

    

}
