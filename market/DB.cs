using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace market
{
    public static class DB
    {
        private const string ConString =
            "Server=localhost;Port=3306;Database=market;Uid=root;Pwd=;CharSet=utf8;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConString);
        }
    }
}
