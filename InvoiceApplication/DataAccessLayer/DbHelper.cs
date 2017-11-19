using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication.DataAccessLayer
{
    public class DbHelper
    {
        private IConfiguration _configuration;

        
        public DbHelper()
        { }
        public DbHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public DbUserModel FetchUser(string userName,string sql)
        {
            using (var connection = new SqlConnection("Server=LAPTOP-D8N1NPGG\\MSSQLSERVER1;Database=iCPMS_OMTI_FZ;Integrated Security=True;"))
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
            using (var connection = new SqlConnection("Server=LAPTOP-D8N1NPGG\\MSSQLSERVER1;Database=iCPMS_OMTI_FZ;Integrated Security=True;"))
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

        public int UpdateStatusForChecker(long invoiceId, char username,string sql)
        {
            int status = 0;
            try
            {
                using (var connection = new SqlConnection("Server=LAPTOP-D8N1NPGG\\MSSQLSERVER1;Database=iCPMS_OMTI_FZ;Integrated Security=True;"))
                {
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add(new SqlParameter("invoiceId", invoiceId));
                    command.Parameters.Add(new SqlParameter("currentDate", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("username", username));

                    connection.Open();
                    status = command.ExecuteNonQuery();
                    command.Dispose();
                    return status;
                }

            }
            catch(Exception ex)
            {
                return status;

            }

        }

        public List<DbInvoiceModel> GetInvoices(string sql, DateTime startDate, DateTime endDate)
        {
            List<DbInvoiceModel> invoices = new List<DbInvoiceModel>();
            using (var connection = new SqlConnection("Server=LAPTOP-D8N1NPGG\\MSSQLSERVER1;Database=iCPMS_OMTI_FZ;Integrated Security=True;"))
            {
                var command = new SqlCommand(sql, connection);
                if (startDate != DateTime.MinValue)
                {
                    DateTime start = (DateTime)startDate;
                    command.Parameters.Add(new SqlParameter("@StartDate", startDate));

                }
                if (endDate != DateTime.MinValue)
                {
                    DateTime end = (DateTime)endDate;
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
                            model.DueDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("DueDate"));
                            model.DeliveryDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("DeliveryDate"));
                            model.TotalAmt = (decimal)reader["TotalAmt"];
                            model.TotalLocalAmt = (decimal)reader["TotalLocalAmt"];
                            model.InvoiceDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("InvoiceDate"));
                            model.ExRate = (int)reader["ExRate"];
                            invoices.Add(model);
                        }
                    }

                    
                }
            }
            return invoices;
        }

        public  DbInvoiceModel GetInvoiceDetails(long invoiceId, string sql)
        {
            using (var connection = new SqlConnection("Server=LAPTOP-D8N1NPGG\\MSSQLSERVER1;Database=iCPMS_OMTI_FZ;Integrated Security=True;"))
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
                        model.InvoiceDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("InvoiceDate"));
                        model.AccountDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("AccountDate"));
                        model.DueDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("DueDate"));
                        model.DeliveryDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("DeliveryDate"));
                        model.TotalAmt = (decimal)reader["TotalAmt"];
                        model.TotalLocalAmt = (decimal)reader["TotalLocalAmt"];
                        model.InvoiceDate = reader.GetFieldValue<DateTime>(reader.GetOrdinal("InvoiceDate"));
                        model.ExRate = (int)reader["ExRate"];
                        model.CreditTerms = (string)reader["CreditTerms"];
                        return model;
                    }
                }
            }
            return null;
        }
    }
}
