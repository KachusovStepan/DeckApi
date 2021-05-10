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
            return Json(new
            {
                status = "success",
                message = "",
                data = deckNames
            });
        }
        
        [HttpGet]
        [Route("decks/{name}")]
        public async Task<JsonResult> GetDeck(string name)
        {
            logger.LogDebug($"Get Deck {name}");
            
            var deck = await  repository.GetDeck(name);
            if (deck != null)
            {
                return Json(new
                {
                    status = "success",
                    message = "",
                    data = new {
                            name = name,
                            cards = deck.Cards
                        }
                });
            }
            
            if (HttpContext != null)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            return Json(new
            {
                status = "failed",
                message = "Deck Not Fount"
            });
        }
        
        [HttpGet]
        [Route("decks/{name}/shuffle")]
        public async Task<JsonResult> ShuffleDeck(string name)
        {
            logger.LogDebug($"Shuffle Deck {name}");
            
            var succ = await repository.ShuffleDeck(name);
            if (succ)
            {
                return Json(new
                {
                    status = "success",
                    message = "Deck was successfully shuffled"
                });
            }
            
            if (HttpContext != null)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            return Json(new
            {
                status = "failed",
                message = "Deck Not Fount"
            });
        }
        
        [HttpPost]
        [Route("decks/{name}")]
        public async Task<JsonResult> CreateDeck(string name)
        {
            logger.LogDebug($"Create Deck {name}");
            
            var succ = await repository.CreateNewDeck(name);
            if (succ)
            {
                return Json(new
                {
                    status = "success",
                    message = "Deck was successfully created",
                    data = new {
                        url = $"/api/decks/{name}"
                    }
                });
            }
            
            if (HttpContext != null)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            return Json(new
            {
                status = "failed",
                message = "Can't create deck with this name"
            });
        }
        
        [HttpDelete]
        [Route("decks/{name}")]
        public async Task<JsonResult> DeleteDeck(string name)
        {
            logger.LogDebug($"Delete Deck {name}");
            
            var succ = await repository.DeleteDeck(name);
            if (succ)
            {
                return Json(new
                {
                    status = "success",
                    message = "Deck was successfully deleted"
                });
            }
            
            if (HttpContext != null)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            return Json(new
            {
                status = "failed",
                message = "Can't delete deck with this name"
            });
        }
    }
}