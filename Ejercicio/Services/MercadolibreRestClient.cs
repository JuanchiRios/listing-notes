using Ejercicio.Utils;
using RestSharp;

namespace Ejercicio.Services
{
	public class MercadolibreRestClient : RestClient
	{
		public MercadolibreRestClient() : base("https://api.mercadolibre.com")
		{
			this.ClearHandlers();
			this.AddHandler("*", new SnakeCaseJsonConverter());
		}
	}
}