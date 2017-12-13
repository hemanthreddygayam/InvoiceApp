using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using InvoiceApplication.Models;
using System.Security.Claims;
using System.Globalization;
using static InvoiceApplication.Utilies;

namespace InvoiceApplication.Controllers
{
    public class InvoiceActionController : Controller
    {
        private DbHelper _helper;
        public InvoiceActionController()
        {
            _helper = new DbHelper();
        }
        [HttpPost]
        [Authorize(Policy = "Checker")]
        public IActionResult Check(InvoiceApprovalModel model)
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            try
            {
                string color = UpdateStatusForInvoice(model.InvoiceId, model.Status,model.Remarks);
                ViewBag.UpdateMessage = String.Format("Successfully updated status for Invoice:{0}", model.InvoiceId);

                return Json(color);

            }
            catch (Exception ex)
            {
                return Json("Error in updating status");
            }
            return Json("Error in updating status");
        }


        public string UpdateStatusForInvoice(long invoiceId, InvoiceStatus status,string remarks)
        {
            string color = string.Empty;
            IDBService service = new DBservice(_helper);

            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string email = service.GetEmailForUser(username);

            var invoice = service.GetInvoice(invoiceId);
            var invoiveViewModel = new InvoiceViewModel
            {
                AccountDate = invoice.AccountDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),
                CustomerName = invoice.CustomerName,
                DelivaryDate = invoice.DeliveryDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),
                TotalLocalAmount = invoice.TotalLocalAmt.ToString("C",
                                                CultureInfo.CreateSpecificCulture("en-IN")),
                DueDate = invoice.DueDate,
                InvoiceNumber = invoice.InvoiceNo

            };

            if (status == InvoiceStatus.CheckPending)
            {
                service.UpdatePendingStatusForChecker(invoiceId, username,remarks);
                color = service.getColorToUpdate((int)InvoiceStatus.CheckPending);

            }
            else if (status == InvoiceStatus.Checked)
            {
                service.UpdateCheckedStatusForChecker(invoiceId, username,remarks);
                color = service.getColorToUpdate((int)InvoiceStatus.Checked);
                ConstructEmail construct = new ConstructEmail();
                construct.SendEmail(invoiveViewModel, email);
            }
            else if (status == InvoiceStatus.CheckRejected)
            {
                service.UpdateRejectedStatusForChecker(invoiceId, username,remarks);
                color = service.getColorToUpdate((int)InvoiceStatus.CheckRejected);

            }
            else if(status == InvoiceStatus.Approved)
            {
                service.UpdateApprovedStatusForApprover(invoiceId, username,remarks);
                color = service.getColorToUpdate((int)InvoiceStatus.Approved);

            }
            else if(status == InvoiceStatus.ApprovePending)
            {
                service.UpdatePendingStatusForApprover(invoiceId, username,remarks);
                color = service.getColorToUpdate((int)InvoiceStatus.ApprovePending);

            }
            else if(status == InvoiceStatus.ApproveRejected)
            {
                service.UpdateRejectedStatusForApprover(invoiceId, username,remarks);
                color = service.getColorToUpdate((int)InvoiceStatus.ApproveRejected);
            }
            else
            {
                throw new Exception("Invalid Status");
            }
            return color;
        }


        [HttpPost]
        [Authorize(Policy = "Approver")]
        public IActionResult Approve(InvoiceApprovalModel model)
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            try
            {
                string color = UpdateStatusForInvoice(model.InvoiceId, model.Status,model.Remarks);
                ViewBag.UpdateMessage = String.Format("Successfully updated status for Invoice:{0}", model.InvoiceId);
                return new JsonResult(color);

            }
            catch (Exception ex)
            {
                new JsonResult("Error in updating status");
            }
            return new JsonResult("Error in updating status");
        }
        
    }

}