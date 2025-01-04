using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace assessment_platform_developer {

    public class WebApiConfig {

        public static void Register(HttpConfiguration config) {
            // Default Web API route
            config.MapHttpAttributeRoutes();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings {
                    ContractResolver = new DefaultContractResolver {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore
                };

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}