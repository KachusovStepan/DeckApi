using System.Collections.Generic;
using System.Threading.Tasks;
using DeckApi.Controllers;
using DeckApi.Logging;
using DeckApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace DeckApiTests
{
    public class DeckControllerTests
    {
        private DeckController CreateController()
        {
            var repo = new InMemoryDeckRepository();
            var mock = new Mock<ILoggerManager>();
            var controller = new DeckController(repo, mock.Object);
            return controller;
        }
        
        [Fact]
        public async Task CreatesDeckWithUniqueName()
        {
            var controller = CreateController();

            var deckName = "NewDeck";
            var createResp = await controller.CreateDeck(deckName);
            Assert.IsType<JsonResult>(createResp);
            
            string actual = JsonConvert.SerializeObject(createResp.Value);
            string expected = JsonConvert.SerializeObject(new {
                Message = $"Deck {deckName} was successfully created",
                Url = $"/api/decks/{deckName}"
            });
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task DeletesDeck()
        {
            var controller = CreateController();

            var deckName = "NewDeck";
            var createResp = await controller.CreateDeck(deckName);
            
            var createDuplicateResp = await controller.DeleteDeck(deckName);
            
            Assert.IsType<JsonResult>(createDuplicateResp);
            string actual = JsonConvert.SerializeObject(createDuplicateResp.Value);
            string expected = JsonConvert.SerializeObject(new {
                Message = $"Deck {deckName} was successfully deleted"
            });
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task GetDeckNamesReturnsAllNames()
        {
            var controller = CreateController();

            var deckNames = new List<string>()
            {
                "deck1", "deck2", "deck3", "deck4"
            };
            
            foreach (var name in deckNames)
            {
                var createResp = await controller.CreateDeck(name);
            }

            var namesResp = await controller.GetDeckNames();
            Assert.IsType<JsonResult>(namesResp);
            
            string actual = JsonConvert.SerializeObject(namesResp.Value);
            foreach (var name in deckNames)
            {
                Assert.Contains(name, actual);
            }
        }
        
        [Fact]
        public async Task GetDeckReturnsRightDeck()
        {
            var controller = CreateController();

            var deckName = "NewDeck";
            var resp = await controller.CreateDeck(deckName);

            var data = await controller.GetDeck(deckName);
            Assert.IsType<JsonResult>(data);
            string actual = JsonConvert.SerializeObject(data.Value);
            Assert.Contains(deckName, actual);
        }
    }
}