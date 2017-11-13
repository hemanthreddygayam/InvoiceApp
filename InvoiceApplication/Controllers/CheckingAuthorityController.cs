﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Models;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApplication.Controllers
{
    public class CheckingAuthorityController : Controller
    {
        public TrackingDbContext _context;

        public CheckingAuthorityController(TrackingDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Policy = "")]
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
        [Authorize(Policy ="")]
        public IActionResult Index(InvoiceApprovalModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            UpdateStatusForInvoice(model.InvoiceId, model.status);
            return View();
        }


        public void UpdateStatusForInvoice(long invoiceId, string status)
        {
            var modifyinvoice = from invoice in _context.BtsinvoiceAr
                                where invoice.InvoiceId == invoiceId
                                select invoice;
            var InvoiceApprove = modifyinvoice.First();
            if (status == "Pending")
            {
                InvoiceApprove.IsCheckedPending = true;
                InvoiceApprove.CheckedBy = "";
                InvoiceApprove.CheckedDate = DateTime.Now.ToString();

            }
            else if (status == "Approved")
            {
                InvoiceApprove.IsChecked = true;
                InvoiceApprove.CheckedBy = "";
                InvoiceApprove.CheckedDate = DateTime.Now.ToString();
            }
            else if (status == "Rejected")
            {
                InvoiceApprove.IsWaitingApproval = false;
                InvoiceApprove.CheckedBy = "";
                InvoiceApprove.CheckedDate = DateTime.Now.ToString();
            }
            else
            {

            }

            _context.Entry(InvoiceApprove).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}