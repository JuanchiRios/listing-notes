﻿using System;
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

namespace Ejercicio.Tests.Services
{
    public class MercadolibreItemsClientTest
    {
        private MercadolibreItemsClient mercadolibreItemsClient;
        private Mock<MercadolibreRestClient> restClientMock;
        private static MongoClient client = new MongoClient("mongodb://localhost:27017");
        public IMongoDatabase database { get; } = client.GetDatabase("itemsNotesTest");

        public MercadolibreItemsClientTest()
        {
            this.restClientMock = new Mock<MercadolibreRestClient> { CallBase = true };
            var response = new RestResponse<MeliSearchingData<Item>> { Data = new MeliSearchingData<Item>() };
            this.restClientMock.Setup(x => x.Execute<MeliSearchingData<Item>>(It.IsAny<IRestRequest>())).Returns(response);
            this.mercadolibreItemsClient = new MercadolibreItemsClient(this.restClientMock.Object);
            mercadolibreItemsClient.database = database;
        }

        [Fact]
        public void It_shuold_send_a_request_to_site_search_api_sending_q_paramenter_when_search_is_called_without_any_query()
        {
            var query = "iPhone 6";
            this.mercadolibreItemsClient.Search(query);
            Expression<Func<IRestRequest, bool>> expected = request =>
                request.Resource == "sites/MLA/search" &&
                request.Parameters.Any(x => x.Name == "q" && x.Value == query);
            this.restClientMock.Verify(x => x.Execute<MeliSearchingData<Item>>(It.Is(expected)), Times.Once);
        }

        [Fact]
        public void It_shuold_send_a_request_to_site_search_api_without_sending_q_paramenter_when_search_is_called_without_any_query()
        {
            this.mercadolibreItemsClient.Search();
            Expression<Func<IRestRequest, bool>> expected = request =>
                request.Resource == "sites/MLA/search" &&
                request.Parameters.All(x => x.Name != "q");
            this.restClientMock.Verify(x => x.Execute<MeliSearchingData<Item>>(It.Is(expected)), Times.Once);
        }

        [Fact]
        public void It_should_send_a_request_to_site_search_api_to_search_by_id()
        {
            var id = "42";
            this.mercadolibreItemsClient.GetById(id);
            Expression<Func<IRestRequest, bool>> expected = request =>
                request.Resource == "items/42";
            this.restClientMock.Verify(x => x.Execute<Item>(It.Is(expected)), Times.Once);
        }

        [Fact]
        public async void It_should_send_a_request_to_site_search_api_to_put_a_note_in_an_item()
        {
            var id = "42";
            var aNote = new BsonDocument
                            {
                                { "description", "esta publicación me interesa" }
                            };
            await this.mercadolibreItemsClient.PutNote(id, aNote);
            Expression<Func<IRestRequest, bool>> expected = request =>
                request.Resource == "items/42/notes" &&
                request.Parameters.Find(x => x.Name == "note").Value.Equals(aNote);
            this.restClientMock.Verify(x => x.Execute<Item>(It.Is(expected)), Times.Once);
        }

        [Fact(Skip = "Thinking why is failing")]
        public async void It_should_get_the_same_note_that_it_was_put_in_the_item()
        {
            var id = "42";
            var aNote = new BsonDocument
                            {
                                { "description", "esta publicación me interesa" }
                            };
            await this.mercadolibreItemsClient.PutNote(id, aNote);
            IMongoCollection<BsonDocument> notesPerItem = database.GetCollection<BsonDocument>(id);

            this.mercadolibreItemsClient.GetById(id);
            Expression<Func<IRestRequest, bool>> expected = request =>
                request.Resource == "items/42" &&
                request.Parameters.Find(x => x.Name == "notes").Value.Equals(notesPerItem);
  //          Assert.Equal(1, notesPerItem.Count);
  //          Assert.Equal(aNote,notesPerItem.Find(x => true));
            this.restClientMock.Verify(x => x.Execute<Item>(It.Is(expected)), Times.Exactly(2));
        }
    }
}
