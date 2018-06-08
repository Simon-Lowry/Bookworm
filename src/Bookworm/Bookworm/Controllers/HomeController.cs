using Bookworm.ViewModels;
using Bookworm.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Models;
using System.Web.Routing;

namespace Bookworm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["userId"] = 10;  
            return View();     
        }
        
    }
}