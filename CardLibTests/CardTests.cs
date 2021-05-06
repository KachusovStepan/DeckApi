using System;
using CardLib;
using NUnit.Framework;

namespace CardLibTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CardsRemainState()
        {
            foreach (var suit in (CardSuit[])Enum.GetValues(typeof(CardSuit)))
            {
                foreach (var rank in (CardRank[])Enum.GetValues(typeof(CardRank)))
                {
                    var card = new Card(suit, rank);
                    Assert.AreEqual(card.Suit, suit);
                    Assert.AreEqual(card.Rank, rank);
                }
            }
        }

        [Test]
        public void InitDeckIsSorted()
        {
            var deck = new Deck();
            
            Assert.IsFalse(deck.Shuffled);
            Assert.IsTrue(CardsAreOrdered(deck.Cards));
        }

        private bool CardsAreOrdered(Card[] cards)
        {
            var suitCount = Enum.GetNames(typeof(CardSuit)).Length;
            var rankCount = Enum.GetNames(typeof(CardRank)).Length;

            for (int s = 1; s < suitCount; s++)
            {
                for (int r = 1; r < rankCount; r++)
                {
                    if (cards[(s - 1) * (rankCount - 1) + r - 1].Suit != (CardSuit)s
                            || cards[(s - 1) * (rankCount - 1) + r - 1].Rank != (CardRank)r)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        [Test]
        public void SimpleShuffleModifyOrder()
        {
            var n = 1000;
            var deck = new Deck();
            deck.Shuffle(Shuffler.SimpleShuffle);
            Assert.IsTrue(deck.Shuffled);
            var orderedCout = 0;
            for (int i = 0; i < n; i++)
            {
                if (CardsAreOrdered(deck.Cards))
                {
                    orderedCout++;
                }
                
                deck.Shuffle(Shuffler.SimpleShuffle);
            }
            
            Assert.IsTrue((double)orderedCout / n < 0.01);
        }
        
        [Test]
        public void ManualShuffleModifyOrder()
        {
            var n = 1000;
            var deck = new Deck();
            deck.Shuffle(Shuffler.ManualShuffle);
            Assert.IsTrue(deck.Shuffled);
            var orderedCout = 0;
            for (int i = 0; i < n; i++)
            {
                if (CardsAreOrdered(deck.Cards))
                {
                    orderedCout++;
                }
                
                deck.Shuffle(Shuffler.ManualShuffle);
            }
            
            Assert.IsTrue((double)orderedCout / n < 0.1);
        }
    }
}