
using PokerGameCore.Attributes;

namespace PokerGameCore.Model
{
    /// <summary>
    /// Class containing the suits of a deck
    /// </summary>
    public class Numbers
    {
        /// <summary>
        /// The 13 cards in a deck of cards
        /// </summary>
        public enum CardNumbers
        {
            [Name("Two")]
            Two = 1,

            [Name("Three")]
            Three,

            [Name("Four")]
            Four,

            [Name("Five")]
            Five,

            [Name("Six")]
            Six,

            [Name("Seven")]
            Seven,

            [Name("Eight")]
            Eight,

            [Name("Nine")]
            Nine,

            [Name("Ten")]
            Ten,

            [Name("Jack")]
            Jack,

            [Name("Queen")]
            Queen,

            [Name("King")]
            King,

            [Name("Ace")]
            Ace
        }
    }
}
