
using PokerGameCore.Attributes;
using PokerGameCore.Util;
using System;
using static PokerGameCore.Model.Numbers;
using static PokerGameCore.Model.Suits;

namespace PokerGameCore
{
    /// <summary>
    /// A card, containg a suit and a number
    /// </summary>
    public class Card : IComparable
    {
        /// <summary>
        /// The card's suit
        /// </summary>
        public CardSuits CardSuit { get; set; }

        /// <summary>
        /// The card's number
        /// </summary>
        public CardNumbers CardNumber { get; set; }

        /// <summary>
        /// String representation of the CardSuit enum
        /// </summary>
        public string CardSuitString => (string)AttributeUtil.GetAttributeValue<NameAttribute>(typeof(CardSuits), CardSuit.ToString(), nameof(NameAttribute.Name));

        /// <summary>
        /// String representation of the CardNumber enum
        /// </summary>
        public string CardNumberString => (string)AttributeUtil.GetAttributeValue<NameAttribute>(typeof(CardNumbers), CardNumber.ToString(), nameof(NameAttribute.Name));

        /// <summary>
        /// IComparable implementation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if(obj == null)
            {
                return 1;
            }
            
            var otherCard = obj as Card;
            if (otherCard != null)
            {
                return CardNumber.CompareTo(otherCard.CardNumber); 
            }
            else
            {
                throw new ArgumentException($"Invalid Comparison in {nameof(Card)}");
            }
        }
    }
}
