using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AccessToWebApi.Entities
{
    public class Seance 
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Start { get; set; }      
    }
}
