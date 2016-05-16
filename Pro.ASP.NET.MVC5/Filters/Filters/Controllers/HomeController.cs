using Filters.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Filters.Infrastructure;

namespace Filters.Controllers
{
    public class HomeController : Controller
    {
        //[CustomAuth(false)]
        //[Authorize(Users = "admin")]
        public string Index()
        {
            return "This is the Index Action on the Home controller";
        }

        [GoogleAuth]
        [Authorize(Users = "bob@google.com")]
        public string List()
        {
            return "This is the List action on the Home controller";
        }

        [RangeException]
        public string RangeTest(int id)
        {
            if (id > 100)
            {
                return string.Format($"The id value is : {id}");
            }
            else
            {
                throw new ArgumentOutOfRangeException("id", id, "");
            }
        }

        [ProfileAction]
        [ProfileResult]
        public string FilterTest()
        {
            return "This is the FilterTest Action";
        }
    }
}