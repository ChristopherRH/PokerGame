using System.Collections.Generic;
using System.Diagnostics;
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
        /// Nondeterministic test
        /// Test RNG until a straight appears
        /// </summary>
        [TestMethod]
        public void TestStraight()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var deck = new Deck();
            DeckService.ShuffleDeck(deck);
            var player1 = new Player
            {
                PlayerName = "Test1"
            };
            var player2 = new Player
            {
                PlayerName = "Test2"
            };
            var player3 = new Player
            {
                PlayerName = "Test3"
            };
            var player4 = new Player
            {
                PlayerName = "Test4"
            };
            var straightFound = false;
            var straightCounter = 1;
            var handString = "";
            while (!straightFound)
            {
                deck.Cards = DeckService.CreateCardDeck();
                DeckService.ShuffleDeck(deck);

                player1.PlayerHand.Cards.Clear();
                player2.PlayerHand.Cards.Clear();
                player3.PlayerHand.Cards.Clear();
                player4.PlayerHand.Cards.Clear();
                for (var i = 0; i < 5; i++)
                {
                    DeckService.DrawCard(player1, deck);
                    DeckService.DrawCard(player2, deck);
                    DeckService.DrawCard(player3, deck);
                    DeckService.DrawCard(player4, deck);
                }

                CheckResult(player1, ref straightFound, ref handString);
                if (!straightFound)
                {
                    CheckResult(player2, ref straightFound, ref handString);
                }
                if (!straightFound)
                {
                    CheckResult(player3, ref straightFound, ref handString);
                }
                if (!straightFound)
                {
                    CheckResult(player4, ref straightFound, ref handString);
                }

                straightCounter++;
                if(stopwatch.ElapsedMilliseconds > 60000) // 1 minute
                {
                    Assert.Fail($"Failed to generate a straight within {straightCounter} iterations");
                }
            }

            // we got to a successful iteration, so pass
            Assert.IsTrue(true, handString);

        }

        private static void CheckResult(Player player1, ref bool straightFound, ref string handString)
        {
            var result = HandResult.DetermineHandResult(player1.PlayerHand);
            if (result.Item1 == HandResult.HandResults.Straight || result.Item1 == HandResult.HandResults.StraightFlush)
            {
                foreach (var card in player1.PlayerHand.Cards)
                {
                    handString += card.CardSuitString + card.CardNumberString + ",";
                }
                straightFound = true;
            }
        }

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

            hand.Cards = BuildStraightFlush();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.StraightFlush, result.Item1);

            hand.Cards = Build4OfAkind();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.FourOfAKind, result.Item1);

            hand.Cards = BuildFullHouse();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.FullHouse, result.Item1);

            hand.Cards = BuildFlush();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.Flush, result.Item1);

            hand.Cards = BuildStraight();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.Straight, result.Item1);

            hand.Cards = Build3OfAkind();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.ThreeOfAKind, result.Item1);

            hand.Cards = BuildTwoPair();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.TwoPair, result.Item1);

            hand.Cards = BuildPair();
            result = HandResult.DetermineHandResult(hand);
            Assert.AreEqual(HandResult.HandResults.Pair, result.Item1);

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
        /// Build a straight flush
        /// </summary>
        /// <returns></returns>
        public IList<Card> BuildStraightFlush()
        {
            var list = new List<Card>();
            var card1 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
            };
            var card2 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Six
            };
            var card3 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Seven
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Eight
            };
            var card5 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Nine
            };

            list.Add(card1);
            list.Add(card2);
            list.Add(card3);
            list.Add(card4);
            list.Add(card5);

            return list;
        }

        /// <summary>
        /// Build a full house hand
        /// </summary>
        /// <returns></returns>
        public IList<Card> BuildFullHouse()
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
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Spades,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Clubs,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Seven
            };
            var card5 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Seven
            };

            list.Add(card1);
            list.Add(card2);
            list.Add(card3);
            list.Add(card4);
            list.Add(card5);

            return list;
        }

        /// <summary>
        /// Build a flush hand
        /// </summary>
        /// <returns></returns>
        public IList<Card> BuildFlush()
        {
            var list = new List<Card>();
            var card1 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
            };
            var card2 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Three
            };
            var card3 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Nine
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
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
        /// Build a straight
        /// </summary>
        /// <returns></returns>
        public IList<Card> BuildStraight()
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
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Six
            };
            var card3 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Spades,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Seven
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Clubs,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Eight
            };
            var card5 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Hearts,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Nine
            };

            list.Add(card1);
            list.Add(card2);
            list.Add(card3);
            list.Add(card4);
            list.Add(card5);

            return list;
        }

        /// <summary>
        /// Build 4 of a kind
        /// </summary>
        /// <returns></returns>
        public IList<Card> Build4OfAkind()
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
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Spades,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
            };
            var card4 = new Card
            {
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Diamonds,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
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
        /// Build 3 of a kind
        /// </summary>
        /// <returns></returns>
        public IList<Card> Build3OfAkind()
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
                CardSuit = PokerGameCore.Model.Suits.CardSuits.Spades,
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Five
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
        /// Build pair hand
        /// </summary>
        /// <returns></returns>
        public IList<Card> BuildPair()
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
                CardNumber = PokerGameCore.Model.Numbers.CardNumbers.Eight
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
