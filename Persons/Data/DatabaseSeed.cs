using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Persons.Data
{
    public class DatabaseSeed
    {
        public static void CreateLocalDatabase(string connStr)
        {
            using (var conn = new SQLiteConnection(connStr))
            {
                conn.Open();

                try
                {
                    var sql_tableGenerate = "CREATE TABLE people_tbl" +
                                            "('ID' INTEGER NOT NULL UNIQUE," +
                                            " 'Name' TEXT NOT NULL," +
                                            " 'Birthday' TEXT NOT NULL," +
                                            " 'GUID' TEXT NOT NULL," +
                                            " PRIMARY KEY('ID' AUTOINCREMENT));";

                    var a = conn.Execute(sql_tableGenerate);
                }
                catch (System.Data.SQLite.SQLiteException)
                {
                    return;
                }
            }
        }
    }
}
