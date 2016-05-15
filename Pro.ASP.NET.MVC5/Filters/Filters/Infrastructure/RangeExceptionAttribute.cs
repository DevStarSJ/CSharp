using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Filters.Infrastructure
{
    class RangeExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is ArgumentOutOfRangeException)
            {
                filterContext.Result = new RedirectResult("~/Content/RangeErrorPage.html");
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
