using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Customer
    {
        public string project_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone_num { get; set; }
        public string email { get; set; }
        public string supervisor { get; set; }
        public string architect { get; set; }

        public Customer(string _project_name, string _first_name, string _last_name, string _phone_num, string _email, string _supervisor, string _architect)
        {
            project_name = project_name;
            first_name = _first_name;
            last_name = _last_name;
            phone_num = _phone_num;
            email = _email;
            supervisor = _supervisor;
            architect = _architect;
        }

        public Customer()
        {

        }

        public int insert()
        {
            DBservices dbs = new DBservices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }

        public List<Customer> GetCustomers()
        {
            DBservices db = new DBservices();
            List<Customer> customerList = new List<Customer>();
            customerList = db.GetCustomers();
            return customerList;
        }

        public void Put(Customer c)
        {
            DBservices db = new DBservices();
            db.Put(c);
        }

        public void DeleteCust(string custID)
        {
            DBservices db = new DBservices();
            db.DeleteCust(custID);
        }
    }
}
