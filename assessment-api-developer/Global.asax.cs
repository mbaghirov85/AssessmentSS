using SimpleInjector;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using assessment_platform_developer.Repositories;
using System.Web.UI;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using assessment_platform_developer.Services;
using SimpleInjector.Integration.Web;
using System.Web.Http;
using SimpleInjector.Integration.WebApi;
using assessment_platform_developer.Controllers;
using SimpleInjector.Lifestyles;
using System.Net.Http;
using System.Net.Http.Headers;

namespace assessment_platform_developer {

    public sealed class PageInitializerModule : IHttpModule {

        public static void Initialize() {
            DynamicModuleUtility.RegisterModule(typeof(PageInitializerModule));
        }

        void IHttpModule.Init(HttpApplication app) {
            app.PreRequestHandlerExecute += (sender, e) => {
                var handler = app.Context.CurrentHandler;
                if (handler != null) {
                    string name = handler.GetType().Assembly.FullName;
                    if (!name.StartsWith("System.Web") &&
                        !name.StartsWith("Microsoft")) {
                        Global.InitializeHandler(handler);
                    }
                }
            };
        }

        void IHttpModule.Dispose() {
        }
    }

    public class Global : HttpApplication {
        private static readonly Container container = new Container();

        public static void InitializeHandler(IHttpHandler handler) {
            var handlerType = handler is Page
                ? handler.GetType().BaseType
                : handler.GetType();
            container.GetRegistration(handlerType, true).Registration
                .InitializeInstance(handler);
        }

        private void Application_Start(object sender, EventArgs e) {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure((config) => WebApiConfig.Register(config));
            Bootstrap();
        }

        private static void Bootstrap() {
            // Create a new Simple Injector container.
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Create a single HttpClient instance
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            container.RegisterInstance<HttpClient>(httpClient);

            // Configure the container (register)
            container.Register<ICustomerRepository, CustomerRepository>(Lifestyle.Singleton);
            container.Register<ICustomerService, CustomerService>(Lifestyle.Scoped);
            container.Register<IRestfulCustomerService, RestfulCustomerService>(Lifestyle.Scoped);
            container.Register<CustomersController>(new AsyncScopedLifestyle());

            // Verify the container's configuration.
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
            new SimpleInjectorWebApiDependencyResolver(container);

            HttpContext.Current.Application["DIContainer"] = container;
        }
    }
}