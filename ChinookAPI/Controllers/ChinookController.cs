using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChinookAPI.DataAccess;
using ChinookAPI.Models;
using Microsoft.Extensions.Configuration;

namespace ChinookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChinookController : ControllerBase
    {
        private readonly ChinookStorage _storage;

        public ChinookController(IConfiguration config)
        {
            _storage = new ChinookStorage(config);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            return Ok(_storage.GetById(id));
        }

        [HttpGet("invoices")]
        public IActionResult GetAll()
        {
            return Ok(_storage.GetInvoice());
        }

        [HttpGet("lineitems/{id}")]
        public IActionResult GetInvoiceCount(int id)
        {
            return Ok(_storage.GetCount(id));
        }

        [HttpPost]
        public void AddInvoice(Invoice invoice)
        {
            _storage.Add(invoice);
        }
    }
}