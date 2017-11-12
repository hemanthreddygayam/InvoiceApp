using System;
using System.Collections.Generic;

namespace InvoiceApplication.DbModels
{
    public partial class UserEmail
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool? IsActive { get; set; }
        public int? CategoryId { get; set; }
        public string Email { get; set; }
    }
}
