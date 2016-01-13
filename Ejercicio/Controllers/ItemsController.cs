using System.Collections.Generic;
using System.Web.Http;
using Ejercicio.Models;
using Ejercicio.Services;
using Ejercicio.Repositories;
using MongoDB.Driver;

namespace Ejercicio.Controllers
{
	[RoutePrefix("items")]
	public class ItemsController : ApiController
    {

        private readonly MercadolibreItemsClient itemsClient;
        private readonly RepoNotes repoNotes;

        public ItemsController(MercadolibreItemsClient itemsClient, RepoNotes repoNotes)
	    {
		    this.itemsClient = itemsClient;
            this.repoNotes = repoNotes;
        }

        [Route("{id}")]
        public Item GetById(string id)
        {
            var item = itemsClient.GetById(id);
            repoNotes.setNote(item);
            return item;

        }

        [Route("{id}"), HttpPut]
        public async void PutNote(string itemID, string aNote)
        {
            repoNotes.saveNote(itemID,  aNote);
        }

        [Route("search")]
        public IEnumerable<Item> GetSearch(string query = null)
        {
            IEnumerable<Item> items = itemsClient.Search(query);
            foreach (var item in items)
            {
                repoNotes.setNote(item);
            }
            return items;
        }
        
        [Route("search")]
        public IEnumerable<Item> GetSearchWithnotes(string query = null)
        {
            var itemsWithNotes = new List<Item>();
            var notes = repoNotes.getNotes(query);
            Item item;
            notes.ForEachAsync(note =>
            {
                item = GetById(note.GetValue("itemID").AsString);
                itemsWithNotes.Add(item);
             }
             );
            return itemsWithNotes;
        }

    }
}

