
using PokerGameCore.Attributes;

namespace PokerGameCore.Model
{
    /// <summary>
    /// Class containing the suits of a deck
    /// </summary>
    public class Suits
    {
        /// <summary>
        /// 4 suits in deck of cards
        /// </summary>
        public enum CardSuits
        {
            [Name("Hearts")]
            Hearts = 0,

            [Name("Diamonds")]
            Diamonds,

            [Name("Spades")]
            Spades,

            [Name("Clubs")]
            Clubs
        }
    }
}
