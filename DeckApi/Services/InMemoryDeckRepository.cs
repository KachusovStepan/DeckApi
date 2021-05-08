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
        private ShuffleCards<Card> shuffle;

        /// <summary>
        /// Creates InMemoryDeckRepository with specified shuffle algorithm 
        /// </summary>
        /// <param name="shuffleType">shuffle algorithm: "Simple" or "Manual"</param>
        /// <exception cref="ArgumentException">Throws an exception if the algorithm is not recognized</exception>
        public InMemoryDeckRepository(string shuffleType = "Simple")
        {
            ShuffleType = shuffleType;
            switch (shuffleType)
            {
                case "Simple":
                    shuffle = Shuffler.SimpleShuffle;
                    break;
                case "Manual":
                    shuffle = Shuffler.ManualShuffle;
                    break;
                default:
                    throw new ArgumentException($"Unrecognized Shuffle Type: {shuffleType}");
            }

            deckDict = new ConcurrentDictionary<string, Deck>();
        }

        /// <summary>
        /// Creates new deck with specified name
        /// </summary>
        /// <param name="name">New deck name</param>
        /// <returns>True if was successfully created</returns>
        public async Task<bool> CreateNewDeck(string name)
        {
            return deckDict.TryAdd(name, new Deck());
        }

        /// <summary>
        /// Deletes existing deck with specified name
        /// </summary>
        /// <param name="name">Deck name to be deleted</param>
        /// <returns>True if was successfully deleted</returns>
        public async Task<bool> DeleteDeck(string name)
        {
            return deckDict.TryRemove(name, out Deck removedDeck);
        }

        /// <summary>
        /// Gets names of existing decks (not ordered)
        /// </summary>
        /// <returns>List of existing deck names</returns>
        public async Task<List<string>> GetDeckNames()
        {
            return deckDict.Keys.ToList();
        }

        /// <summary>
        /// Shuffles deck with specified name
        /// </summary>
        /// <param name="name">The name of the deck shuffling</param>
        /// <returns>List of existing deck shuffled</returns>
        public async Task<bool> ShuffleDeck(string name)
        {
            if (deckDict.TryGetValue(name, out Deck deck))
            {
                deck.Shuffle(shuffle);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets deck by its name
        /// </summary>
        /// <param name="name">Query deck name</param>
        /// <returns>Queried deck</returns>
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