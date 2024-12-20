﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace xchedulerDBFinalDraft
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "LoggedIn",
                url: "Account/LoggedIn",
                defaults: new { controller = "Account", action = "LoggedIn" }
            );
            routes.MapRoute(
                 name: "AppointmentChart",
                 url: "AppoinmentDetails/AppointmentChart",
                defaults: new { controller = "AppoinmentDetails", action = "AppointmentChart" }
                );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
