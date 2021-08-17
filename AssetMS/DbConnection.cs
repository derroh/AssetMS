using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMS
{
    public class DbConnection
    {
        static string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
        MySqlConnection conn = new MySqlConnection(conString);
        public MySqlConnection ActiveCon()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
          return conn;
        }
    }
}
