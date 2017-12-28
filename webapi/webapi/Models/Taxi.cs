using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi.Models
{
    public class Taxi
    {
        public int id { set; get; }
        public int taxinum { set; get; }
        public string date { get; set; }
        public string time { get; set; }
        public string money { get; set; }
    }
}