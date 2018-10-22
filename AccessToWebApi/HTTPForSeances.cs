using AccessToWebApi.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessToWebApi
{
    public class HTTPForSeances
    {
        public static HTTP http;

        public HTTPForSeances()
        {
            if (http == null)
            {
                http = new HTTP();
            }
        }

        public IEnumerable<Seance> GetSeances()
        {
            string result = "";
            result=http.Request("https://localhost:44333/api/seance/", "Get");            
            return JsonConvert.DeserializeObject<IEnumerable<Seance>>(result);
        }

        public string GetSeancesString()
        {
            string result = "";
            result = http.Request("https://localhost:44333/api/seance/", "Get");
            return result;
        }

    }
}
