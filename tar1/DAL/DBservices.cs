using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using WebApplication1.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts a person to the perosns table 
    //--------------------------------------------------------------------------------------------------
    public int insert(Customer cust)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("customerDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String pStr = BuildInsertCommand(cust);      // helper method to build the insert string

        cmd = CreateCommand(pStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private String BuildInsertCommand(Customer cust)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', '{3}', '{4}','{5}',{6})", cust.project_name, cust.first_name, cust.last_name, cust.phone_num, cust.email, cust.supervisor, cust.architect);
        String prefix = "INSERT INTO Customer1 " + "(project_name, first_name, last_name, phone_num, email,sup_name,arc_name) ";

        command = prefix + sb.ToString();

        return command;
    }

    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }

    public List<Supervisor> Read(string conString, string tableName)
    {

        SqlConnection con = null;
        List<Supervisor> lc = new List<Supervisor>();
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Supervisor s = new Supervisor();
                s.sup_id = Convert.ToInt32(dr["sup_id"]);
                s.sup_name = (string)dr["sup_name"];
                s.sup_phone = (string)dr["sup_phone"];
                lc.Add(s);
            }

            return lc;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public List<Architect> Read2(string conString2, string tableName2)
    {

        SqlConnection con = null;
        List<Architect> lc = new List<Architect>();
        try
        {
            con = connect(conString2); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName2;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Architect a = new Architect();
                a.arc_id = Convert.ToInt32(dr["arc_id"]);
                a.arc_name = (string)dr["arc_name"];
                lc.Add(a);
            }

            return lc;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public List<Customer> GetCustomers()
    {
        SqlConnection con;
        List<Customer> CustomersList = new List<Customer>();

        try
        {
            con = connect("customerDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        try
        {
            String selectSTR = "SELECT * FROM Customer1 ";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {// Read till the end of the data into a row
             // read first field from the row into the list collection
                Customer customer = new Customer();
                customer.project_name = Convert.ToString(dr["project_name"]);
                customer.first_name = Convert.ToString(dr["first_name"]);
                customer.last_name = Convert.ToString(dr["last_name"]);
                customer.phone_num = Convert.ToString(dr["phone_num"]);
                customer.email = Convert.ToString(dr["email"]);
                customer.supervisor = Convert.ToString(dr["sup_name"]);
                customer.architect = Convert.ToString(dr["arc_name"]);

                CustomersList.Add(customer);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        return CustomersList;


    }
    //----------------------------------------------------------------------------

    public int Put(Customer c)
    {

        SqlConnection con;
        SqlCommand cmd;


        try
        {
            con = connect("customerDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCustomer(c);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command
        
        try
        {
           int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
            


        }
        catch (Exception ex)
        {

            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private string BuildUpdateCustomer(Customer c)
    {
        string v = "UPDATE Customer1 SET first_name='" + c.first_name + "' ,last_name='" + c.last_name + "' ,phone_num='" + c.phone_num + "' ,email='" + c.email + "' ,sup_name='" + c.supervisor + "' ,arc_name='" + c.architect + "' WHERE project_name='" + c.project_name+"'";
        string cmdStr = v;
        return cmdStr;

    }

    //-----------------------------------------------------
    public int DeleteCust(string custID)
    {

        SqlConnection con;
        SqlCommand cmd;


        try
        {
            con = connect("customerDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteCust(custID);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command


        try
        {
            int numAffected = cmd.ExecuteNonQuery(); // execute the comm

            return numAffected;


        }
        catch (Exception ex)
        {

            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private string BuildDeleteCust(string custID)
    {
        string cmdStr = "DELETE FROM Customer1  WHERE project_name='" + custID + "'";
        return cmdStr;
    }

    //public void Put(Customer c)
    //{
    //    SqlConnection con;

    //    try
    //    {
    //        con = connect("customerDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
    //    }

    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    try
    //    {
    //        String updateSTR = "UPDATE [bgroup66_test2].[dbo].[Customer1] SET project_name= " + @c.project_name + " first_name= " + c.first_name.ToString() + " last_name= " + c.last_name.ToString() + " phone_num= " + c.phone_num.ToString() + " email= " + c.email.ToString() + " sup_name= " + c.supervisor.ToString() + " arc_name= " + c.architect.ToString() + " WHERE project_name= " + c.project_name;

    //        SqlCommand cmd = new SqlCommand(updateSTR, con);

    //        int numEffected = cmd.ExecuteNonQuery();

    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //            throw (ex);
    //    }
    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }
    //}

}
    
    //--------------------------------------------------------------------------------------------------
    // This method filters from DB
    //--------------------------------------------------------------------------------------------------
    //public List<Person> Filter(Filter filter)
    //{
    //    SqlConnection con;
    //    List<Person> personList = new List<Person>();

    //    try
    //    {
    //        con = connect("personDBConnectionString");
    //    }

    //    catch (Exception ex)
    //    {
    //        throw (ex);
    //    }

    //    try
    //    {
    //        string selectSTR = "SELECT * FROM [bgroup66_test1].[dbo].[PersonTbl] where age>=" + filter.MinAge + "and age <=" + filter.MaxAge + "and gender like '" +filter.Gender + "'";

    //        SqlCommand cmd = new SqlCommand(selectSTR, con);

    //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //        while (dr.Read())
    //        {
    //            Person person = new Person();

    //            person.name = Convert.ToString(dr["name"]);
    //            person.familyName = Convert.ToString(dr["familyName"]);
    //            person.address = Convert.ToString(dr["address"]);
    //            person.gender = Convert.ToString(dr["gender"]);
    //            person.age = Convert.ToDouble(dr["age"]);
    //            person.height = Convert.ToDouble(dr["height"]);
    //            person.image = Convert.ToString(dr["image"]);

    //            personList.Add(person);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return personList; 
    //}
    //--------------------------------------------------------------------------------------------------
    // This method inserts a car to the cars table 
    //--------------------------------------------------------------------------------------------------
    //public int insert(Car car)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("carsDBConnectionString"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    String cStr = BuildInsertCommand(car);      // helper method to build the insert string

    //    cmd = CreateCommand(cStr, con);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}

    ////--------------------------------------------------------------------
    //// insert a movie
    ////--------------------------------------------------------------------
    //public int insert(movie mov)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("moviesDBConnectionString"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    String cStr = BuildInsertCommand(mov);      // helper method to build the insert string

    //    cmd = CreateCommand(cStr, con);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}


    ////--------------------------------------------------------------------
    //// Build the Insert a movie command String
    ////--------------------------------------------------------------------
    //private String BuildInsertCommand(movie mov)
    //{
    //    String command;

    //    StringBuilder sb = new StringBuilder();
    //    // use a string builder to create the dynamic string
    //    sb.AppendFormat("Values('{0}', '{1}' )", mov.Name, mov.Actor);
    //    String prefix = "INSERT INTO Movies " + "(name, actor) ";
    //    command = prefix + sb.ToString();

    //    return command;
    //}

    ////--------------------------------------------------------------------
    //// Build the Insert command String
    ////--------------------------------------------------------------------
    //private String BuildInsertCommand(Car car)
    //{
    //    String command;

    //    StringBuilder sb = new StringBuilder();
    //    // use a string builder to create the dynamic string
    //    sb.AppendFormat("Values('{0}', '{1}' ,{2}, {3})", car.Model, car.Manufacturer, car.Year.ToString(), car.Price.ToString());
    //    String prefix = "INSERT INTO Cars " + "(model, manufacturer, year, price) ";
    //    command = prefix + sb.ToString();

    //    return command;
    //}
    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand
    ////---------------------------------------------------------------------------------
    //private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    //{

    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object

    //    cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

    //    return cmd;
    //}

    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand
    ////---------------------------------------------------------------------------------

    //public List<double> getCarPrices()
    //{

    //    SqlConnection con;
    //    List<double> prices = new List<double>();

    //    try
    //    {

    //        con = connect("carsConnectionString"); // create a connection to the database using the connection String defined in the web config file
    //    }

    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);

    //    }

    //    try
    //    {
    //        String selectSTR = "SELECT price FROM usedCars";

    //        SqlCommand cmd = new SqlCommand(selectSTR, con);

    //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //        while (dr.Read())
    //        {// Read till the end of the data into a row
    //            // read first field from the row into the list collection
    //            prices.Add(Convert.ToDouble(dr["price"]));
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);

    //    }
    //    return prices;
    //}


    ////--------------------------------------------------------------------
    //// Read from the DB into a table
    ////--------------------------------------------------------------------
    //public void readCarsDataBase()
    //{

    //    SqlConnection con = connect("carsConnectionString"); // open the connection to the database/

    //    String selectStr = "SELECT * FROM Cars"; // create the select that will be used by the adapter to select data from the DB

    //    SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

    //    DataSet ds = new DataSet("carsDS"); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB

    //    da.Fill(ds);       // Fill the datatable (in the dataset), using the Select command

    //    dt = ds.Tables[0]; // point to the cars table , which is the only table in this case
    //}


    ////--------------------------------------------------------------------
    //// insert a movie
    ////--------------------------------------------------------------------
    //public int delete(string prefix)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("moviesDBConnectionString"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    String cStr = BuildDeleteCommand(prefix);      // helper method to build the insert string

    //    cmd = CreateCommand(cStr, con);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}

    //private string BuildDeleteCommand(string prefix)
    //{


    //    string cmdStr = "DELETE FROM Movies WHERE actor LIKE '" + prefix + "%'";
    //    return cmdStr;


    //}


