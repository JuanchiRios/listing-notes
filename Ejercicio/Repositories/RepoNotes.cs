using Ejercicio.Models;
using Ejercicio.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ejercicio.Repositories
{
    public class RepoNotes
    {
        private IMongoCollection<BsonDocument> collection { get; set; }

        public RepoNotes(IMongoCollection<BsonDocument> collection)
        {
            this.collection = collection;
        }

        public async void setNote(Item item)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_ID", item.Id);
            item.Note = await collection.Find(filter).FirstOrDefaultAsync();//if it doesn't exist return null
        }
        public async void saveNote(string itemID, string aNote)
        {
            string json = "{" + "{ '_ID' : " + itemID + "}," + aNote + "}";
            BsonDocument document = BsonDocument.Parse(json);
            await collection.InsertOneAsync(document);
        }

    }
}