
using System.Collections.Generic;

namespace PokerGameCore
{
    /// <summary>
    /// Deck object, contains a list of cards
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// Default constructor, creates a deck of cards
        /// </summary>
        public Deck()
        {
            Cards = DeckService.CreateCardDeck();
        }

        /// <summary>
        /// List of cards in the deck
        /// </summary>
        public IList<Card> Cards { get; set; }
    }
}
