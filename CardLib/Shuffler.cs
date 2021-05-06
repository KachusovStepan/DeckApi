using System;

namespace CardLib
{
    public delegate void ShuffleCards(Card[] cards);

    public static class Shuffler
    {
        public static void SimpleShuffle(Card[] cards)
        {
            var random = new Random();
            for (var i = cards.Length - 1; i > 0; i--)
            {
                int n = random.Next(i + 1);
                var tempCard = cards[i];
                cards[i] = cards[n];
                cards[n] = tempCard;
            }
        }

        public static void ManualShuffle(Card[] cards)
        {
            var random = new Random();
            var iterationCount = 1000;
            var n = cards.Length;
            var tempArr = new Card[n];
            for (int i = 0; i < iterationCount; i++)
            {
                var thirdOfLen = cards.Length / 3;
                var approximateMiddle = random.Next(thirdOfLen, thirdOfLen * 2);

                Array.Copy(cards, 0, tempArr, 0, approximateMiddle);

                Array.Copy(cards, approximateMiddle, cards, 0, n - approximateMiddle);

                Array.Copy(tempArr, 0, cards, n - approximateMiddle, approximateMiddle);
            }
        }
    }
}