using System;
using System.Linq;
using System.Linq.Expressions;
using Ejercicio.Models;
using Ejercicio.Services;
using FluentAssertions;
using Moq;
using RestSharp;
using Xunit;
using MongoDB.Bson;
using MongoDB.Driver;
using Ejercicio.Controllers;
using Ejercicio.Repositories;

namespace Ejercicio.Tests.Services
{
    public class ItemNoteTest
    {
        private IMongoCollection<BsonDocument> collection;
        private ItemsController itemController;
        private RepoNotes repoNotes;
        private string expectedNote;
        public ItemNoteTest()
        {
            
            var client = new MongoClient();
            var database = client.GetDatabase("itemsNotesDBTest");
            this.collection = database.GetCollection<BsonDocument>("itemsNotesColTest");
            var filter = new BsonDocument();
            collection.DeleteManyAsync(filter);
            this.repoNotes = new RepoNotes(collection);

            var restClientMock = new Mock<MercadolibreRestClient> { CallBase = true };
            var response = new RestResponse<MeliSearchingData<Item>> { Data = new MeliSearchingData<Item>() };
            restClientMock.Setup(x => x.Execute<MeliSearchingData<Item>>(It.IsAny<IRestRequest>())).Returns(response);
            var mercadolibreItemsClient = new MercadolibreItemsClient(restClientMock.Object);
            this.itemController = new ItemsController(mercadolibreItemsClient, repoNotes);

            this.expectedNote = new BsonDocument
                                {
                                    { "itemID", "42" },
                                    { "notes", "esta publicación me interesa" }
                                }.ToJson();


        }

        [Fact]
        public void It_should_persist_the_note()
        { 
            this.itemController.PutNote("42", "esta publicación me interesa");
            Assert.Equal(this.expectedNote, repoNotes.getNote("42"));
            Assert.Equal(1, collection.Count(new BsonDocument()));
        }

        [Fact]
        public void It_should_set_the_note_persisted_to_the_item()
        {
            var item42 = new Item();
            item42.Id = "42";
            Assert.Equal(null, item42.Note);
            this.itemController.PutNote("42", "esta publicación me interesa");
            this.repoNotes.setNote(item42);
            Assert.Equal(this.expectedNote, item42.Note);
            Assert.Equal(1, collection.Count(new BsonDocument()));
        }

    }
}
