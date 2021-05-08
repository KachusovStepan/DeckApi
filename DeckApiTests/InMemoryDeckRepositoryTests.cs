using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckApi.Services;
using Xunit;

namespace DeckApiTests
{
    public class InMemoryDeckRepositoryTests
    {
        [Fact]
        public async Task CanCreateDeckWithUniqueName()
        {
            var repo = new InMemoryDeckRepository();
            for (int i = 0; i < 1000; i++)
            {
                var created = await repo.CreateNewDeck(i.ToString());
                Assert.True(created);
            }
        }
        
        [Fact]
        public async Task NotAllowsToCreateDeckWithSameName()
        {
            var repo = new InMemoryDeckRepository();
            var deckName = "NewDeck";
            
            var created = await repo.CreateNewDeck(deckName);
            Assert.True(created);
            
            var duplicate = await repo.CreateNewDeck(deckName);
            Assert.False(duplicate);
        }

        [Fact]
        public async Task CanDeleteDeckAndCreateNewWithThisName()
        {
            var deckName = "NewDeck";
            var repo = new InMemoryDeckRepository();
            
            var created = await repo.CreateNewDeck(deckName);
            Assert.True(created);
            
            var deleted = await repo.DeleteDeck(deckName);
            Assert.True(deleted);
            
            var createdAgain = await repo.CreateNewDeck(deckName);
            Assert.True(createdAgain);
        }

        [Fact]
        public async Task GetDeckNamesReturnsAllCreatedDeckNames()
        {
            var deckNames = new List<string>()
            {
                "deck1", "deck2", "deck3", "deck4"
            };
            
            var repo = new InMemoryDeckRepository();

            foreach (var name in deckNames)
            {
                var succ = await repo.CreateNewDeck(name);
            }

            var actualNames = await repo.GetDeckNames();
            Assert.Equal(deckNames.OrderBy(x => x), actualNames.OrderBy(x => x));
        }

        [Fact]
        public async Task ShuffleDeckModifyCardOrder()
        {
            var deckName = "test deck";
            var shuffleCount = 1000;
            var shuffleTypes = new[] { "Simple", "Manual" };
            
            foreach (var shuffleType in shuffleTypes)
            {
                var repo = new InMemoryDeckRepository(shuffleType);
                var created = await repo.CreateNewDeck(deckName);
                Assert.True(created);

                var initDeck = await repo.GetDeck(deckName);
                var lastCards = initDeck.Cards.ToList();
                
                var equalCount = 0;
                for (int i = 0; i < shuffleCount; i++)
                {
                    var shuffled = await repo.ShuffleDeck(deckName);
                    Assert.True(shuffled);

                    var deck = await repo.GetDeck(deckName);
                    var cardsAfterShuffle = deck.Cards;

                    if (lastCards.SequenceEqual(cardsAfterShuffle))
                    {
                        equalCount++;
                    }

                    lastCards = cardsAfterShuffle.ToList();
                }
                
                Assert.True((double)equalCount / shuffleCount < 0.1);
            }
        }

        [Fact]
        public async Task GetDeckReturnsSameDeck()
        {
            var deckName = "NewDeck";
            
            var repo = new InMemoryDeckRepository();
            
            var created = await repo.CreateNewDeck(deckName);
            Assert.True(created);

            var firstGet = await repo.GetDeck(deckName);
            var secondGet = await repo.GetDeck(deckName);
            
            Assert.Equal(firstGet, secondGet);
        }
    }
}