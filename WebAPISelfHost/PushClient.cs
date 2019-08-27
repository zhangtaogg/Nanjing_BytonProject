using System;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using System.Web.Http;

namespace WebApiSelfHost {
    public class PushClient {
        public static void StartPushClient() {
            string baseAddress = "http://localhost:10282/";
            WebApp.Start<Startup>(baseAddress);
        }
    }

    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.UseCors(CorsOptions.AllowAll);
            // Configure Web API for Self-Host
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);

        }
    }
}
