using System;
using System.Collections.Generic;

namespace CardLib
{
    public class Deck : IDeck<Card>
    {
        public List<Card> Cards { get; }

        public bool Shuffled { get; private set; }
        public Deck()
        {
            Cards = new List<Card>();
            FillDeck();
            Shuffled = false;
        }

        public void Shuffle(ShuffleCards<Card> shuffle)
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
                    Cards.Add(new Card((CardSuit)s, (CardRank)r));
            }
        }
    }
}