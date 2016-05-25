using AjaxExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AjaxExample.Controllers
{
    public class HomeController : Controller
    {
        private Person[] personData = {
            new Person { FirstName = "Adam", LastName = "Freeman", Role = Role.Admin },
            new Person { FirstName = "Jacqui", LastName = "Griffyth", Role = Role.User },
            new Person { FirstName = "John", LastName = "Smith", Role = Role.User },
            new Person { FirstName = "Anne", LastName = "Jones", Role = Role.Guest }
        };

        public ActionResult Index()
        {
            return View(personData);
        }

        [HttpPost]
        public ActionResult Index(string selectedRole)
        {
            if (selectedRole == null || selectedRole == "All")
            {
                return View(personData);
            }
            else
            {
                Role seleted = (Role)Enum.Parse(typeof(Role), selectedRole);
                return View(personData.Where(p => p.Role == seleted));
            }
        }
    }
}