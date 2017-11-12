using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Models;
using InvoiceApplication.DbModels;

namespace InvoiceApplication.Controllers
{
    public class CheckingAuthorityController : Controller
    {
        public TrackingDbContext _context;

        public CheckingAuthorityController(TrackingDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int invoiceId)
        {
            

            var invoices = from invoice in _context.BtsinvoiceAr
                           where invoice.InvoiceId == invoiceId
                           select invoice;
            InvoiceViewModel model = new InvoiceViewModel();
            var invoiceList = invoices.First();
            model.invoiceId = invoiceList.InvoiceId;
            model.invoiceDate = invoiceList.InvoiceDate;
            model.invoiceNumber = invoiceList.InvoiceNo;
            model.totalLocalAmount = invoiceList.TotalLocalAmt;
            model.exchangeRate = invoiceList.ExRate;
            model.CustomerName = invoiceList.CustomerName;
            model.customerId = invoiceList.CustomerId;
            model.DelivaryDate = invoiceList.DeliveryDate;
            model.currencyCode = invoiceList.CurrencyCode;
            model.Amount = invoiceList.TotalAmt;
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(CheckingAuthorityApproverModel model)
        {

            return View();
        }
    }
}