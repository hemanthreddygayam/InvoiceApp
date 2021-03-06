﻿using InvoiceApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApplication.Controllers
{
    public class ConstructEmail
    {

        public void SendEmail(InvoiceViewModel model,string email)
        {
            string subject = String.Format("Approve Invoice for InvoiceNo {0}",model.InvoiceNumber);
            string body = ConstructMessage(model).ToString();
            string msg = @"mailto:" + email;
           // System.Diagnostics.Process.Start(msg);


        }
        public StringBuilder ConstructMessage(InvoiceViewModel model)
        {
            StringBuilder messge = new StringBuilder();
            messge.Append("<html>");
            messge.Append("<table>");
            messge.Append("<tr>");
            messge.Append("</tr>");

            messge.Append("<tr>");
            messge.Append("<td>Invoice Number</td>");
            messge.Append(String.Format("<td>{0}</td>", model.InvoiceNumber));
            messge.Append("</tr>");

            messge.Append("<tr>");
            messge.Append("<td>Customer Name</td>");
            messge.Append(String.Format("<td>{0}</td>", model.CustomerName));
            messge.Append("</tr>");

            messge.Append("<tr>");
            messge.Append("<td>Due Date</td>");
            messge.Append(String.Format("<td>{0}</td>", model.DueDate.Date.ToString()));
            messge.Append("</tr>");

            messge.Append("<tr>");
            messge.Append("<td>Delivary Date</td>");
            messge.Append(String.Format("<td>{0}</td>", model.DelivaryDate));
            messge.Append("</tr>");

            messge.Append("<tr>");
            messge.Append("<td>Total Amount</td>");
            messge.Append(String.Format("<td>{0}</td>", model.TotalLocalAmount.ToString()));
            messge.Append("</tr>");
            string url = "";
            messge.Append("</table>");
            messge.Append(String.Format("<a> href='{0}'>Approve Invoice</a>", url));
            messge.Append("</html>");


            return messge;
        }
    }
}
