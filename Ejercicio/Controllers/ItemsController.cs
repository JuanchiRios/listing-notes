using System.Collections.Generic;
using System.Web.Http;
using Ejercicio.Models;
using Ejercicio.Services;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Ejercicio.Controllers
{
	[RoutePrefix("items")]
	public class ItemsController : ApiController
    {
        private static MongoClient client = new MongoClient();
        public IMongoDatabase database { get; } = client.GetDatabase("itemsNotes");
        private readonly MercadolibreItemsClient itemsClient;
	    public ItemsController(MercadolibreItemsClient itemsClient)
	    {
		    this.itemsClient = itemsClient;
	    }

		[Route("{id}")]
		public Item GetById(string id)
		{
			return itemsClient.GetById(id);
		}

        [Route("id/notes")]
        public Item PutNote(string itemID, BsonDocument aNote)
        {
            return itemsClient.PutNote(itemID, aNote, database);
       //     var notesPerItem = database.GetCollection<BsonDocument>(itemID);
       //     await notesPerItem.InsertOneAsync(aNote);
        }

        [Route("search")]
		public IEnumerable<Item> GetSearch(string query = null)
		{
			return itemsClient.Search(query);
		}
	}
}
