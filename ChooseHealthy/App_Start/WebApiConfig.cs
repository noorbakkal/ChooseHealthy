using ChooseHealthy.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace ChooseHealthy
{
  
    public static class WebApiConfig
    {
        public static IntentsList IntentHandlers { get; set; }
        public static void Register(HttpConfiguration config)   
        {
            IntentHandlers = new IntentsList
            {
                {"ProviderIntent", (cm) => Handlers.ProviderIntent.Process(cm) },
                {"AMAZON.YesIntent", (cm) => Handlers.YesIntent.Process(cm) },
                {"AMAZON.NoIntent", (cm) => Handlers.NoIntent.Process(cm) },
                {"AMAZON.CancelIntent", (cm) => Handlers.CancelIntent.Process(cm) },
                {"AMAZON.StopIntent", (cm) => Handlers.CancelIntent.Process(cm) },
                {"AMAZON.HelpIntent", (cm) => Handlers.HelpIntent.Process(cm) },
                {"DefaultWelcomeIntent", (cm) => Handlers.WelcomeIntent.Process(cm) }
            };
            // Json settings
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
