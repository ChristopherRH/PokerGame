######## Set up ########
1. Node js is required to build and deploy the solutions.  Install node js at https://nodejs.org/ and then restart your computer.
2. Open the project in visual studio 2019, then right click the solution and select "Build".  This will restore any nuget packages required, and download any npm packages required.
3. Open a terminal inside of the PokerGame project (e.g. /PokerGame/PokerGame) and run npm install to download all required modules
4. The project can be started in debug mode now
5. For unit testing, you can right click the PokerGame.Tests project, and Run Tests, or use the "Test" menu option at the top of visual studio and select "Run all tests"


######## Notes ########
1. Unit tests cover PokerGameCore, as this project contains all Models, Service, and Utility methods required of the solutions.
2. React is currently untested.

######## Explanation ########
1. The application will start up with 2 players (Player 1 and Player 2).  And their 5 drawn poker cards from the deck.
2. The "Find Winner" button will display in an alert which of the players has a winning hand, and what they won with.
3. The "New Game" button will discard the current hands, and draw 5 new cards for each player.

######## Online Version ########
Try the app out at:
https://pokergame20201107113144.azurewebsites.net/
