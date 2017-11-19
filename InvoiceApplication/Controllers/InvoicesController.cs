using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using InvoiceApplication.Models;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;


namespace InvoiceApplication.Controllers
{
    public class InvoicesController : Controller
    {
        
        private TrackingDbContext _context;
        private string _userAgent;

        public InvoicesController(TrackingDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _userAgent = httpContext.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        [Authorize]
        public IActionResult Index(int id = 1)
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<VesselDocuments> documents = _context.VesselDocuments.Where(doc => doc.InvoiceId == id);
            ViewBag.IsMobile = IsMobileDevice();
            return View(documents);
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetFilePath([FromBody]int id)
        {
            return Json(_context.VesselDocuments.Find(id));
        }

        private Boolean IsMobileDevice()
        {
            Regex regex = new Regex(@"(iPhone|Android|iPad|Mobile|iPod|webOS|BlackBerry|Windows Phone)");
            Match match = regex.Match(_userAgent);
            return match.Success;
        }
    }
}