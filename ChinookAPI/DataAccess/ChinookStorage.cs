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

                command.Parameters.AddWithValue("@id", id);

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
        public List<Query2> GetInvoice()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = @"SELECT
	                                        Total = I.Total,
	                                        Customer = C.FirstName + ' ' + C.LastName,
	                                        Country = C.Country,
	                                        SalesAgent = E.FirstName + ' ' + E.LastName
                                        FROM Customer C
                                        JOIN Employee E
                                        ON C.SupportRepId = E.EmployeeId
                                        JOIN Invoice I
                                        ON I.CustomerId = C.CustomerId";

                var reader = command.ExecuteReader();

                var queryList = new List<Query2>();
                while (reader.Read())
                {
                    var result = new Query2
                    {
                        Total = (decimal)reader["Total"],
                        CustomerName = reader["Customer"].ToString(),
                        Country = reader["Country"].ToString(),
                        SalesAgent = reader["SalesAgent"].ToString()
                    };
                    queryList.Add(result);
                }
                return queryList;
            }
        }
        public Query3 GetCount(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = @"SELECT
	                                        Invoice = InvoiceId,
	                                        NumberOfItems = Count(InvoiceLineId)
                                        FROM InvoiceLine
                                        WHERE InvoiceId = @id
                                        GROUP BY InvoiceId";

                command.Parameters.AddWithValue("@id", id);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var result = new Query3
                    {
                        Invoice = (int)reader["Invoice"],
                        NumberOfItems = (int)reader["NumberOfItems"]
                    };
                    return result;
                }
                return null;
            }
        }
    }
}
