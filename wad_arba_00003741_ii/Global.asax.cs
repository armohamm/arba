using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace wad_arba_00003741_ii
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AcquireRequestState()
        {
            try
            {
                HttpCookie authCookie = Request.Cookies.Get("UserLoginData");
                if(authCookie != null && !string.IsNullOrEmpty(authCookie.Value) && FormsAuthentication.Decrypt(authCookie.Value) != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    var userName = ticket.Name;
                    Session["User"] = userName;
                }
                else
                {
                    Session["User"] = null;
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}
