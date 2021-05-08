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
        private string deckName = "NewDeck";
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
        public async Task ErrorOnCreateDeckWithExistingName()
        {
            var controller = CreateController();
        
            var creationResult = await controller.CreateDeck(deckName);
            
            var creationDuplicate = await controller.CreateDeck(deckName);
            
            Assert.IsType<JsonResult>(creationDuplicate);
            string actual = JsonConvert.SerializeObject(creationDuplicate.Value);
            string expected = JsonConvert.SerializeObject(new {
                Message = $"Can't create deck with name {deckName}"
            });
            
            Assert.Equal(actual, expected);
        }

        [Fact]
        public async Task DeletesDeck()
        {
            var controller = CreateController();

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
        public async Task ErrorOnDeletesDeckWhenNotExists()
        {
            var controller = CreateController();

            var creationDuplicate = await controller.DeleteDeck(deckName);
            Assert.IsType<JsonResult>(creationDuplicate);
            
            string actual = JsonConvert.SerializeObject(creationDuplicate.Value);
            string expected = JsonConvert.SerializeObject(new {
                Message = $"Can't delete deck with name {deckName}"
            });
            Assert.Equal(actual, expected);
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

            var resp = await controller.CreateDeck(deckName);

            var data = await controller.GetDeck(deckName);
            Assert.IsType<JsonResult>(data);
            string actual = JsonConvert.SerializeObject(data.Value);
            Assert.Contains(deckName, actual);
        }
        
        [Fact]
        public async Task GetDeckReturnsErrorIfNotExist()
        {
            var controller = CreateController();

            var data = await controller.GetDeck(deckName);
            Assert.IsType<JsonResult>(data);
            string actual = JsonConvert.SerializeObject(data.Value);
            string expected = JsonConvert.SerializeObject(new {
                Message = "Deck Not Fount" 
            });
            Assert.Equal(actual, expected);
        }
        
        [Fact]
        public async Task AfterShuffleDeckGetDeckReturnsDifferent()
        {
            var controller = CreateController();

            var resp = await controller.CreateDeck(deckName);
            var beforeShuffleResp = await controller.GetDeck(deckName);
            string actualBeforeShuffle = JsonConvert.SerializeObject(beforeShuffleResp.Value);
            
            var shuffleResp = await controller.ShuffleDeck(deckName);
            
            var afterShuffleResp = await controller.GetDeck(deckName);
            string actualAfterShuffle = JsonConvert.SerializeObject(afterShuffleResp.Value);
            
            
            Assert.NotEqual(actualBeforeShuffle, actualAfterShuffle);  
        }
        
        [Fact]
        public async Task ShuffleNotExistingDeckReturnsError()
        {
            var controller = CreateController();

            var shuffleResp = await controller.ShuffleDeck(deckName);
            
            string shuffleMessage = JsonConvert.SerializeObject(shuffleResp.Value);
            var expectedJson = JsonConvert.SerializeObject(new { Message = "Deck Not Fount" });
            Assert.Equal(expectedJson, shuffleMessage);  
        }
    }
}