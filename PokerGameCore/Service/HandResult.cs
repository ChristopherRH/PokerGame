﻿
using PokerGameCore.Attributes;
using PokerGameCore.Model;
using System;
using System.Linq;

namespace PokerGameCore.Service
{
    /// <summary>
    /// HandComparer, will compare 2 hands and determine a winner
    /// Hand values were determined by: https://upswingpoker.com/poker-hands-rankings
    /// </summary>
    public class HandResult
    {
        /// <summary>
        /// The possible types of hands
        /// </summary>
        public enum HandResults
        {
            [Name("Royal Flush")]
            RoyalFlush = 10,

            [Name("Straight Flush")]
            StraightFlush = 9,

            [Name("Four of a Kind")]
            FourOfAKind = 8,

            [Name("Full House")]
            FullHouse = 7,

            [Name("Flush")]
            Flush = 6,

            [Name("Straight")]
            Straight = 5,

            [Name("Three of a Kind")]
            ThreeOfAKind = 4,

            [Name("Two Pair")]
            TwoPair = 3,

            [Name("Pair")]
            Pair = 2,

            [Name("High Card")]
            HighCard = 1
        }

        /// <summary>
        /// Returns the value of the hand
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public static Tuple<HandResults, Numbers.CardNumbers> DetermineHandResult(Hand hand)
        {
            var highCard = hand.Cards.Max().CardNumber;

            // look for a RoyalFlush
            if(hand.Cards.Select(x => x.CardSuit).Distinct().Count() == 1)
            {
                // 10 -> Ace == RoyalFlush
                var numbers = hand.Cards.Select(x => x.CardNumber);
                if(numbers.Contains(Numbers.CardNumbers.Ten) &&
                    numbers.Contains(Numbers.CardNumbers.Jack) &&
                    numbers.Contains(Numbers.CardNumbers.Queen) &&
                    numbers.Contains(Numbers.CardNumbers.King) &&
                    numbers.Contains(Numbers.CardNumbers.Ace))
                {
                    return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.RoyalFlush, highCard);
                }

                // Any other set of numbers is a StraightFlush
                var isStraight = true;
                var orderedNumbers = numbers.OrderBy(x => (int)x).ToArray();
                var sequenceStart = (int)orderedNumbers.First();
                for(var i = 0; i<5; i++)
                {
                    if((int)orderedNumbers[i] == sequenceStart)
                    {
                        sequenceStart++;
                        continue;
                    }
                    else
                    {
                        isStraight = false;
                        break;
                    }
                }

                if (isStraight)
                {
                    return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.StraightFlush, highCard);
                }

                // It's still a flush
                return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.Flush, highCard);
            }

            // look for a 4 of a kind
            var grouping = hand.Cards.Select(x => x.CardNumber).GroupBy(x => x);
            if (grouping.Any(y => y.Count() == 4))
            {
                return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.FourOfAKind, highCard);
            }

            // look for a full house
            if(grouping.Any(y => y.Count() == 3) && grouping.Any(y => y.Count() == 2))
            {
                return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.FullHouse, highCard);
            }

            // 3 of a kind
            if (grouping.Any(y => y.Count() == 3))
            {
                return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.ThreeOfAKind, highCard);
            }

            // look for 2 pair
            if(grouping.Count(y => y.Count() == 2) == 2)
            {
                return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.TwoPair, highCard);
            }

            // look for a pair
            if(grouping.Any(y => y.Count() == 2))
            {
                return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.Pair, highCard);
            }

            // all we have is a high card
            return new Tuple<HandResults, Numbers.CardNumbers>(HandResults.HighCard, highCard);
        }
    }
}
