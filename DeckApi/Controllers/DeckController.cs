using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using DeckApi.Logging;
using DeckApi.Models;

namespace DeckApi.Controllers
{
    [Route("api")]
    public class DeckController : Controller
    {
        private IDeckRepository repository;
        
        public DeckController(IDeckRepository repository)
        {
            this.repository = repository;
        }
        
        [HttpGet]
        [Route("test/{name}")]
        public IEnumerable<string> Test(string name)
        {
            return new string[] {name};
        }
        
        [HttpPost]
        [Route("decks/{name}")]
        public HttpResponseMessage CreateDeck(string name)
        {
            var succ = repository.CreateNewDeck(name);
            var statusCode = succ ? HttpStatusCode.OK : HttpStatusCode.NotAcceptable;
            return new HttpResponseMessage(statusCode);
        }
        
        [HttpDelete]
        [Route("decks/{name}")]
        public HttpResponseMessage DeleteDeck(string name)
        {
            var succ = repository.DeleteDeck(name);
            var statusCode = succ ? HttpStatusCode.OK : HttpStatusCode.NotAcceptable;
            return new HttpResponseMessage(statusCode);
        }
        
        [HttpGet]
        [Route("decks/{name}/shuffle")]
        public HttpResponseMessage ShuffleDeck(string name)
        {
            var succ = repository.ShuffleDeck(name);
            var statusCode = succ ? HttpStatusCode.OK : HttpStatusCode.NotAcceptable;
            return new HttpResponseMessage(statusCode);
        }
        
        [HttpGet]
        [Route("decks")]
        public List<string> GetDeckNames()
        {
            var deckNames = repository.GetDeckNames();
            return deckNames;
        }
        
        [HttpGet]
        [Route("decks/{name}")]
        [Produces("application/json")]
        public JsonResult GetDeckNames(string name)
        {
            var deck = repository.GetDeck(name);
            if (deck is null)
            {
                return Json(new {});
            }
            
            var objs = deck.Cards.Select(card => new 
                {
                    Suit = card.Suit.ToString(),
                    Rank =  card.Rank.ToString()
                })
                .ToList();
            var result = new
            {
                Name = name,
                Cards = objs
            };
            return Json(result);
        }
    }
}