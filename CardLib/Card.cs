using System;

namespace CardLib
{
    public enum CardSuit
    {
        NonSet,
        Hearts,
        Clubs,
        Diamonds,
        Spades
    }

    public enum CardRank
    {
        NonSet,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public class Card
    {
        public readonly CardSuit Suit;
        public readonly CardRank Rank;
        public Card(CardSuit suit, CardRank rank)
        {
            Suit = suit;
            Rank = rank;
        }

    }
}