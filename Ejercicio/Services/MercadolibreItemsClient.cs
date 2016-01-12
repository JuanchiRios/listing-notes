using System.Collections.Generic;
using Ejercicio.Models;
using RestSharp;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ejercicio.Services
{
    public class MercadolibreItemsClient
    {
        private readonly MercadolibreRestClient restClient;
        private static MongoClient client = new MongoClient("mongodb://localhost:27017");
        public IMongoDatabase database { get; set; } = client.GetDatabase("itemsNotes");

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

        public Item GetById(string id)
        {
            var request = new RestRequest("items/" + id);
            request.Method = Method.GET;
            var notesPerItem = database.GetCollection<BsonDocument>(id);

            request.Parameters.Add(new Parameter { Name = "notes", Type = ParameterType.RequestBody, Value = notesPerItem });
            var response = this.restClient.Execute<Item>(request);
            return response.Data;
        }

        public async System.Threading.Tasks.Task<Item> PutNote(string itemID, BsonDocument aNote)
        {
            IMongoCollection<BsonDocument> notesPerItem = database.GetCollection<BsonDocument>(itemID);
            await notesPerItem.InsertOneAsync(aNote);
            var request = new RestRequest("items/" + itemID + "/notes");
            request.Method = Method.PUT;
            request.Parameters.Add(new Parameter { Name = "note", Type = ParameterType.RequestBody, Value = aNote });
            var response = this.restClient.Execute<Item>(request);
            return response.Data;
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