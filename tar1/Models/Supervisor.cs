using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Supervisor
    {
        public int sup_id { get; set; }
        public string sup_name { get; set; }
        public string sup_phone { get; set; }

        public List<Supervisor> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.Read("customerDBConnectionString", "supervisor");
        }
    }
}
