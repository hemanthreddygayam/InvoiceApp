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
         List<DbInvoiceModel> FetchAllPendingInvoices(DateTime fromDate, DateTime toDate, int numberOfTranscations);
         List<DbInvoiceModel> FetchAllApprovedInvoices(DateTime fromDate, DateTime toDate, int numberOfTranscations);
            List<DbInvoiceModel> FetchAllRevertedInvoices(DateTime fromDate, DateTime toDate, int numberOfTranscations);


        string GetEmailForUser(string username);


    }

    public class DBservice : IDBService
    {
        public DbHelper _helper;
        public DBservice(DbHelper helper)
        {
            _helper = helper;
        }

        public List<DbInvoiceModel> FetchAllApprovedInvoices(DateTime fromDate, DateTime toDate, int numberOfTranscations)
        {
            string sql = "select top "+numberOfTranscations.ToString()+" InvoiceNo,InvoiceDate,AccountDate,DeliveryDate,DueDate,CurrencyCode,ExRate,CreditTerms,TotalAmt,TotalLocalAmt,CustomerName from BTSInvoiceAR Where AccountDate >= @fromDate and AccountDate < @toDate and IsApproved = 1";
            return null;
        }

        public List<DbInvoiceModel> FetchAllPendingInvoices(DateTime fromDate, DateTime toDate, int numberOfTranscations)
        {
            string sql = "select top " + numberOfTranscations.ToString() + " InvoiceNo,InvoiceDate,AccountDate,DeliveryDate,DueDate,CurrencyCode,ExRate,CreditTerms,TotalAmt,TotalLocalAmt,CustomerName from BTSInvoiceAR Where AccountDate >= @fromDate and AccountDate < @toDate and ( IsCheckedPending = 1 or IsApprovalPending = 1 or IsChecked = 1 or IsWaitingForApproval = 0)";
            return null;
        }

        public List<DbInvoiceModel> FetchAllRevertedInvoices(DateTime fromDate, DateTime toDate, int numberOfTranscations)
        {
            string sql = "select top " + numberOfTranscations.ToString() + " InvoiceNo,InvoiceDate,AccountDate,DeliveryDate,DueDate,CurrencyCode,ExRate,CreditTerms,TotalAmt,TotalLocalAmt,CustomerName from BTSInvoiceAR Where AccountDate >= @fromDate and AccountDate < @toDate and ( IsCheckingReverted = 1 or IsApprovalReverted = 1 )";
            return null;
        }

        public DbUserModel FetchUser(string userName)
        {
            string sql = "select username,password,categoryId from BTSUser Where userName=@userName and IsActive = 1";
            var user = _helper.FetchUser(userName, sql);
            return user;

        }

        public string GetEmailForUser(string username)
        {
            string sql = "select Email from UserEmail Where username= @username and IsActive = 1";
            var email = _helper.GetEmail(username, sql);
            return email;
        }

        public DbInvoiceModel GetInvoice(long invoiceId)
        {
            string sql = "select InvoiceNo,InvoiceDate,AccountDate,DeliveryDate,DueDate,CurrencyCode,ExRate,CreditTerms,TotalAmt,TotalLocalAmt,CustomerName from BTSInvoiceAR Where InvoiceId = @invoiceId";
            var invoice = _helper.GetInvoiceDetails(invoiceId,sql);
            return invoice;
        }

        public void UpdateApprovedStatusForApprover(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsApproved = 1,IsApprovalPending = 0, IsApprovalReverted =0, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdateCheckedStatusForChecker(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsChecked = 1,IsCheckedPending = 0, CheckedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdatePendingStatusForApprover(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsApproved = 0,IsApprovalPending = 1, IsApprovalReverted = 0, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdatePendingStatusForChecker(long invoiceId,string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsCheckedPending = 1,IsChecked = 0, CheckedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b',sql);
        }

        public void UpdateRejectedStatusForApprover(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 0,IsApproved = 0,IsApprovalPending = 0, IsApprovalReverted = 1, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql);
        }

        public void UpdateRejectedStatusForChecker(long invoiceId, string username)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 0,IsCheckedPending = 0,IsChecked = 0,IsCheckingReverted = 1, CheckedBy = @username,EditBy=@username,EditDate = @currentDate Where Innvoiceid = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b',sql);
        }
    }
}
