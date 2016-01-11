using System.Collections.Generic;
using Ejercicio.Models;
using RestSharp;

namespace Ejercicio.Services
{
	public class MercadolibreItemsClient
	{
		private readonly MercadolibreRestClient restClient;

		public MercadolibreItemsClient(MercadolibreRestClient restClient)
		{
			this.restClient = restClient;
		}

		public IEnumerable<Item> Search(string query = null)
		{
			var request = new RestRequest("sites/MLA/search");
			request.Method = Method.GET;
			request.Parameters.Add(new Parameter { Name = "category", Type = ParameterType.QueryString, Value = "MLA1648" });
			if (query != null)
			{
				request.Parameters.Add(new Parameter { Name = "q", Type = ParameterType.QueryString, Value = query });
			}
			var response = this.restClient.Execute<MeliSearchingData<Item>>(request);
			return response.Data.Results;
		}
	}

	public class MeliSearchingData<T>
	{
		public MeliPagingData Paging { get; set; }
		public IEnumerable<T> Results { get; set; }
	}

	public class MeliPagingData
	{
		public int Offset { get; set; }
		public int Limit { get; set; }
		public int Total { get; set; }
	}
}