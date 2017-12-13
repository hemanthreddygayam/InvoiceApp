using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using InvoiceApplication.DataAccessLayer;

namespace InvoiceApplication.Controllers
{
    public class InvoicesController : Controller
    {
        
        private string _userAgent;
        public DbHelper _helper;

        public InvoicesController(IHttpContextAccessor httpContext)
        {
            _helper = new DbHelper();
            _userAgent = httpContext.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        [Authorize]
        public IActionResult Index(long id = 1)
        {

            IDBService service = new DBservice(_helper);
            var Roles = User.FindFirst(ClaimTypes.Role).Value;

            var documents = service.FetchDocumentsForInvoice(id);
            if (Roles == "1")
                ViewBag.controller = "CheckingAuthority";
            else
                ViewBag.controller = "ApproverAuthority";

            if(documents.Count() > 0)
            {
                ViewBag.InvoiceId = documents.ElementAt(0).InvoiceId;

            }
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.IsMobile = IsMobileDevice();
            return View(documents);
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetFilePath([FromBody]int id)
        {
            IDBService service = new DBservice(_helper);
            var Roles = User.FindFirst(ClaimTypes.Role).Value;

            var documents = service.FetchFile(id);
            if(Roles == "1")
                ViewBag.controller = "CheckingAuthority";
            else
                ViewBag.controller = "ApproverAuthority";
            return Json(documents);
        }

        private Boolean IsMobileDevice()
        {
            Regex regex = new Regex(@"(iPhone|Android|iPad|Mobile|iPod|webOS|BlackBerry|Windows Phone)");
            Match match = regex.Match(_userAgent);
            return match.Success;
        }

        [HttpGet]
        public ActionResult GetFileMemoryStream(string path)
        {
            return PhysicalFile(path, "application/pdf");
        }
    }
}