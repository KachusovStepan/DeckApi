using System.Collections.Generic;
using CardLib;

namespace DeckApi.Services
{
    public interface IDeckRepository
    {
        bool CreateNewDeck(string name);
        bool DeleteDeck(string name);
        List<string> GetDeckNames();
        bool ShuffleDeck(string name);
        Deck GetDeck(string name);
    }
}