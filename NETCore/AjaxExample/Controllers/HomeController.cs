using AjaxExample.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public ActionResult Index(string selectedRole = "All")
        {
            return View((object)selectedRole);
        }

        //[HttpPost]
        //public ActionResult Index(string selectedRole)
        //{
        //    if (selectedRole == null || selectedRole == "All")
        //    {
        //        return View(personData);
        //    }
        //    else
        //    {
        //        Role seleted = (Role)Enum.Parse(typeof(Role), selectedRole);
        //        return View(personData.Where(p => p.Role == seleted));
        //    }
        //}

        public PartialViewResult GetPeopleData(string selectedRole = "All")
        {
            IEnumerable<Person> data = personData;
            if (selectedRole != "All")
            {
                Role seleted = (Role)Enum.Parse(typeof(Role), selectedRole);
                data = personData.Where(p => p.Role == seleted);
            }
            return PartialView(data);
        }
    }
}