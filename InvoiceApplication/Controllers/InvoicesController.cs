using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceApplication.Controllers
{
    public class InvoicesController : Controller
    {
        private TrackingDbContext _context;

        public InvoicesController(TrackingDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index(int id = 1)
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.Name).Value;
            IEnumerable<VesselDocuments> documents = _context.VesselDocuments.Where(doc => doc.InvoiceId == id);
            return View(documents);
        }
    }
}