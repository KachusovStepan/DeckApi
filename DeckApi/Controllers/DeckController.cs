using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeckApi.Logging;
using DeckApi.Services;

namespace DeckApi.Controllers
{
    [Route("api")]
    public class DeckController : Controller
    {
        private IDeckRepository repository;
        private ILoggerManager logger;
        
        public DeckController(IDeckRepository repository, ILoggerManager logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        [Route("decks")]
        public async Task<JsonResult> GetDeckNames()
        {
            logger.LogDebug("Request handled by GetDeckNames");
            var deckNames = await repository.GetDeckNames();
            return Json(deckNames);
        }
        
        [HttpGet]
        [Route("decks/{name}")]
        public async Task<JsonResult> GetDeck(string name)
        {
            logger.LogDebug($"Get Deck {name}");
            
            var deck = await  repository.GetDeck(name);
            if (deck is null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Deck Not Fount" });
            }
            
            var result = new
            {
                Name = name,
                Cards = deck.Cards
            };
            
            return Json(result);
        }
        
        [HttpGet]
        [Route("decks/{name}/shuffle")]
        public async Task<JsonResult> ShuffleDeck(string name)
        {
            logger.LogDebug($"Shuffle Deck {name}");
            
            var succ = await repository.ShuffleDeck(name);
            if (!succ)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = "Deck Not Fount" });
            }

            return Json(new {Message = $"Deck {name} successfully shuffled"});
        }
        
        [HttpPost]
        [Route("decks/{name}")]
        public async Task<JsonResult> CreateDeck(string name)
        {
            logger.LogDebug($"Create Deck {name}");
            
            var succ = await repository.CreateNewDeck(name);
            if (!succ)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = $"Can't create deck with name {name}" });
            }
            return Json(new
                {
                    Message = $"Deck {name} successfully created",
                    Url = $"/api/decks/{name}"
                });
        }
        
        [HttpDelete]
        [Route("decks/{name}")]
        public async Task<JsonResult> DeleteDeck(string name)
        {
            logger.LogDebug($"Delete Deck {name}");
            
            var succ = await repository.DeleteDeck(name);
            if (!succ)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Message = $"Can't delete deck with name {name}" });
            }
            return Json(new
                {
                    Message = $"Deck {name} successfully deleted"
                });
        }
    }
}