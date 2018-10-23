using AccessToWebApi.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessToWebApi
{
    public class HTTPForOrder
    {
        public static HTTP http;

        public HTTPForOrder()
        {
            if (http == null)
            {
                http = new HTTP();
            }
        }

        public Order PostOrder(Order newOrder)
        {
            string result = "";
            string strNewOrder = JsonConvert.SerializeObject(newOrder);
            result = http.Request("https://localhost:44333/api/order", "Post", strNewOrder);
            return JsonConvert.DeserializeObject<Order>(result);
        }

       
    }
}
