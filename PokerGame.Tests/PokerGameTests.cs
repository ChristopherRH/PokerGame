using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerGameCore;
using PokerGameCore.Service;

namespace PokerGame.Tests
{
    [TestClass]
    public class PokerGameTests
    {
       
        /// <summary>
        /// Test creating a deck with 52 cards in it
        /// </summary>
        [TestMethod]
        public void TestCreateDeck()
        {
            var deck = new Deck();
            var suits = deck.Cards.Select(x => x.CardSuit).Distinct();
            var numbers = deck.Cards.Select(x => x.CardNumber).Distinct();

            // 52 cards, 4 suits, 13 numbers
            Assert.AreEqual(52, deck.Cards.Count);
            Assert.AreEqual(4, suits.Count());
            Assert.AreEqual(13, numbers.Count());
        }

        /// <summary>
        /// Tests drawing a card from the deck
        /// </summary>
        [TestMethod]
        public void TestDrawCard()
        {
            var player = new Player
            {
                PlayerName = "TestPlayer"
            };

            var deck = new Deck();

            DeckService.DrawCard(player, deck);

            // 1 card in player hand, and 51 cards in deck
            Assert.AreEqual(player.PlayerHand.Cards.Count, 1);
            Assert.AreEqual(deck.Cards.Count, 51);
        }

        /// <summary>
        /// Test the hand result
        /// </summary>
        [TestMethod] 
        public void TestHandResult()
        {
            var hand = new Hand();

            hand.Cards = BuildRoyalFlush();
            var result = HandResult.DetermineHandResult(hand);

            Assert.AreEqual(HandResult.HandResults.RoyalFlush, result.Item1);

            hand.Cards = BuildTwoPair();
            result = HandResult.DetermineHandResult(hand);

            Assert.AreEqual(HandResult.HandResults.TwoPair, result.Item1);

            hand.Cards = BuildHighCard();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.HighCard, result.Item1);
            Assert.AreEqual(PokerGameCore.Model.Numbers.CardNumbers.Ace, result.Item2);
        }

        /// <summary>
        /// Compare 2 hands for a winner
        /// </summary>
        [TestMethod]
        public void TestDetermineWinner()
        {
            var player1 = new Player
            {
                PlayerName = "Test1"
            };
            player1.PlayerHand = new Hand
            {
                Cards = BuildRoyalFlush()
            };

            var player2 = new Player
            {
                PlayerName = "Test2"
            };
            player2.PlayerHand = new Hand
            {
                Cards = BuildHighCard()
            };

            var winner = DeckService.DetermineWinner(player1, player2);

            Assert.AreEqual(player1, winner);
        }


        #region HandBuilders

        /// <summary>
        /// Builds a royal flush hand
        /// </summary>
        /// <returns></returns>
        private IList<Card> BuildRoyalFlush()
        {
            var list = new List<Card>();
            var card1 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Ten
            };
            var card2 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Jack
            };
            var card3 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Queen
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.King
            };
            var card5 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Ace
            };

            list.Add(card1);
            list.Add(card2);
            list.Add(card3);
            list.Add(card4);
            list.Add(card5);

            return list;
        }

        /// <summary>
        /// Build 2 pair hand
        /// </summary>
        /// <returns></returns>
        public IList<Card> BuildTwoPair()
        {
            var list = new List<Card>();
            var card1 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
            };
            var card2 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Clubs,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
            };
            var card3 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Seven
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Clubs,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Seven
            };
            var card5 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Ace
            };

            list.Add(card1);
            list.Add(card2);
            list.Add(card3);
            list.Add(card4);
            list.Add(card5);

            return list;
        }

        /// <summary>
        /// Build hand with only a high card
        /// </summary>
        /// <returns></returns>
        public IList<Card> BuildHighCard()
        {
            var list = new List<Card>();
            var card1 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Two
            };
            var card2 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Clubs,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Four
            };
            var card3 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Seven
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Clubs,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Nine
            };
            var card5 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Ace
            };

            list.Add(card1);
            list.Add(card2);
            list.Add(card3);
            list.Add(card4);
            list.Add(card5);

            return list;
        }

        #endregion
    }
}
