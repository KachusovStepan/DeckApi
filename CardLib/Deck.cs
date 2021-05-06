using System;

namespace CardLib
{
    public class Deck
    {
        public readonly Card[] Cards = new Card[52];

        public bool Shuffled { get; private set; }
        public Deck()
        {
            FillDeck();
            Shuffled = false;
        }

        public void Shuffle(ShuffleCards shuffle)
        {
            shuffle(Cards);
            Shuffled = true;
        }

        public void FillDeck()
        {
            var suitCount = Enum.GetNames(typeof(CardSuit)).Length;
            var rankCount = Enum.GetNames(typeof(CardRank)).Length;

            for (int s = 1; s < suitCount; s++)
            {
                for (int r = 1; r < rankCount; r++)
                    Cards[(s - 1) * (rankCount - 1) + r - 1] = new Card((CardSuit)s, (CardRank)r);
            }
        }
    }
}