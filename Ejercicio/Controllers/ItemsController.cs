using System.Collections.Generic;
using System.Web.Http;
using Ejercicio.Models;
using Ejercicio.Services;

namespace Ejercicio.Controllers
{
	[RoutePrefix("items")]
	public class ItemsController : ApiController
    {
	    private readonly MercadolibreItemsClient itemsClient;
	    public ItemsController(MercadolibreItemsClient itemsClient)
	    {
		    this.itemsClient = itemsClient;
	    }

		[Route("search"), HttpGet]
		public IEnumerable<Item> GetSearch(string query)
        {
            return itemsClient.Search(query);
        }
    }
}
