using System;
using System.Collections.Generic;

namespace CardLib
{
    public delegate void ShuffleCards<TCard>(List<TCard> cards);

    public static class Shuffler
    {
        public static void SimpleShuffle<TCard>(List<TCard> cards)
        {
            var random = new Random();
            for (var i = cards.Count - 1; i > 0; i--)
            {
                int n = random.Next(i + 1);
                var tempCard = cards[i];
                cards[i] = cards[n];
                cards[n] = tempCard;
            }
        }

        public static void ManualShuffle<TCard>(List<TCard> cards)
        {
            var random = new Random();
            var iterationCount = 1000;
            var n = cards.Count;
            var tempArr = new TCard[n];
            for (int i = 0; i < iterationCount; i++)
            {
                var thirdOfLen = cards.Count / 3;
                var approximateMiddle = random.Next(thirdOfLen, thirdOfLen * 2);

                // Array.Copy(cards, 0, tempArr, 0, approximateMiddle);
                for (int j = 0; j < approximateMiddle; j++)
                {
                    tempArr[j] = cards[j];
                }
                

                // Array.Copy(cards, approximateMiddle, cards, 0, n - approximateMiddle);
                for (int j = 0; j < n - approximateMiddle; j++)
                {
                    cards[j] = cards[j + approximateMiddle];
                }
                

                // Array.Copy(tempArr, 0, cards, n - approximateMiddle, approximateMiddle);
                for (int j = n - approximateMiddle; j < n; j++)
                {
                    cards[j] = tempArr[j - (n - approximateMiddle)];
                }
            }
        }
    }
}