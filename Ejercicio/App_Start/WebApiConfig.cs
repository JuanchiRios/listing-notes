﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Ejercicio
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
