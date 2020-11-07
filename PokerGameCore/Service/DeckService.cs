
using PokerGameCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static PokerGameCore.Model.Numbers;
using static PokerGameCore.Model.Suits;

namespace PokerGameCore
{
    /// <summary>
    /// Deck service, containing utility methods for Deck
    /// </summary>
    public class DeckService
    {
        /// <summary>
        /// Creates a list of 52 cards, 13 of each suit
        /// </summary>
        /// <returns></returns>
        public static IList<Card> CreateCardDeck()
        {
            var cardList = new List<Card>();
            foreach(var suit in Enum.GetValues(typeof(CardSuits)).Cast<CardSuits>())
            {
                foreach(var number in Enum.GetValues(typeof(CardNumbers)).Cast<CardNumbers>())
                {
                    cardList.Add(new Card
                    {
                        CardSuit = suit,
                        CardNumber = number
                    });
                }
            }

            return cardList;
        }               
        
        /// <summary>
        /// Shuffles and returns the deck
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        public static void ShuffleDeck(Deck deck)
        {
            var rnd = new Random();
            var n = deck.Cards.Count;
            while (n > 1)
            {
                n--;
                var k = rnd.Next(n + 1);
                var card = deck.Cards[k];
                deck.Cards[k] = deck.Cards[n];
                deck.Cards[n] = card;
            }
        }

        /// <summary>
        /// Draw a card from the deck, assign to a player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="deck"></param>
        public static void DrawCard(Player player, Deck deck)
        {
            if (!deck.Cards.Any())
            {
                return;
            }

            var card = deck.Cards.Take(1).First();
            player.PlayerHand.Cards.Add(card);
            deck.Cards.Remove(card);
        }
        
        /// <summary>
        /// Compare the player hands and determine the winner
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns></returns>
        public static Player DetermineWinner(Player player1, Player player2)
        {
            var player1Result = HandResult.DetermineHandResult(player1.PlayerHand);
            var player2Result = HandResult.DetermineHandResult(player2.PlayerHand);
            
            if(player1Result.Item1.CompareTo(player2Result.Item1) > 0)
            {
                return player1;
            }
            // both results are the same, determine by the High card
            else if(player1Result.Item1.CompareTo(player2Result.Item1) == 0)
            {
                if(player1Result.Item2.CompareTo(player2Result.Item2) > 0)
                {
                    return player1;
                }
                else
                {
                    return player2;
                }
            }
            else
            {
                return player2;
            }
        }
    }
}
