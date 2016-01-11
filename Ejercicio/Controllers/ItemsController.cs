using System.Collections.Generic;
using System.Web.Http;
using Ejercicio.Models;
using Ejercicio.Services;

namespace Ejercicio.Controllers
{
    public class ItemsController : ApiController
    {
	    private readonly MercadolibreItemsClient itemsClient;
	    public ItemsController(MercadolibreItemsClient itemsClient)
	    {
		    this.itemsClient = itemsClient;
	    }

	    // GET: api/Items
        public IEnumerable<Item> Get(string query)
        {
            return itemsClient.Search(query);
        }
    }
}
