using System;

namespace PokerGameCore.Attributes
{
    /// <summary>
    /// Attribute to define the string representation of the suits and cards
    /// </summary>
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// String representation of the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor definition of name string
        /// </summary>
        /// <param name="suitName"></param>
        public NameAttribute(string suitName)
        {
            Name = suitName;
        }
    }
}
