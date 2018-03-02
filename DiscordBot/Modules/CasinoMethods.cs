using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Modules
{
    #region 'Card Games'
    public class Deck
    {
        public int NumberOfDecks { get; set; }
        public List<Card> ShuffledDeck { get; set; }

        public static Deck GetNewDeck(int decks)
        {
            //Method to get a set number of decks depending on game
            var Returndeck = new Deck();
            Returndeck.NumberOfDecks = decks;
            for (int i = 0; i < 4 * decks; i++)
            {
                string suit = "";
                switch (i)
                {
                    case 0:
                        suit = "Hearts";
                        break;
                    case 1:
                        suit = "Spades";
                        break;
                    case 2:
                        suit = "Diamonds";
                        break;
                    case 3:
                        suit = "Clubs";
                        break;

                    default:
                        break;
                }
                for (int i2 = 1; i2 < 14; i2++)
                {
                    var card = new Card
                    {
                        Value = i2,
                        Suit = suit
                    };
                    Returndeck.ShuffledDeck.Add(card);
                }

            }
            return ShuffleDeck(Returndeck);
        }
        public static Deck ShuffleDeck(Deck toShuffle)
        {
            var ShuffledDeck = new Deck
            {
                NumberOfDecks = toShuffle.NumberOfDecks,
                ShuffledDeck = new List<Card>()
            };
            Random xt = new Random();
            for (int i = 0; i < 52 * toShuffle.NumberOfDecks; i++)
            {
                int MoveThisOne = xt.Next(0, toShuffle.ShuffledDeck.Count);
                ShuffledDeck.ShuffledDeck.Add(toShuffle.ShuffledDeck[MoveThisOne]);
                toShuffle.ShuffledDeck.RemoveAt(MoveThisOne);
            }
            return ShuffledDeck;
        }
    }
    public class Card
    {
        public int Value { get; set; }
        public string Suit { get; set; }
        public string Name { get; set; }

        //Constructor to build a friendly name
        public Card()
        {
            string face = Value.ToString();
            switch (Value)
            {
                case 1:
                    face = "Ace";
                    break;
                case 11:
                    face = "Jack";
                    break;
                case 12:
                    face = "Queen";
                    break;
                case 13:
                    face = "King";
                    break;

                default:
                    break;


            }
            Name = face + " of " + Suit;
        }
    }
    #endregion
    #region Player
    public class Player
    {
        public string Name { get; set; }
        public int Chips { get; set; }
        public bool IsHere { get; set; }
        public DateTime LastUpdate { get; set; }
    }
    #endregion
    #region BlackJack
    public class BlackJack
    {
        public Deck deckOfCards { get; set; }
        public List<Player> players { get; set; }

        public BlackJack()
        {
            deckOfCards = Deck.GetNewDeck(5);
            players = new List<Player>();
        }

    }
    #endregion
}
