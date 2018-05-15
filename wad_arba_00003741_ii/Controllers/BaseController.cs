using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace wad_arba_00003741_ii.Controllers
{
    public class BaseController : Controller
    {
        private static readonly List<string> _cultures = new List<string> {
            "en-US",  // first culture is the DEFAULT
            "ru"
        };

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = RouteData.Values["culture"] as string;

            // Attempt to read the culture cookie from Request
            if (cultureName == null)
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            if (cultureName == null)
                cultureName = _cultures[0];

            cultureName = cultureName.ToLowerInvariant();
            // Validate culture name
            cultureName = _cultures.Any(c => c.ToLowerInvariant().Contains(cultureName)) ? cultureName : _cultures[0].ToLowerInvariant(); // This is safe

            if (RouteData.Values["culture"] as string != cultureName)
            {
                // Force a valid culture in the URL
                RouteData.Values["culture"] = cultureName; // lower case too
                // Redirect user
                Response.RedirectToRoute(RouteData.Values);
            }
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            return base.BeginExecuteCore(callback, state);
        }

    }
}