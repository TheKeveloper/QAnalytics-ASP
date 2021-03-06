﻿using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace QAnalytics.Models
{
    public class DBManager
    {
        //Dummy connection string to use with local db. Must be changed for deployment
        public const string DB_CONNECTION_STRING = "Database=hcourser_db; Data Source=localhost; User id=root; Password=KevinRox";
        public const string TABLE_NAME = "courses";

        public MySqlConnection connection{
            get;
            private set;
        }

        public DBManager()
        {
            connection = null;
        }

        public MySqlConnection Open(){
            connection = new MySqlConnection(DB_CONNECTION_STRING);
            connection.Open();
            return connection;
        }

        public void Close(){
            connection.Close();
        }

        public MySqlCommand CreateCommand(){
            return connection.CreateCommand();
        }
    }
}
