using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace examTest1
{
    class DataBase
    {
        public string strConexion =System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        NpgsqlConnection conn;
        public NpgsqlConnection Open()
        {
            try
            {
                conn = new NpgsqlConnection(strConexion);
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                conn.Close();
                throw new Exception(e.Message);
            }
        }

        public NpgsqlConnection Close()
        {
            try
            {
                conn = new NpgsqlConnection(strConexion);
                conn.Close();
                return conn;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        /**
         * campos
         * table
         * 
         */
        public string Select()
        {
            String data = "";
            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand("select * from example", conn);

            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();
            // Read all rows and output the first column in each row
            while (dr.Read())
            {
                data+=(dr[0].ToString()) + (dr[1].ToString()) + (dr[2].ToString());

            }


            // Close connection
            conn.Close();
            return data;
        }

    }
}
