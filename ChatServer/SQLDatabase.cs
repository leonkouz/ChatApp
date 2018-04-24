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

    }
}
