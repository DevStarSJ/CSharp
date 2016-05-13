using Filters.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Filters.Controllers
{
    public class HomeController : Controller
    {
        //[CustomAuth(false)]
        [Authorize(Users = "admin")]
        public string Index()
        {
            return "This is the Index Action on the Home controller";
        }
    }
}