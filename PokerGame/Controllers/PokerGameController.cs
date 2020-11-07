using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        /// <summary>
        /// logger for this controller
        /// </summary>
        private readonly ILogger<PokerGameController> _logger;
        
        // todo: this shouldn't be static -- every request will be using this deck
        private static Deck _deck;
       
        /// <summary>
        /// Default constructor, initialize logging
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
            if (_deck == null)
            {
                _deck = new Deck();
            }

            // if there's less than 10, we need to reset the deck
            if (_deck.Cards.Count < 10)
            {
                _deck.Cards = DeckService.CreateCardDeck();
            }

            // shuffle between every call to controller == every play
            DeckService.ShuffleDeck(_deck);

            var player1 = new Player
            {
                PlayerName = "Player 1"
            };

            var player2 = new Player
            {
                PlayerName = "Player 2"
            };

            // assign player hands
            for (int i = 0; i < 5; i++)
            {
                DeckService.DrawCard(player1, _deck);
                DeckService.DrawCard(player2, _deck);
            }

            // return players as a list with their cards
            return new List<Player> { player1, player2 };
       }

        /// <summary>
        /// Get the winner of the current players
        /// </summary>
        /// <returns></returns>
        [HttpGet("getwinner")]
        public string GetWinner(string playerInfo)
        {
            // the players in this game, assume there are always only 2
            var players = JsonConvert.DeserializeObject<List<Player>>(playerInfo);
            var winner = DeckService.DetermineWinner(players[0], players[1]);
            return $"Winner is {winner.PlayerName} with {AttributeUtil.GetAttributeValue<NameAttribute>(typeof(HandResult.HandResults), HandResult.DetermineHandResult(winner.PlayerHand).Item1.ToString(), nameof(NameAttribute.Name))}";
        }
    }
}
