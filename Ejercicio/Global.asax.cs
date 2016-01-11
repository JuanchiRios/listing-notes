using System.Web.Http;
using Ejercicio.Utils;

namespace Ejercicio
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
	        GlobalConfiguration.Configure(WebApiConfig.Register);
	        RegisterFormatters();
        }

	    private static void RegisterFormatters()
	    {
		    var formatters = GlobalConfiguration.Configuration.Formatters;
		    formatters.Remove(formatters.XmlFormatter);
		    formatters.JsonFormatter.SerializerSettings.ContractResolver = new SnakeCasePropertiesContractResolver();
		    formatters.JsonFormatter.UseDataContractJsonSerializer = false;
	    }
    }
}
