using System.Collections.Generic;

namespace CardLib
{
    public interface IDeck<TCard> where TCard : ICard
    {
        List<Card> Cards { get; }
        bool Shuffled { get; }
        void Shuffle(ShuffleCards<TCard> shuffle);
        void FillDeck();
    }
}