﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AssessmentPlatformDeveloper.App_Start {

    public class WebApiConfig {

        public static void Register(HttpConfiguration config) {
            // Default Web API route
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {
                    id = RouteParameter.Optional
                }
            );
        }
    }
}