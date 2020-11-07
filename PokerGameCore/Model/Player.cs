using System;

namespace PokerGameCore
{
    /// <summary>
    /// The poker player
    /// </summary>
    public class Player
    {   
        /// <summary>
        /// Player constructor
        /// </summary>
        public Player()
        {
            PlayerHand = new Hand();
        }

        /// <summary>
        /// Player's name
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Player's hand
        /// </summary>
        public Hand PlayerHand { get; set; }
    }
}
