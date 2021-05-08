using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardLib;

namespace DeckApi.Services
{
    public class InMemoryDeckRepository : IDeckRepository
    {
        private ConcurrentDictionary<string, Deck> deckDict;
        public readonly string ShuffleType;
        private ShuffleCards shuffle;

        public InMemoryDeckRepository(string shuffleType = "Simple")
        {
            ShuffleType = shuffleType;
            switch (shuffleType)
            {
                case "Simple":
                    shuffle = Shuffler.SimpleShuffle;
                    break;
                case "Manual":
                    shuffle = Shuffler.SimpleShuffle;
                    break;
                default:
                    throw new ArgumentException($"Unrecognized Shuffle Type: {shuffleType}");
            }

            deckDict = new ConcurrentDictionary<string, Deck>();
        }

        public async Task<bool> CreateNewDeck(string name)
        {
            return deckDict.TryAdd(name, new Deck());
        }

        public async Task<bool> DeleteDeck(string name)
        {
            return deckDict.TryRemove(name, out Deck removedDeck);
        }

        public async Task<List<string>> GetDeckNames()
        {
            return deckDict.Keys.ToList();
        }

        public async Task<bool> ShuffleDeck(string name)
        {
            if (deckDict.TryGetValue(name, out Deck deck))
            {
                deck.Shuffle(shuffle);
                return true;
            }

            return false;
        }

        public async Task<Deck> GetDeck(string name)
        {
            if (deckDict.TryGetValue(name, out Deck deck))
            {
                return deck;
            }

            return null;
        }
    }
}