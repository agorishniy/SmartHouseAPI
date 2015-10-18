using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ApiOnOff",
                routeTemplate: "api/{controller}/onoff"
            );

            config.Routes.MapHttpRoute(
                name: "ApiParam",
                routeTemplate: "api/{controller}/param"
            );

            //config.Routes.MapHttpRoute(
            //    name: "ApiDel",
            //    routeTemplate: "api/{controller}/del/"
            //);

        }
    }
}
