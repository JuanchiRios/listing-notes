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
        private static IMongoDatabase database  = client.GetDatabase("itemsNotesDB");
        public IMongoCollection<BsonDocument> collection { get; set; } = database.GetCollection<BsonDocument>("itemsNotesCollection");

        private readonly MercadolibreItemsClient itemsClient;
	    public ItemsController(MercadolibreItemsClient itemsClient)
	    {
		    this.itemsClient = itemsClient;
	    }

        [Route("{id}")]
        public async System.Threading.Tasks.Task<Item> GetById(string id)
        {
            var item = itemsClient.GetById(id);
            item.setNote(collection);
            return item;

        }

        [Route("{id}"), HttpPut]
        public async void PutNote(string itemID, string aNote)
        {
            string json = "{" + "{ '_ID' : "+ itemID + "}," + aNote + "}";
            BsonDocument document = BsonDocument.Parse(json);
            await collection.InsertOneAsync(document);
        }

        [Route("search")]
        public IEnumerable<Item> GetSearch(string query = null)
        {
            IEnumerable<Item> items = itemsClient.Search(query);
            foreach (var item in items)
            {
                item.setNote(collection);
            }
            return items;
        }

        [Route("search")]
        public IEnumerable<Item> GetSearchWithnotes(string query = null)
        {
            var itemsWithNotes = new List<Item>();
            IEnumerable<Item> items = this.GetSearch();
            foreach (var item in items)
            {
                if (item.Note != null) itemsWithNotes.Add(item);
            }
            return itemsWithNotes;
        }

    }
}

