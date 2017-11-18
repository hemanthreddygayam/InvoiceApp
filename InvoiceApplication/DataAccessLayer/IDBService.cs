using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.DataAccessLayer
{
    interface IDBService
    {
         DbUserModel FetchUser(string userName);
         DbInvoiceModel GetInvoice(long invoiceId);
         void UpdatePendingStatusForChecker(long invoiceId,string username);
         void UpdateCheckedStatusForChecker(long invoiceId, string username);
         void UpdateRejectedStatusForChecker(long invoiceId, string username);
         void UpdatePendingStatusForApprover(long invoiceId, string username);
         void UpdateApprovedStatusForApprover(long invoiceId, string username);
         void UpdateRejectedStatusForApprover(long invoiceId, string username);


    }

    public class DBservice : IDBService
    {
        public DbHelper _helper;
        public DBservice(DbHelper helper)
        {
            _helper = helper;
        }
        public DbUserModel FetchUser(string userName)
        {
            string sql = "select username,password,categoryId from BTSUser Where userName=@userName and IsActive = 1";
            var user = _helper.FetchUser(userName, sql);
            return user;

        }

        public DbInvoiceModel GetInvoice(long invoiceId)
        {
            string sql = "select InvoiceNo,InvoiceDate,AccountDate,DeliveryDate,DueDate,CurrencyCode,ExRate,CreditTerms,TotalAmt,TotalLocalAmt,CustomerName from BTSInvoiceAR Where InvoiceId = @invoiceId";
            var invoice = _helper.GetInvoiceDetails(invoiceId,sql);
            return invoice;
        }

        public void UpdateApprovedStatusForApprover(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsApproved = 1,IsApprovalPending = 0, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdateCheckedStatusForChecker(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsChecked = 1,IsCheckedPending = 0, CheckedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdatePendingStatusForApprover(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsApproved = 0,IsApprovalPending = 1, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdatePendingStatusForChecker(long invoiceId,string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsCheckedPending = 1,IsChecked = 0, CheckedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b',sql);
        }

        public void UpdateRejectedStatusForApprover(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 0,IsApproved = 0,IsApprovalPending = 0, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdateRejectedStatusForChecker(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 0,IsCheckedPending = 0,IsChecked = 0,CheckedBy = @username,EditBy=@username,EditDate = @currentDate Where Innvoiceid = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b',sql);
        }
    }
}
