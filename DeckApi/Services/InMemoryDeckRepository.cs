using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardLib;

namespace DeckApi.Services
{
    public class InMemoryDeckRepository : IDeckRepository
    {
        private Dictionary<string, Deck> deckDict;
        public readonly string ShuffleType;
        private ShuffleCards shuffle;

        public InMemoryDeckRepository(string shuffleType="Simple")
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
            deckDict = new Dictionary<string, Deck>();
        }
        
        public async Task<bool> CreateNewDeck(string name)
        {
            if (deckDict.ContainsKey(name))
            {
                return false;
            }

            var newDeck = new Deck();
            deckDict[name] = newDeck;
            return true;
        }

        public async Task<bool> DeleteDeck(string name)
        {
            if (!deckDict.ContainsKey(name))
            {
                return false;
            }

            deckDict.Remove(name);
            return true;
        }

        public async Task<List<string>> GetDeckNames()
        {
            return deckDict.Keys.ToList();
        }

        public async Task<bool> ShuffleDeck(string name)
        {
            if (!deckDict.ContainsKey(name))
            {
                return false;
            }

            deckDict[name].Shuffle(shuffle);
            return true;
        }

        public async Task<Deck> GetDeck(string name)
        {
            if (!deckDict.ContainsKey(name))
            {
                return null;
            }

            return deckDict[name];
        }
    }
}