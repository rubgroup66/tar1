using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Architect
    {
        public int arc_id { get; set; }
        public string arc_name { get; set; }

        public List<Architect> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.Read2("customerDBConnectionString", "Architect");
        }
    }
}
