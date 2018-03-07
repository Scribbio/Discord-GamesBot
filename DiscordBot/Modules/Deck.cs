using DiscordBot.Modules.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    class Deck
    {
        public class Card
        {
            public Suits Suit;
            public Faces Face;
            public int Value;

            public override string ToString()
            {
                return $"{Face} of {Suit}s";
            }
        }

        private List<Card> Cards;

        public Deck()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            Cards = new List<Card>();

            for (int CardSuit = 0; CardSuit < 4; CardSuit++)
            {
                for (int CardValue = 0; CardValue < 13; CardValue++)
                {
                    Cards.Add(new Card() { Suit = (Suits)CardSuit, Face = (Faces)CardValue });

                    if (CardValue <= 8)
                        Cards[Cards.Count - 1].Value = CardValue + 1;
                    else
                        Cards[Cards.Count - 1].Value = 10;
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            Random random = new Random();
            for (int cardCount = Cards.Count - 1; cardCount >= 0; cardCount--)
            {
                int movingIndex = random.Next(cardCount);
                Card card = Cards[movingIndex];
                Cards[movingIndex] = Cards[cardCount];
                Cards[cardCount] = card;
            }
        }
        public Card DrawACard()
        {
            if (Cards.Count <= 0)
            {
                this.Initialize();
                this.Shuffle();
            }

            Card cardToReturn = Cards[Cards.Count - 1];
            Cards.RemoveAt(Cards.Count - 1);
            return cardToReturn;
        }

        public int GetAmountOfRemainingCrads()
        {
            return Cards.Count;
        }

        public void PrintDeck()
        {
            int currentCard = 1;
            foreach (Card card in Cards)
            {
                Console.WriteLine("Card {0}: {1} of {2}. Value: {3}", currentCard, card.Face, card.Suit, card.Value);
                currentCard++;
            }
        }
    }
}