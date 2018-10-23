using System;
using System.Collections.Generic;
using System.Text;

namespace AccessToWebApi.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int IdSeance { get; set; }

        public int CountPlace { get; set; }

        public string TicketSales { get; set; }
    }
}
