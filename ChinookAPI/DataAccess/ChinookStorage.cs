using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ChinookAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ChinookAPI.DataAccess
{
    public class ChinookStorage
    {
        private readonly string ConnectionString;

        public ChinookStorage(IConfiguration config)
        {
            ConnectionString = config.GetSection("ConnectionString").Value;
        }

        public List<query1> GetById(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var result = connection.Query<query1>(@"SELECT
    	                                                    InvoiceId = I.InvoiceId,
	                                                        SalesAgent = E.FirstName + ' ' + E.LastName
                                                        FROM Employee E
                                                        Join Customer C
                                                        ON E.EmployeeId = C.SupportRepId
                                                        JOIN Invoice I
                                                        ON I.CustomerId = C.CustomerId
                                                        WHERE E.EmployeeId = @id
                                                        ORDER BY I.InvoiceId", new { id = id } );

                return result.ToList();
            }
        }

        public List<Query2> GetInvoice()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var result = connection.Query<Query2>(@"SELECT
                                                             Total = I.Total,
                                                             Customer = C.FirstName + ' ' + C.LastName,
                                                             Country = C.Country,
                                                             SalesAgent = E.FirstName + ' ' + E.LastName
                                                        FROM Customer C
                                                        JOIN Employee E
                                                        ON C.SupportRepId = E.EmployeeId
                                                        JOIN Invoice I
                                                        ON I.CustomerId = C.CustomerId");

                return result.ToList();
            }
        }
        public Query3 GetCount(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var result = connection.QueryFirst<Query3>(@"SELECT
                                                            Invoice = InvoiceId,
                                                            NumberOfItems = Count(InvoiceLineId)
                                                        FROM InvoiceLine
                                                        WHERE InvoiceId = @id
                                                        GROUP BY InvoiceId", new { id = id});
                return result; 
            }
        }

        public bool Add(Invoice invoice)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                var result = db.Execute(@"INSERT INTO [dbo].[Invoice]([CustomerId],[InvoiceDate],[BillingAddress],[BillingCity],
                                                            [BillingState],[BillingCountry],[BillingPostalCode],[Total])
                                VALUES(@CustomerId, @InvoiceDate, @BillingAddress, @BillingCity, @BillingState, @BillingCountry, @Zipcode, @Total", invoice);
                return result == 1;
            }
        }
    }
}


//public bool Add(Invoice invoice)
//{
//    using (var db = new SqlConnection(ConnectionString))
//    {
//        db.Open();

//        var command = db.CreateCommand();

//        command.CommandText = @"INSERT INTO [dbo].[Invoice]([CustomerId],[InvoiceDate],[BillingAddress],[BillingCity],
//                                                            [BillingState],[BillingCountry],[BillingPostalCode],[Total])
//                                VALUES(@CustomerId, @InvoiceDate, @BillingAddress, @BillingCity, @BillingState, @BillingCountry, @Zipcode, @Total)";

//        command.Parameters.AddWithValue("@CustomerId", invoice.CustomerId);
//        command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
//        command.Parameters.AddWithValue("@BillingAddress", invoice.BillingAddress);
//        command.Parameters.AddWithValue(@"BillingCity", invoice.BIllingCity);
//        command.Parameters.AddWithValue(@"BillingState", invoice.BillingState);
//        command.Parameters.AddWithValue(@"BillingCountry", invoice.BillingCountry);
//        command.Parameters.AddWithValue(@"Zipcode", invoice.Zipcode);
//        command.Parameters.AddWithValue(@"Total", invoice.Total);

//        var result = command.ExecuteNonQuery();

//        return result == 1;
//    }
//}
