using System.Data.SQLite;
using Persons.Abstractions.Models;
using Persons.Abstractions.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.Data.SqlClient;
using System.Text.Json;

namespace Persons.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connString;
        private readonly ILogger _logger;

        public PersonRepository(string connString, ILogger logger)
        {
            _connString = connString;
            _logger = logger;
        }

        public Person Find(Guid guid)
        {
            using (IDbConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();

                var sql = "Select * From people_tbl Where GUID = @Guid";
                       
                var parameters = new DynamicParameters();
                parameters.Add("@Guid", guid.ToString("D"), DbType.String);

                var result = conn.Query<Person>(sql, parameters).FirstOrDefault();

                if (result != null)
                    return result;

                return null;
            }
        }

        public void Insert(Person newPerson)
        {
            using (IDbConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();

                var sql = "Insert Into people_tbl (Name, Birthday, GUID) Values (@Name, @Birthday, @Guid)";

                var parameters = new DynamicParameters();

                parameters.Add("@Guid", newPerson.GUID, DbType.String);
                parameters.Add("@Name", newPerson.Name, DbType.String);
                parameters.Add("@Birthday", newPerson.Birthday.ToString("d"), DbType.String);

                conn.Execute(sql, parameters);
            }
        }
    }
}
