using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcBlogg
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }
    }
}
