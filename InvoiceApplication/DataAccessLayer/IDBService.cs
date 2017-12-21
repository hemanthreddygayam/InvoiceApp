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
         void UpdatePendingStatusForChecker(long invoiceId,string username, string remarks);
         void UpdateCheckedStatusForChecker(long invoiceId, string username, string remarks);
         void UpdateRejectedStatusForChecker(long invoiceId, string username, string remarks);
         void UpdatePendingStatusForApprover(long invoiceId, string username, string remarks);
         void UpdateApprovedStatusForApprover(long invoiceId, string username, string remarks);
         void UpdateRejectedStatusForApprover(long invoiceId, string username,string remarks);
         List<DbInvoiceModel> GetAllInvoices(DateTime startDate, DateTime endDate, string status,string userRole);
         string getColorToUpdate(int status);
         string GetEmailForUser(string username);
         List<DbVesselDocument> FetchDocumentsForInvoice(long id);
         DbVesselDocument FetchFile(int id);
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

        public List<DbVesselDocument> FetchDocumentsForInvoice(long id)
        {
            string sql = "select FileId,InvoiceId,FilePath,FileName from VesselDocuments Where InvoiceId = @invoiceId";
            var results = _helper.FetchDocumentDetails(sql, id);
            return results;
        }

        public DbVesselDocument FetchFile(int id)
        {
            string sql = "select FileId,InvoiceId,FilePath,FileName from VesselDocuments Where FileId = @fileId";
            var result = _helper.FetchFileDetails(sql, id);
            return result;
        }

        public DbUserModel FetchUser(string userName)
        {
            string sql = "select username,password,categoryId from BTSUser Where userName=@userName and IsActive = 1";
            var user = _helper.FetchUser(userName, sql);
            return user;

        }

        public List<DbInvoiceModel> GetAllInvoices(DateTime startDate, DateTime endDate, string status,string userRole)
        {
            string sql = @"SELECT InvoiceId,InvoiceNo,DueDate,DeliveryDate,CurrencyCode,ExRate,VesselName,TotalAmt,TotalLocalAmt,CustomerName,InvoiceStatus,AccountDate,
                           InvStatus = CASE
                           WHEN( InvoiceStatus = 3 or InvoiceStatus = 6 or InvoiceStatus = 2 ) THEN  1
                           WHEN( InvoiceStatus = 5) THEN  2
                           WHEN( InvoiceStatus = 4 or InvoiceStatus = 7 ) THEN 3
                           ELSE 4 END from BTSInvoiceAR 
                           Where 1 = 1 ";

            if (status == "pending")
            {
                if (userRole == "CheckingAuthority")
                {
                    sql += " AND ( InvoiceStatus = 3 or InvoiceStatus = 1 )";
                }
                else if (userRole == "ApproverAuthority")
                {
                    sql += " AND ( InvoiceStatus = 2 or InvoiceStatus = 6 )";
                }
            }
            else if (status == "approved")
            {
                sql += " AND (InvoiceStatus = 5 ) ";

            }
            else if (status == "rejected")
            {
                if (userRole == "ApproverAuthority")
                {
                    sql += " AND ( InvoiceStatus = 7 ) ";
                }
                else if (userRole == "CheckingAuthority")
                {
                    sql += " AND ( InvoiceStatus = 4 ) ";
                }
            }
            else if (status == "checked")
            {
                sql += " AND ( InvoiceStatus = 2 ) ";
            }
            else { }
            if(startDate != DateTime.MinValue)
            {
                sql += " AND ( AccountDate >= @StartDate ) ";
            }
            if(endDate != DateTime.MinValue)
            {
                endDate = endDate.AddDays(1);
                sql += " AND ( AccountDate < @EndDate ) ";
            }
            sql += " ORDER BY InvStatus ASC, AccountDate DESC ";

            var results = _helper.GetInvoices(sql, startDate, endDate);
           
            return results;
        }

        public string GetEmailForUser(string username)
        {
            string sql = "select Email from UserEmail Where CategoryId= 2 and IsActive = 1";
            var email = _helper.GetEmail(username, sql);
            return email;
        }

        public DbInvoiceModel GetInvoice(long invoiceId)
        {
            string sql = "select InvoiceId,InvoiceNo,AccountDate,DeliveryDate,CurrencyCode,ExRate,TotalAmt,TotalLocalAmt,CustomerName, VesselName, InvoiceStatus, remarks from BTSInvoiceAR Where InvoiceId = @invoiceId";
            var invoice = _helper.GetInvoiceDetails(invoiceId,sql);
            return invoice;
        }

        public void UpdateApprovedStatusForApprover(long invoiceId, string username,string remarks)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsApproved = 1,IsApprovalPending = 0, IsApprovalReverted =0, InvoiceStatus = 5,ApprovedBy = @username,EditBy=@username,EditDate = @currentDate, Remarks = @remarks Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql, remarks);
        }

        public void UpdateCheckedStatusForChecker(long invoiceId, string username,string remarks)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsChecked = 1,IsCheckedPending = 0, InvoiceStatus = 2,CheckedBy = @username,EditBy=@username,EditDate = @currentDate, Remarks = @remarks Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql, remarks); 	

        }

        public void UpdatePendingStatusForApprover(long invoiceId, string username,string remarks)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsApproved = 0,IsApprovalPending = 1, IsApprovalReverted = 0, InvoiceStatus = 6, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate, Remarks = @remarks Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql,remarks);
        }

        public void UpdatePendingStatusForChecker(long invoiceId,string username,string remarks)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 1,IsCheckedPending = 1,IsChecked = 0, InvoiceStatus = 3, CheckedBy = @username,EditBy=@username,EditDate = @currentDate, Remarks = @remarks Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b',sql, remarks);
        }

        public void UpdateRejectedStatusForApprover(long invoiceId, string username,string remarks)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 0,IsApproved = 0,IsApprovalPending = 0, IsApprovalReverted = 1, InvoiceStatus = 7, ApprovedBy = @username,EditBy=@username,EditDate = @currentDate, Remarks = @remarks Where InvoiceId = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b', sql, remarks);
        }

        public void UpdateRejectedStatusForChecker(long invoiceId, string username,string remarks)
        {
            string sql = "update BTSInvoiceAR set IsWaitingApproval = 0,IsCheckedPending = 0,IsChecked = 0,IsCheckingReverted = 1, InvoiceStatus = 4, CheckedBy = @username,EditBy=@username,EditDate = @currentDate, Remarks = @remarks Where Invoiceid = @invoiceId";
            _helper.UpdateStatusForChecker(invoiceId, 'b',sql, remarks);
        }

        public string getColorToUpdate(int status)
        {
            string sql = "select color from AdmInvoiceApprovalStatus Where StatusId= @statusId";
           return _helper.GetColor(sql,status);
        }

        
    }
}
