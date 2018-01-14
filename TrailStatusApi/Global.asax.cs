using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TrailStatusApi.Business;

namespace TrailStatusApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<PageRetriever>().As<IPageRetriever>().SingleInstance();
            builder.RegisterType<StatusParser>().As<IStatusParser>().SingleInstance();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "OPTIONS")
            {
                if (Request.Headers["Origin"] != null)
                    Response.AddHeader("Access-Control-Allow-Origin", Request.Headers["Origin"]);
                Response.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                Response.AddHeader("Access-Control-Allow-Credentials", "true");
                Response.End();
            }
        }
    }
}
