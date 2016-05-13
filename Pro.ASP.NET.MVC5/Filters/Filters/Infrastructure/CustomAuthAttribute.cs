using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Filters.Infrastructure
{
    class CustomAuthAttribute : AuthorizeAttribute
    {
        private bool localAllowed;

        public CustomAuthAttribute(bool allowed)
        {
            localAllowed = allowed;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsLocal)
            {
                return localAllowed;
            }
            else
            {
                return true;
            }
        }
    }
}
