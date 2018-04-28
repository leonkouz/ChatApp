﻿using ChatServer.Core;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    // Using SQL
    // http://blog.tigrangasparian.com/2012/02/09/getting-started-with-sqlite-in-c-part-one/

    // creating booleans in SQL
    // https://stackoverflow.com/questions/843780/store-boolean-value-in-sqlite



    public class SQLDatabase
    {
        #region Private Fields

        private SQLiteConnection _dbConnection;

        #endregion

        #region Constructor

        public SQLDatabase()
        {
            // Open database
            _dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            _dbConnection.Open();

        }
        
        #endregion

        public void Connect()
        {
            _dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            _dbConnection.Open();
        }

        public async Task<Response> AddUser(string email, string firstName, string lastName, string password)
        {
            try
            {
                string sql = "INSERT INTO Users (Email, Password, FirstName, LastName)";
                sql += " VALUES (@Email, @Password, @FirstName, @LastName)";

                SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                command.ExecuteNonQuery();

                Response response = new Response
                {
                    Status = StatusCode.Success
                };

                return response;
            }
            catch (SQLiteException exc)
            {
                string errorMessage = exc.Message;

                if (exc.ErrorCode == 19 && exc.Message.Contains("UNIQUE"))
                {
                    // The primary key (email) is not unique
                    errorMessage = "This email is already in use";
                }

                Response response = new Response
                {
                    Status = StatusCode.Failure,
                    Error = errorMessage
                };

                return response;
            }
        }
    }
}
