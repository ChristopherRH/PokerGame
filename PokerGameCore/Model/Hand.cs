
using System.Collections.Generic;

namespace PokerGameCore
{
    /// <summary>
    /// The player's hand
    /// </summary>
    public class Hand
    {
        /// <summary>
        /// Hand default constructor
        /// </summary>
        public Hand()
        {
            Cards = new List<Card>();
        }
                
        /// <summary>
        /// The list of cards in the hand
        /// </summary>
        public IList<Card> Cards { get; set; }
    }
}
