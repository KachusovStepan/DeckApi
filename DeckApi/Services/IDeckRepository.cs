using System.Collections.Generic;
using System.Threading.Tasks;
using CardLib;

namespace DeckApi.Services
{
    public interface IDeckRepository
    {
        Task<bool> CreateNewDeck(string name);
        Task<bool> DeleteDeck(string name);
        Task<List<string>> GetDeckNames();
        Task<bool> ShuffleDeck(string name);
        Task<Deck> GetDeck(string name);
    }
}