using ChatServer.Shared;
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

        /// <summary>
        /// Connect to the SQLite Database
        /// </summary>
        public void Connect()
        {
            _dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            _dbConnection.Open();
        }

        /// <summary>
        /// Attempts to add a user to the Users table in the SQLite database
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
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

        /// <summary>
        /// Validates the credentials sent by the client and returns a response
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns></returns>
        public async Task<Response> LogIn(string email, string password)
        {
            Response response = null;

            try
            {
                string sql = "SELECT * FROM Users WHERE Email = @Email";

                SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);

                command.Parameters.AddWithValue("@Email", email);

                var reader = command.ExecuteReader();

                int ordEmail = reader.GetOrdinal("Email");
                int ordPassword = reader.GetOrdinal("Password");

                string dbEmail = null;
                string dbPassword = null;

                while (reader.Read())
                {
                    dbEmail = reader.GetString(ordEmail);
                    dbPassword = reader.GetString(ordPassword);
                }

                // If credentials are correct
                if (dbEmail == email && dbPassword == password)
                {
                    response = new Response()
                    {
                        Status = StatusCode.Success
                    };

                    Console.WriteLine(email + " has logged in");

                }
                // If either password or email is not correct
                else if (dbEmail != email || dbPassword != password)
                {
                    response = new Response()
                    {
                        Status = StatusCode.Failure,
                        Error = "Invalid username or password, please try again"
                    };
                }
                // If none of the above
                else
                {
                    response = new Response()
                    {
                        Status = StatusCode.Failure,
                        Error = "Unkown error"
                    };
                }

                return response;
            }
            catch (Exception exc)
            {
                response = new Response
                {
                    Status = StatusCode.Failure,
                    Error = exc.ToString()
                };
            }

            return response;

        }
    }
}
