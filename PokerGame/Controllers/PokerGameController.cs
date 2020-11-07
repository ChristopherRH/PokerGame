using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokerGameCore;
using PokerGameCore.Attributes;
using PokerGameCore.Service;
using PokerGameCore.Util;

namespace PokerGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokerGameController : ControllerBase
    {
        private readonly ILogger<PokerGameController> _logger;
        
        // todo: these shouldn't be static (very bad...)
        private static Deck _deck;
        private static Player _player1;
        private static Player _player2;

        /// <summary>
        /// Player 1 singleton
        /// </summary>
        private Player Player1
        {
            get
            {
                if (_player1 == null)
                {
                    _player1 = new Player();
                    _player1.PlayerName = "Player 1";
                }

                return _player1;
            }
        }

        /// <summary>
        /// Player 2 singleton
        /// </summary>
        private Player Player2
        {
            get
            {
                if (_player2 == null)
                {
                    _player2 = new Player();
                    _player2.PlayerName = "Player 2";
                }

                return _player2;
            }
        }


        /// <summary>
        /// Default constructor, ininitalize logging
        /// </summary>
        /// <param name="logger"></param>
        public PokerGameController(ILogger<PokerGameController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Set the player hands
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Player> Get()
        {
            // instantiate singleton
            if(_deck == null)
            {
                _deck = new Deck();
            }

            // if there's less than 10, we need to reset the deck
            if(_deck.Cards.Count < 10)
            {
                _deck.Cards = DeckService.CreateCardDeck();
            }

            // shuffle between every call to controller == every play
            DeckService.ShuffleDeck(_deck);

            // assign player hands
            Player1.PlayerHand.Cards.Clear();
            Player2.PlayerHand.Cards.Clear();
            for (int i = 0; i < 5; i++)
            {
                DeckService.DrawCard(Player1, _deck);
                DeckService.DrawCard(Player2, _deck);
            }

            // return players as a list with their cards
            return new List<Player> { Player1, Player2 }.ToArray();
       }

        /// <summary>
        /// Get the winner of the current players
        /// </summary>
        /// <returns></returns>
        [HttpGet("getwinner")]
        public string GetWinner()
        {
            var winner = DeckService.DetermineWinner(Player1, Player2);
            return $"Winner is {winner.PlayerName} with {AttributeUtil.GetAttributeValue<NameAttribute>(typeof(HandResult.HandResults), HandResult.DetermineHandResult(winner.PlayerHand).Item1.ToString(), nameof(NameAttribute.Name))}";
        }
    }
}
