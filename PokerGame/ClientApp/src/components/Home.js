import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { players: [], loading: true };
    }

    componentDidMount() {
        this.populatePlayerData();
    }

    static renderPlayersTable(players) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Player Name</th>
                        <th>Card 1</th>
                        <th>Card 2</th>
                        <th>Card 3</th>
                        <th>Card 4</th>
                        <th>Card 5</th>
                    </tr>
                </thead>
                <tbody>
                    {players.map(player =>
                        <tr key={player.playerName}>
                            <td>{player.playerName}</td>
                            {player.playerHand.cards.map(card =>
                                <td key={card.cardNumberString + " " + card.cardSuitString}>{card.cardNumberString} {card.cardSuitString}</td>)}

                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderPlayersTable(this.state.players);
        return (
            <div>
                <h1 id="tabelLabel" >Poker Game</h1>
                {contents}
                <button className="btn btn-primary" onClick={this.declareWinner}>Find Winner</button>
                &nbsp;&nbsp;
                <button className="btn btn-primary" onClick={this.newGame.bind(this)}>New Game</button>
            </div>
        );
    }

    async populatePlayerData() {
        const response = await fetch('pokergame');
        const data = await response.json();
        this.setState({ players: data, loading: false });
    }

    async newGame() {
        this.populatePlayerData();
    }

    async declareWinner() {
        const response = await fetch('pokergame/getwinner');
        const data = await response.text();
        alert(data);
    }


}