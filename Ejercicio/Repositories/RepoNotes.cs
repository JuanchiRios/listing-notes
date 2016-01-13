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

        public  void setNote(Item item)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("itemID", item.Id);
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            item.Note = collection.Find(filter).Project(projection).First().ToJson();//if it doesn't exist return null
        }

        public string getNote(string itemId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("itemID", itemId);
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            return collection.Find(filter).Project(projection).First().ToJson();//if it doesn't exist return null
        }

        public void saveNote(string itemID, string aNote)
        {
            var document = new BsonDocument
                                {
                                    { "itemID", itemID },
                                    { "notes", aNote }
                                };
             collection.InsertOneAsync(document);
        }

        public IFindFluent<BsonDocument, BsonDocument>  getNotes(string query)
        {
            var filter = Builders<BsonDocument>.Filter.Text(query);
            return collection.Find(filter);
        }
    }
}