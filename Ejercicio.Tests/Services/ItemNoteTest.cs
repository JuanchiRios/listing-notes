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

        public ItemNoteTest()
        {
            
            var client = new MongoClient();
            var database = client.GetDatabase("itemsNotesDBTest");
            this.collection = database.GetCollection<BsonDocument>("itemsNotesColTest");
            this.repoNotes = new RepoNotes(collection);
            var restClientMock = new Mock<MercadolibreRestClient> { CallBase = true };
            var response = new RestResponse<MeliSearchingData<Item>> { Data = new MeliSearchingData<Item>() };
            restClientMock.Setup(x => x.Execute<MeliSearchingData<Item>>(It.IsAny<IRestRequest>())).Returns(response);
            var mercadolibreItemsClient = new MercadolibreItemsClient(restClientMock.Object);
            this.itemController = new ItemsController(mercadolibreItemsClient, repoNotes);

        }

        [Fact]
        public void It_shuold_send_a_request_to_site_search_api_sending_q_paramenter_when_search_is_called_without_any_query()
        {
       /*     var query = "iPhone 6";
            this.mercadolibreItemsClient.Search(query);
            Expression<Func<IRestRequest, bool>> expected = request =>
                request.Resource == "sites/MLA/search" &&
                request.Parameters.Any(x => x.Name == "q" && x.Value == query);
            this.restClientMock.Verify(x => x.Execute<MeliSearchingData<Item>>(It.Is(expected)), Times.Once);*/
        }

    }
}
