using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ChinookAPI.Models;

namespace ChinookAPI.DataAccess
{
    public class ChinookStorage
    {
        private const string ConnectionString = "Server=(local); Database=Chinook; Trusted_Connection=True;";
        public List<query1> GetById(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = @"SELECT *
                                        FROM Employee E
                                        Join Customer C
                                        ON E.EmployeeId = C.SupportRepId
                                        JOIN Invoice I
                                        ON I.CustomerId = C.CustomerId
                                        WHERE E.EmployeeId = @id";
                var reader = command.ExecuteReader();

                var queryList = new List<query1>();
                while (reader.Read())
                {
                    var result = new query1
                    {
                        InvoiceId = (int)reader["InvoiceId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString()
                    };
                    queryList.Add(result);
                }
                return queryList;
            }
        }
    }
}
