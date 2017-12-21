using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.DataAccessLayer
{
    public class DbHelper
    {
        private IConfiguration _configuration;
        public string DbString;

        
        public DbHelper()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            this._configuration = builder.Build();
            DbString = _configuration.GetConnectionString("EMSDatabase");
        }
        

        public DbUserModel FetchUser(string userName,string sql)
        {
            using (var connection = new SqlConnection(DbString))
            {
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("userName", userName));
               
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                   
                    while (reader.Read())
                    {
                        DbUserModel model = new DbUserModel();
                        model.UserName = (string)reader["username"];
                        model.Password = (string)reader["password"];
                        model.CategoryId = (Int64)reader["categoryId"];
                        return model;
                    }
                }
            }
            return null;
        }

        public string GetEmail(string username, string sql)
        {
            string email = null;
            using (var connection = new SqlConnection(DbString))
            {
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("username", username));

                connection.Open();
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        email = (string)reader["Email"];

                    }
                }
            }
            return email;
        }

        public int UpdateStatusForChecker(long invoiceId, char username,string sql,string remarks)
        {
            int status = 0;
            try
            {
                using (var connection = new SqlConnection(DbString))
                {
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add(new SqlParameter("invoiceId", invoiceId));
                    command.Parameters.Add(new SqlParameter("currentDate", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("username", username));
                    command.Parameters.Add(new SqlParameter("remarks", remarks));


                    connection.Open();
                    status = command.ExecuteNonQuery();
                    
                }
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception("Update not sucessfull");

            }

        }

        public List<DbVesselDocument> FetchDocumentDetails(string sql, long id)
        {
            List<DbVesselDocument> documents = new List<DbVesselDocument>();
            using (var connection = new SqlConnection(DbString))
            {
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("invoiceId", id));

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DbVesselDocument doc = new DbVesselDocument();
                            doc.FileId = (int)reader["FileId"];
                            doc.InvoiceId = (long)reader["InvoiceId"];
                            doc.FilePath = (string)reader["FilePath"];
                            doc.FileName = (string)reader["FileName"];
                            documents.Add(doc);
                        }
                    }
                }
            }
            return documents;
        }

        public  DbVesselDocument FetchFileDetails(string sql, int id)
        {
            DbVesselDocument doc = null;
            using (var connection = new SqlConnection(DbString))
            {
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("fileId", id));

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            doc = new DbVesselDocument();
                            doc.FileId = (int)reader["FileId"];
                            doc.InvoiceId = (long)reader["InvoiceId"];
                            doc.FilePath = (string)reader["FilePath"];
                            doc.FileName = (string)reader["FileName"];
                            
                        }
                    }
                }
            }
            return doc;
        }

        public List<DbInvoiceModel> GetInvoices(string sql, DateTime startDate, DateTime endDate)
        {
            List<DbInvoiceModel> invoices = new List<DbInvoiceModel>();
            using (var connection = new SqlConnection(DbString))
            {
                var command = new SqlCommand(sql, connection);
                if (startDate != DateTime.MinValue)
                {
                    DateTime start = (DateTime)startDate.Date;
                    command.Parameters.Add(new SqlParameter("@StartDate", startDate));

                }
                if (endDate != DateTime.MinValue)
                {
                    DateTime end = (DateTime)endDate.Date;
                    command.Parameters.Add(new SqlParameter("@EndDate", endDate));

                }

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DbInvoiceModel model = new DbInvoiceModel();
                            model.InvoiceId = (long)reader["InvoiceId"];
                            model.InvoiceNo = (string)reader["InvoiceNo"];
                            model.CurrencyCode = (string)reader["CurrencyCode"];
                            model.CustomerName = (string)reader["CustomerName"];
                            model.AccountDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("AccountDate"));
                            model.DeliveryDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("DeliveryDate"));
                            model.DueDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("DueDate"));
                            model.TotalAmt = (decimal)reader["TotalAmt"];
                            model.TotalLocalAmt = (decimal)reader["TotalLocalAmt"];
                            model.ExRate = (int)reader["ExRate"];
                            model.VesselName = (string)reader["VesselName"];
                            invoices.Add(model);
                        }
                    }

                    
                }
            }
            return invoices;
        }

        public  DbInvoiceModel GetInvoiceDetails(long invoiceId, string sql)
        {
            using (var connection = new SqlConnection(DbString))
            {
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("invoiceId", invoiceId));

                connection.Open();
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        DbInvoiceModel model = new DbInvoiceModel();
                        model.InvoiceId = (long)reader["InvoiceId"];
                        model.InvoiceNo = (string)reader["InvoiceNo"];
                        model.CurrencyCode = (string)reader["CurrencyCode"];
                        model.CustomerName = (string)reader["CustomerName"];
                        model.AccountDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("AccountDate"));
                        model.DeliveryDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("DeliveryDate"));
                        model.TotalAmt = (decimal)reader["TotalAmt"];
                        model.TotalLocalAmt = (decimal)reader["TotalLocalAmt"];
                        model.ExRate = (int)reader["ExRate"];
                        model.VesselName = (string)reader["VesselName"];
                        model.InvoiceStatus = (int)reader["InvoiceStatus"];
                        model.Remarks = reader["remarks"] != DBNull.Value ? (string)reader["remarks"] : string.Empty;

                        return model;
                    }
                }
            }
            return null;
        }

        public string GetColor(string sql, int status)
        {
            string color = string.Empty;
            using (var connection = new SqlConnection(DbString))
            {
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("statusId", status));

                connection.Open();
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        color = (string)reader["color"];
                    }
                }
            }
            return color;
        }
    }
}
