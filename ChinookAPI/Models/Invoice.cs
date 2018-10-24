using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookAPI.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BillingAddress { get; set; }
        public string BIllingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public int Zipcode { get; set; }
        public decimal Total { get; set; }
    }
}
