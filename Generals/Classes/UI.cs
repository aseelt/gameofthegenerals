using Generals.Exceptions;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Generals.Classes
{
    public class UI
    {
        // give me my colours
        // found on stack overflow. Someone used the ansi escape sequences to work out the colours
        private string Green = Console.IsOutputRedirected ? "" : "\x1b[92m";
        private string Red = Console.IsOutputRedirected ? "" : "\x1b[91m";
        private string Blue = Console.IsOutputRedirected ? "" : "\x1b[94m";
        private string Grey = Console.IsOutputRedirected ? "" : "\x1b[97m";


        // start game
        public Game Game;

        // player toggle. player0 (playerOne) is false, player1 (playerTwo) is true
        //private int PlayerToggle { get { return Game. } }

        public bool InitializeUIRuntime()
        {
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine($"{Red} ####                    {Red}{Grey}                                 {Grey}{Blue} ####                                           {Blue}");
            Console.WriteLine($"{Red}#    #   ##   #    # ####{Red}{Grey}   ###  ####  ##### #   # ####   {Grey}{Blue}#    # #### #    # #### ####    ##   #     #### {Blue}");
            Console.WriteLine($"{Red}#       #  #  ##  ## #   {Red}{Grey}  #   # #       #   #   # #      {Grey}{Blue}#      #    ##   # #    #   #  #  #  #    #     {Blue}");
            Console.WriteLine($"{Red}# #### #    # # ## # ### {Red}{Grey}  #   # ###     #   ##### ###    {Grey}{Blue}# #### ###  # #  # ###  #   # #    # #     #### {Blue}");
            Console.WriteLine($"{Red}#    # ###### #    # #   {Red}{Grey}  #   # #       #   #   # #      {Grey}{Blue}#    # #    #  # # #    ####  ###### #         #{Blue}");
            Console.WriteLine($"{Red}#    # #    # #    # #   {Red}{Grey}  #   # #       #   #   # #      {Grey}{Blue}#    # #    #   ## #    #  #  #    # #    #    #{Blue}");
            Console.WriteLine($"{Red} ####  #    # #    # ####{Red}{Grey}   ###  #       #   #   # ####   {Grey}{Blue} ####  #### #    # #### #   # #    # ####  #### {Blue}");

            //TODO add more fluff later 

            Console.WriteLine($"{Grey}\n\n\n            Welcome to the Game of the Generals.\n\n            Press Alt + Enter at any time to play in full screen mode.\n\n            Press any key to continue.{Grey}");

            Console.ReadKey();
            Console.Clear();

            Phase1GameSetup();
            Phase2SetupPieces();
            Phase3MovesAndCombat();
            Phase4Victory();

            return true;
        }

        /// <summary>
        /// Toggles between the two players. Calls the method in Game, it doesn't live here.
        /// </summary>
        /// <returns>Returns 0 for the first player, 1 for the second player</returns>
        private int TogglePlayersInGame()
        {
            return Game.TogglePlayers();
        }

        /// <summary>
        /// Helper method to get the player number, so I don't have to keep calling the Game object
        /// </summary>
        /// <returns>Returns 0 for the first player, 1 for the second player</returns>
        private int GetPlayerNumberFromGame()
        {
            return Game.GetPlayerNumber();
        }

        /// <summary>
        /// Helper method to get the opposite player number's, so I don't have to keep calling the Game object
        /// </summary>
        /// <returns>If player one selected, returns 1 (player two) and vice versa.</returns>
        private int GetOppositePlayerNumberFromBoard()
        {
            return Game.GetOppositePlayerNumber();
        }

        /// <summary>
        /// Asks for a string with the entry on the same line.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns user input in entered case, trimmed</returns> 
        private string AskForStringEnteredCase(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim();
        }

        /// <summary>
        /// Asks for a string with the entry on the same line.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns user input in UPPERCASE, trimmed</returns> 
        private string AskForStringUpperCase(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim().ToUpper();
        }

        /// <summary>
        /// Asks the user for a single key input which will clear the display.
        /// </summary>
        /// <returns>Always returns true</returns>
        private bool ContinueAndClear(string message)
        {
            Console.Write(message);
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("\u001b[2J\u001b[3J"); // found this on reddit. Uses an ansi escape code,
                                                     // the text represents "ESC [3J", which erases the scrollback buffer
                                                     // can go either before or after the clear, but you need the clear otherwise you have black space
            return true;
        }

        /// <summary>
        /// Simple clear, no key entry required
        /// </summary>
        /// <returns>Always returns true</returns>
        private bool SimpleClear()
        {
            Console.Clear();
            Console.WriteLine("\u001b[2J\u001b[3J"); // found this on reddit. Uses an ansi escape code,
                                                     // the text represents "ESC [3J", which erases the scrollback buffer
                                                     // can go either before or after the clear, but you need the clear otherwise you have black space
            return true;
        }

        /// <summary>
        /// Displays the board / battlefield for that specific player. 
        /// Has to live here otherwise we can't show both sides
        /// </summary>
        /// <param name="playerToggle"></param>
        /// <returns>String battlefield for that player</returns>
        public string BattlefieldDisplay(int playerToggle)
        {
            string output = "";
            // taking out the blank spaces for extra space
            // string BlankLine = "x         x";
            string MiddleLine = $"{Grey}+-       -+{Grey}";
            string BottomLine = $"{Grey}+---------+{Grey}";

            for (int h = 0; h < 8; h++)
            {
                //top line, has to be like this 
                for (int i = 0; i < 9; i++)
                {
                    output += $"{Green}{Game.Battlefields[playerToggle].xPositionToLetter[i]}{h + 1}{Green}{Grey}--------+{Grey}";
                }
                output += "\n";

                //output += CreateUILine(BlankLine);

                //player one line, has to be like this
                for (int i = 0; i < 9; i++)
                {
                    output += DisplayChooserTop(h, i, playerToggle);
                }
                output += "\n";

                //output += CreateUILine(BlankLine);
                output += CreateUILine(MiddleLine);
                //output += CreateUILine(BlankLine);

                //player two line, has to be like this
                for (int i = 0; i < 9; i++)
                {
                    output += DisplayChooserBottom(h, i, playerToggle);
                }
                output += "\n";

                //output += CreateUILine(BlankLine);
            }
            output += CreateUILine(BottomLine);
            return output;
        }

        /// <summary>
        /// Helper method for the battlefield display. Calculates Player One's row - both visible and hidden states.
        /// Should only be used in BattlefieldDisplay(), not elsewhere.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="i"></param>
        /// <returns>String of Player One's pieces, by location</returns>
        private string DisplayChooserTop(int h, int i, int playerToggle)
        {
            string topPlayerLine;
            if (playerToggle == 0)
            {
                // if player toggle is 0 get the first player's battlefield
                // for the cell, plus the army's name (the battlefield name), pull the display name

                topPlayerLine = $"{Grey}| {Grey}{Red}{Game.Battlefields[0].Grid[$"{Game.Battlefields[0].xPositionToLetter[i]}{h + 1}" /* grid position gives that piece */].GetShortDisplayName() /* what that piece is*/ }{Red}{Grey} |{Grey}";
            }
            else
            {
                // if it's not the active player, pull the cell, and the battlefield's name
                // but pull the hidden name
                topPlayerLine = $"{Grey}| {Grey}{Red}{Game.Battlefields[0].Grid[$"{Game.Battlefields[1].xPositionToLetter[i]}{h + 1}"].GetHiddenName()}{Red}{Grey} |{Grey}";
            }
            return topPlayerLine;
        }

        /// <summary>
        /// Helper method for the battlefield display. Calculates Player Two's row - both visible and hidden states.
        /// Should only be used in BattlefieldDisplay(), not elsewhere.
        /// Has to be duplicated like this, the code is slightly different to the Top version.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="i"></param>
        /// <returns>String of Player Two's pieces, by location</returns>
        private string DisplayChooserBottom(int h, int i, int playerToggle)
        {
            string bottomPlayerLine;
            if (playerToggle == 0)
            {
                bottomPlayerLine = $"{Grey}| {Grey}{Blue}{Game.Battlefields[1].Grid[$"{Game.Battlefields[0].xPositionToLetter[i]}{h + 1}"].GetHiddenName()}{Blue}{Grey} |{Grey}";

            }
            else
            {
                bottomPlayerLine = $"{Grey}| {Grey}{Blue}{Game.Battlefields[1].Grid[$"{Game.Battlefields[1].xPositionToLetter[i]}{h + 1}"].GetShortDisplayName()}{Blue}{Grey} |{Grey}";
            }
            return bottomPlayerLine;
        }

        /// <summary>
        /// Helper method for the battlefield display. Generates the static lines.
        /// Should only be used in BattlefieldDisplay(), not elsewhere.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>String used by BattlefieldDisplay()</returns>
        private string CreateUILine(string line)
        {
            string output = "";
            for (int i = 0; i < 9; i++)
            {
                output += line;
            }
            output += "\n";
            return output;
        }

        //TODO intro method
        // includes background and instructions

        /// <summary>
        /// Master Method for Phase 0. Provides background on the game and helper instructions
        /// </summary>
        /// <returns>Prints directly to the console a helper menu. Always returns true</returns>
        private bool Phase0Introduction()
        {
            bool keepGoing = true;
            do
            {
                try
                {
                    string menuSelection = AskForStringUpperCase("\nWelcome to the Help menu for the Game of the Generals!\n" +
                        "\nFor background information on the Game of The Generals, enter (1)." +
                        "\nFor information on the pieces that comprise your army, enter (2)" +
                        "\nFor information on the setup phase, enter (3)" +
                        "\nFor information on moves and combat, enter (4)" +
                        "\nFor information on victory conditions, enter (5)" +
                        "\nTo exit, enter (0)" +
                        "\n\nWhere would you like to navigate to? ");
                    switch (menuSelection)
                    {
                        case "1":
                            MenuOption1Background();
                            break;
                        case "2":
                            MenuOption2Pieces();
                            break;
                        case "3":
                            MenuOption3Setup();
                            break;
                        case "4":
                            MenuOption4MovesAndCombat();
                            break;
                        case "5":
                            MenuOption5Victory();
                            break;
                        case "0":
                            Console.WriteLine();
                            keepGoing = false;
                            break;
                        default:
                            throw new EntryWrongException();
                            break;
                    }
                }
                catch (EntryWrongException ewe)
                {
                    Console.WriteLine(ewe.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("You have entered an incorrect value. Please try again.\n");
                }

            } while (keepGoing);
            return true;
        }
         
        private bool MenuOption1Background()
        {
            Console.WriteLine("\nWelcome to the Game of the Generals!\nThe Game of the Generals, sometimes known as The Generals, is a Filipino board game invented\n" +
                "by Sofronio H. Pasola Jr. in 1970.\n\n" +
                "The game consists of two players with opposing armies with the computer acting as the role of a neutral arbiter. The game simulates the fog of war,\n" +
                "players do not have visibility into their opponents' pieces as in chess - this can only be guessed at based on movement, past actions, and behaviours.\n" +
                "Furthermore, there is an element of rock, paper, scissors involved among the pieces. Pieces may be deployed in any order among the first three rows\n" +
                "and they are free to move one space at a time, horizontally or vertically (not diagonally), provided there is not a conflict with your own army.\n" +
                "These elements allow for a greater amount of strategic depth and subterfuge. during play.\n\n" +                
                "Each team has a Flag which must be protected. There are two ways to win the game: capturing your opponent's Flag or getting your own Flag safely across\n" +
                "to the other side of the board.\n\n" +
                "The game features a high degree of replayability. Players can experiment with different tactics: a full frontal assualt with the most powerful pieces,\n" +
                "holding a defensive position, employing misdirection tactics and feints, stacking pieces to one side of the board, and more.\n\n" +
                "This game is a recreation of the physical board game. This game will preserve the fog of war for you, giving ample opportunity to be prepared to hand\n" +
                "over the console to the opposing player, provided you look away at the correct times. You will be able to see your opponent's deployment,\n" +
                "but not the specific nature of their pieces and their order of battle."); 

            return true;
        }
        private bool MenuOption2Pieces()
        {
            Console.WriteLine("\nEach army has 21 pieces. 1 Flag, 2 Spies, 6 Privates, and 12 Officers of increasing rank. Pieces have differing strengths:\n" +
                "- Any piece may capture the opponent's flag, including your own flag.\n" +
                "- Any Officer may eliminate any lower ranking piece, except for Spies to which they are susceptible.\n" +
                "- Spies may eliminate any piece except for Privates to which they are susceptible.\n" +
                "- Privates may eliminate Spies and the Flag.\n\n" +
                "Name               Rank        Short Name  Vulnerable To\n" +
                "Flag               N/A         Flag!       All\n" +
                "Spy (2x)           Special     Spy         Private\n" +
                "Private (6x)       0           Pte         All Ranked Officers\n" +
                "Sergeant           1           Sarge       All Higher Ranks, Spy\n" +
                "2nd Lieutenant     2           2. Lt       All Higher Ranks, Spy\n" +
                "1st Lieutenant     3           1. Lt       All Higher Ranks, Spy\n" +
                "Captain            4           Cap         All Higher Ranks, Spy\n" +
                "Major              5           Major       All Higher Ranks, Spy\n" +
                "Lieutenant Colonel 6           L.Col       All Higher Ranks, Spy\n" +
                "Colonel            7           Col         All Higher Ranks, Spy\n" +
                "1* General         8           *           All Higher Ranks, Spy\n" +
                "2** General        9           **          All Higher Ranks, Spy\n" +
                "3*** General       10          ***         All Higher Ranks, Spy\n" +
                "4**** General      11          ****        All Higher Ranks, Spy\n" +
                "5***** General     12          *****       All Higher Ranks, Spy\n");
            return true;
        }
        private bool MenuOption3Setup()
        {
            Console.WriteLine("\nAfter entering in your names, you will be taken to the Setup Phase.\n\n" +
                "During this phase, you will be allowed to place your pieces at will in the three rows closest to your side of the board.\n" +
                "There are 21 pieces available for the 27 spaces you have open - you will have some space to maneuver.\n\n" +
                "You may assign the pieces one by one, or have the computer randomly assign the pieces for you.\n" +
                "- You will get the chance to review your pieces and make edits before gameplay begins.\n" +
                "The two middle rows will be empty upon the start of the game. You may move freely into them once the game begins.");
            return true;
        }
        private bool MenuOption4MovesAndCombat()
        {
            Console.WriteLine("\nPlayer One will go first and play will alternate. Neither player should look at their opponent's view directly - this will\n" +
                "ensure you maintain secrecy.\n\n" +
                "All pieces move a singular space horizontally or vertically(not diagonally), forwards or backwards, one piece per turn, on the 8x9 board.\n" +
                "Pieces may not jump over others. The software will not allow your piece to travel off the board. Two pieces of your army may not occupy\n" +
                "the same square.\n\n" +
                "If you move your piece into a square occupied by the opposing team, you will have a chance to verify your intent. If you confirm your intent,\n" +
                "you will initiate the combate phase. You will not be told the opposing piece's rank, simply the outcome.\n" +
                "- If you win, the opposing army's piece is withdrawn from the board.\n" +
                "- If you lose, your piece will be withdrawn from the board.\n" +
                "- In the event of a draw (both pieces with equal rank), both pieces are eliminated.\n\n" +
                "Remember to consider the rock, papers, scissors nature of the pieces.");
            return true;
        }
        private bool MenuOption5Victory()
        {
            Console.WriteLine("\nThere are two ways to win the game: capturing your opponent's Flag or getting your own Flag safely across to the other\n" +
                "side of the board.\n\n" +
                "In the event you capture your opponent's Flag, the game should auto resolve with a print out of the current field of play and the pieces\n" +
                "that were lost on both sides through the game.\n\n" +
                "If your Flag reaches the other side, it must survive one turn there in the event a neighbouring square has an opposing piece on it.\n" +
                "Remember, your Flag's position may not be determined by your opponent and they may not attack it.\n" +
                "- In the event that your Flag reaches the other side and there are no opponent pieces in neighbouring squares, this should resolve to an\n" +
                "immediate victory.");
            return true;
        }
        /// <summary>
        /// Master Method for Phase 1. Gets the name of the players to build the game.
        /// </summary>
        /// <returns>Always returns true</returns>
        private bool Phase1GameSetup()
        {
            string helpMenu = AskForStringUpperCase("Would you like to view the help menu? Y/N: ");
            if (helpMenu == "Y")
            {
                Phase0Introduction();
            }
            else if (helpMenu == "N")
            {
                Console.WriteLine("Okay, we'll get right into the game. You can access the help menu during most phases of the game.");
            }
            else
            {
                Console.WriteLine("I didn't recognize that input. Let's continue with the game.");
            }

            string playerOne = AskForStringEnteredCase("\nPlayer One, enter your army's name: ");

            string playerTwo = PlayerTwoSetup(playerOne);
            string field = BattlefieldName(playerOne, playerTwo);

            // can't add colours here, it gets real funky
            Console.WriteLine($"The great battle between {playerOne} and {playerTwo} is about to begin in {field}!\n");

            ContinueAndClear("Press any key to continue.");

            // each player gets their own battlefield
            // construct
            Game = new Game();
            Game.CreateBattlefield(playerOne, playerTwo);

            return true;
        }

        /// <summary>
        /// Returns the name of Player Two, validating it isn't the same as Player One.
        /// Requires Player One's name to perform the validation.
        /// </summary>
        /// <param name="playerOne"></param>
        /// <returns>String of Player Two, trimmed, in entered case.</returns>
        private string PlayerTwoSetup(string playerOne)
        {
            string playerTwo = "";
            bool keepGoingPlayerTwoName = true;
            do
            {
                playerTwo = AskForStringEnteredCase("\nPlayer Two, enter your army's name: ");
                
                string playerOneLower = playerOne.ToLower();
                string playerTwoLower = playerTwo.ToLower();
                if (playerTwoLower == playerOneLower)
                {
                    Console.WriteLine("That name has already been chosen. Please try again.");
                }
                else
                {
                    keepGoingPlayerTwoName = false;
                }
            } while (keepGoingPlayerTwoName);
            return playerTwo;
        }

        /// <summary>
        /// Returns the name of the battlefield, validating it isn't the same as Player One or Player Two.
        /// Requires Player One and Player Two's name to perform the validation.
        /// </summary>
        /// <param name="playerOne"></param>
        /// <param name="playerTwo"></param>
        /// <returns>String, the name of the battlfield.</returns>
        private string BattlefieldName(string playerOne, string playerTwo)
        {
            string field = "";
            bool keepGoingBattlefield = true;
            do
            {
                field = AskForStringEnteredCase("\nWhere are your armies meeting? ");

                string fieldLower = field.ToLower();
                string playerOneLower = playerOne.ToLower();
                string playerTwoLower = playerTwo.ToLower();
                if (fieldLower == playerTwoLower || fieldLower == playerOneLower)
                {
                    Console.WriteLine("That entry has already been chosen. Please try again.");
                }
                else
                {
                    keepGoingBattlefield = false;
                }
            } while (keepGoingBattlefield);
            return field;
        }

        /// <summary>
        /// Master Method for Phase 2. Sets up initial piece placement, for both Player One and Two
        /// </summary>
        /// <returns>Bool true always</returns>
        private bool Phase2SetupPieces()
        {
            //place piece
            //originates in UI
            // prints list of pieces yet to place y 
            // game method loops through list for each piece to be placed
            // ask for a piece y 
            // check if it's valid y 
            // check if it's on the not on board list, if true go ahead y
            // use that variable to select the piece from the list y 
            // ask where they want to put it y
            // make sure it's 1-3 for player 0, 6-8 for player 1 y
            // make sure that's a valid location y
            // make sure it's unoccupied y
            // sends instructions to the board
            // board then updates the piece y
            // remove from the list of pieces not on board but alive y
            // piece says "i am now on board"
            // keep going till the pieces not on board list is empty y
            // finally end with "Would you like to move any pieces
            // game method toggle determines which battlefield we are working with

            int setupCount = 0;
            do
            {
                // all pieces placed
                // ask them if they want to change any pieces
                // has to be a separate loop that asks when initial placement is all done
                Console.WriteLine($"{Game.Battlefields[GetPlayerNumberFromGame()].PlayerName}, it's time to place your pieces on the board.");

                do
                {
                    Console.WriteLine(SetupPiecesList());
                    do
                    {
                        // have to do this for player one and player two. reference their names
                        int piece = SetupAskForPiece();

                        if (piece == 77)
                        {
                            //do the randomized piece assignments on the board
                            //print the battlefield with "hey this is how the pieces are placed" header
                            // side method to pass location and piece number
                            // do that in the board
                            // call the game
                            Console.WriteLine(Game.SetupPiecePlacementRandomizerGame());
                            Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));
                        }
                        else
                        {
                            string location = SetupAskForLocation();
                            Console.WriteLine(Game.SetupPlacePieceGame(piece, location));
                        }

                    } while (Game.Battlefields[GetPlayerNumberFromGame()].PiecesNotOnBoard.Count > 0);

                    SetupChangePieceLocation();

                } while (Game.Battlefields[GetPlayerNumberFromGame()].PiecesNotOnBoard.Count > 0);

                // once that is done, change the players
                TogglePlayersInGame();

                //add to the count, when count is 2, loop exits
                setupCount++;


                ContinueAndClear("\nYour piece placement is complete. Press any key to clear your screen before handing over the console to your opponent." +
                    "\n\nNO PEEKING!");
                ContinueAndClear("This screen has been provided to ensure operational security for your army.\nPlease press any key to continue.");

            } while (setupCount < 2);



            return true;
        }

        /// <summary>
        /// Displays pieces not currently on the board (and alive).
        /// Used only in the setup phase.
        /// </summary>
        /// <returns>String of pieces.</returns>
        private string SetupPiecesList()
        {

            return Game.PiecesToPlace();
        }

        /// <summary>
        /// Ask for a piece to place from the list of peices not yet on the board.
        /// Also has a shortcut to display the list and grid again.
        /// </summary>
        /// <returns>Validated piece number on the list</returns>
        private int SetupAskForPiece()
        {
            do
            {
                try
                {
                    // this is slightly clunky for the player - there's a lot of typing and entry
                    // but it's simpler in the long run 
                    Console.Write("(Type '99' to view the grid) \n(Type '88' to view the list of pieces to place) " +
                        "\n(Type '77' to have the game assign the remaining pieces)" +
                        "\n(Type '00' to view the help menu)" +
                        "\n\nWhich Piece would you like to place? ");

                    int pieceChosen = int.Parse(Console.ReadLine().Trim());

                    // checks if the piece chosen is on the list of peices not on the board
                    // if so, breaks the loop and returns the piece for use
                    if (pieceChosen == 99)
                    {
                        Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));
                    }
                    else if (pieceChosen == 88)
                    {
                        Console.WriteLine(SetupPiecesList());
                    }
                    else if (pieceChosen == 77)
                    {
                        return pieceChosen;
                    }
                    else if (pieceChosen == 00)
                    {
                        Phase0Introduction();
                    }
                    else if (pieceChosen >= (1 - 1) & pieceChosen <= (Game.Battlefields[GetPlayerNumberFromGame()].PiecesNotOnBoard.Count))
                    {
                        return pieceChosen;
                    }
                    else
                    {
                        throw new PieceChosenWrongException();
                    }
                }
                catch (PieceChosenWrongException pcwe)
                {
                    Console.WriteLine(pcwe.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("You have entered an incorrect value. Please try again.\n");
                }

            } while (Game.Battlefields[GetPlayerNumberFromGame()].PiecesNotOnBoard.Count != 0);

            return 0;
        }

        /// <summary>
        /// Asks for a location to place a piece during the setup phase.
        /// Restricted to rows that the players are allowed to place pieces.
        /// </summary>
        /// <returns></returns>
        private string SetupAskForLocation()
        {
            bool keepGoing = true;
            do
            {
                try
                {
                    string locationChosen;
                    if (GetPlayerNumberFromGame() == 0)
                    {
                        locationChosen = AskForStringUpperCase("Please enter a grid location (e.g. A1, B2, C5) in rows 1-3 to place your piece: ");
                    }
                    else
                    {
                        locationChosen = AskForStringUpperCase("Please enter a grid location (e.g. A6, B7, C8) in rows 6-8 to place your piece: ");
                    }

                    if (!string.IsNullOrEmpty(locationChosen) && Game.Battlefields[GetPlayerNumberFromGame()].GridReference.Contains(locationChosen))
                    {
                        if (GetPlayerNumberFromGame() == 0 && (locationChosen.EndsWith("1") || locationChosen.EndsWith("2") || locationChosen.EndsWith("3")))
                        {
                            keepGoing = false;
                            return locationChosen;
                        }
                        else if (GetPlayerNumberFromGame() == 1 && (locationChosen.EndsWith("6") || locationChosen.EndsWith("7") || locationChosen.EndsWith("8")))
                        {
                            keepGoing = false;
                            return locationChosen;
                        }
                        else
                        {
                            throw new LocationChosenWrongException();
                        }
                    }
                    else
                    {
                        throw new LocationChosenWrongException();
                    }
                }
                catch (LocationChosenWrongException lcwe)
                {
                    Console.WriteLine(lcwe.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("You have entered an incorrect value. Please try again.\n");
                }

            } while (keepGoing);

            return "";
        }

        /// <summary>
        /// After the pieces are first placed on the board, players will have the chance to change their deployment
        /// This is the helper method for this process
        /// </summary>
        /// <returns>Always returns false, this breaks the loop in the calling method</returns>
        private bool SetupChangePieceLocation()
        {
            bool keepGoingYesNo = true;
            do
            {
                string changePieceLocationAnswer = AskForStringUpperCase("\nWould you like to change the location of any pieces? (Y/N) ");
                if (changePieceLocationAnswer == "Y")
                {
                    // removes piece, adds it back to the notonboard list
                    // changes the piece's onboard status to false
                    // put blank piece in that square
                    bool keepGoingPiece = true;
                    do
                    {
                        try
                        {
                            //print message 
                            string locationChosen = AskForStringUpperCase("(Type '99' to view the grid) " +
                                "\n(Type '88' to view the list of pieces to place)" +
                                "\n(Type '00' to view the help menu)" +
                                "\n\nEnter the grid location of the piece you would like to change. ");

                            // checks if the piece chosen is on the list of peices not on the board
                            // if so, breaks the loop and returns the piece for use
                            if (locationChosen == "99")
                            {
                                Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));
                            }
                            else if (locationChosen == "88")
                            {
                                Console.WriteLine(SetupPiecesList());
                            }
                            else if (Game.Battlefields[GetPlayerNumberFromGame()].GridReference.Contains(locationChosen))
                            {
                                // throws exception if you try and choose an empty space
                                if (Game.Battlefields[GetPlayerNumberFromGame()].Grid[locationChosen].GetName() == "Blank")
                                {
                                    throw new PieceChosenWrongException();
                                }
                                // by this point it should be a valid location
                                // change that position's on board status to false
                                // put the piece in that location back on the list
                                // put a blank piece in that location
                                // end the loop 
                                keepGoingPiece = Game.SetupChangePieceLocation(locationChosen);
                            }
                            else
                            {
                                throw new LocationChosenWrongException();
                            }

                        }
                        catch (PieceChosenWrongException pcwe)
                        {
                            Console.WriteLine(pcwe.Message);
                        }
                        catch (LocationChosenWrongException lcwe)
                        {
                            Console.WriteLine(lcwe.Message);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("You have entered an incorrect value. Please try again.\n");
                        }

                    } while (keepGoingPiece);
                }
                else if (changePieceLocationAnswer == "N")
                {
                    keepGoingYesNo = false;
                    return false;
                }
                else
                {
                    Console.WriteLine("You've entered an incorrect value.");
                }
            } while (keepGoingYesNo);

            return false;
        }
        /// <summary>
        /// Master Method for Phase 3. Handles moves, combat, calling on helper methods
        /// </summary>
        /// <returns></returns>
        private bool Phase3MovesAndCombat()
        {
            // check if their ReachedTheOtherSide victory condition is 1 (has to be there for a full turn of the other team) y
            //      And flag has to be alive (it can be killed by the opposing team)
            //      if so, break loop and declare victory
            // ask for a current location y
            //      00 for rules... 
            //      66 to see own killed pieces 
            //      ff for forefeit
            // check if position has a selectable piece - handle this at the board level y
            //      return true/false
            //      if false, loop back
            // store as current location y 
            // ask for a move up/down/left/right y
            //      check if prospective position selected is valid - handle this at the board level 
            //      check if move doesn't collide with existing piece on team - handle this at the board level
            //      if false, loop back            
            // store future location y 
            //      return true/false
            //      if any false, loop back
            //      if both true, store new location, grab piece from current location and continue
            // if all good by here, you have a piece in hand, location to send it to, current location. Then: y
            //      check if it collides with piece of other player - handle at game level y
            //          if it does, return true, if not, false
            //          if true, attack protocol - handle this at the game level  y
            //              Ask Y/N if you want to attack y
            //                  If Y, attack and resolve - handle at the game level y
            //                      For kills, use kill method to kill the piece and remove from board y
            //                          Update whichever lists I have going... y
            //                      If flag attacking opponent flag, give it an attacking boost n/a
            //                      If flag piece killed, update victory condition to whichever player won y
            //                          Break Phase 3 loop y
            //                      Log attack, pieces, outcome
            //                  If N, loop back to start and ask for a location
            //          if false, move protocol - handle at the board level y
            //              Update desired location with piece y
            //              Update current location with blank piece y
            //              Log move, pieces, moves
            //                  If player 0 flag reaches row 8, update "ReachedTheOtherSide" victory condition to 1 y
            //                  If player 1 flag reaches row 1, update "ReachedTheOtherSide" victory condition to 1 y
            //                      If either player's flag reaches the end and the spaces either side are blank, declare immediate victory y
            //                          Need to have an exception that returns ok if the flag reaches the end in a corner and the next space over doesn't exist y
            //                  Break loop
            // If those all resolve, clear screen (twice...) and toggle player
            bool victory = false;

            do
            {
                // check if a player has met the conditions for flag reaching the other side victory
                if (Game.CheckFlagWaitingVictory())
                {
                    Console.WriteLine("Your flag reached safety and was unharmed! Victory is yours!");
                    // this is not quite encapsulated... but I can't figure out how to add it to the method
                    Game.UpdateVictoryMethod($"{Game.Battlefields[GetPlayerNumberFromGame()].PlayerName}'s flag snuck in to safety, unchallenged, for a close win!");
                    Log.AddToLog(Game.Battlefields[GetPlayerNumberFromGame()].PlayerName, "End", "Flag snuck into safety.");
                    victory = true;
                }
                else
                {
                    Console.WriteLine($"{Game.Battlefields[GetPlayerNumberFromGame()]}, it is time to make your move!\n");
                    Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));
                    try
                    {
                        // no loop here, you want this all to play out
                        string currentLocation = "";
                        string futureLocation = "";
                        string code = "";
                        do
                        {
                            // need a loop in case the user gets stuck selecting a piece they can't move
                            currentLocation = AskForLocation(out code);

                            if (code == "FF")
                            {
                                // break the loops                                
                                victory = true;
                            }
                            else
                            {
                                futureLocation = AskForMove(currentLocation, out code);

                                // grab the current piece
                                Piece currentPiece = Game.Battlefields[GetPlayerNumberFromGame()].Grid[currentLocation];

                                // check if the other player has a piece in that future location
                                // if it's blank, make the move
                                // i.e. if it's not blank, then you have a combat situation

                                // attack pathway
                                if (Game.Battlefields[GetOppositePlayerNumberFromBoard()].Grid[futureLocation].GetName() != "Blank")
                                {
                                    // ugh, reusing this variable is not the right move, but it has to go in that bucket
                                    bool attack = StageAttack(currentLocation, futureLocation, currentPiece, out code);

                                    if (attack)
                                    {
                                        string battleOutcome = Game.ManageAttack(currentLocation, futureLocation);
                                        Console.WriteLine("The battlefield, post skirmish");
                                        Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));
                                        Console.WriteLine(battleOutcome);


                                        if (Game.Victor != -1)
                                        {
                                            code = "";
                                            victory = true;
                                        }                                 
                                    } 
                                }
                                else // move pathway
                                {
                                    string victoryCheck = Game.MakeMove(currentLocation, futureLocation);
                                     
                                    ContinueAndClear("Press any key to continue");
                                    Console.WriteLine("The battlefield, post maneuvers.");
                                    Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));
                                    // print the move
                                    Console.WriteLine($"\nYour {currentPiece.GetName()} has been moved from {currentLocation} to {futureLocation}.\n");

                                    // check the flag location
                                    if (victoryCheck == "Immediate")
                                    {
                                        Log.AddToLog(Game.Battlefields[GetPlayerNumberFromGame()].PlayerName, futureLocation, "Flag reached safety unopposed.");
                                        Console.WriteLine("Your flag reached safety! Victory is yours!");
                                        code = "";
                                        victory = true;
                                    }
                                }
                            }

                        } while (code == "XX");
                    }
                    catch (Exception)
                    {

                    }
                    // if victory is called, no need to continue this
                    if (!victory)
                    {
                        // if you get here, your turn is done
                        // once that is done, change the players
                        TogglePlayersInGame();
                        ContinueAndClear("Your moves for this round are complete. Press any key to clear your screen before handing over the console to your opponent." +
                            "\n\nNO PEEKING!");
                        ContinueAndClear("This screen has been provided to ensure operational security for your army.\nPlease press any key to continue.");
                    }
                }

            } while (!victory);

            return true;
        }

        /// <summary>
        /// Asks for a location from the user
        /// </summary>
        /// <returns>The location as a string</returns>
        private string AskForLocation(out string code)
        {
            bool keepGoing = true;
            do
            {
                try
                {
                    string locationChosen;

                    // forfeit has to live here
                    locationChosen = AskForStringUpperCase("(Type '99' to view the grid) " +
                        "\n(Type '66' to view a list of your casualties)" +
                        "\n(Type 'FF' to forfeit the game)" +
                        "\n(Type '00' to view the help menu)" +
                        "\n\nPlease enter the location of the piece you would like to move: ");

                    if (locationChosen == "99")
                    {
                        Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));
                    }
                    else if (locationChosen == "66")
                    {
                        Console.WriteLine(Game.GetListOfDeaths());
                    }
                    else if (locationChosen == "FF")
                    {
                        // forfeit is by the current player
                        // means the other player gets to declare victory
                        // exits this loop 
                        if (AreYouSure("Are you sure? ") == "Y")
                        {
                            Game.ForfeitProtocol();
                            ContinueAndClear("You have forfeited this game!");
                            keepGoing = false;
                            code = "FF";
                        }
                    }
                    else if (locationChosen == "00")
                    {
                        Phase0Introduction();
                    }
                    else if (Game.Battlefields[GetPlayerNumberFromGame()].GridReference.Contains(locationChosen))
                    {
                        locationChosen = Game.ConfirmLocationHasValidPiece(locationChosen);
                        keepGoing = false;
                        code = "";
                        return locationChosen;
                    }
                    else
                    {
                        throw new LocationChosenWrongException();
                    }
                }
                catch (LocationChosenWrongException lcwe)
                {
                    Console.WriteLine(lcwe.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("You have entered an incorrect value. Please try again.\n");
                }

            } while (keepGoing);
            code = "";
            return "";
        }

        /// <summary>
        /// Asks for a move direction from the user. 
        /// Validates if it's possible based on direction (i.e. not off the board)
        /// and if there is a conflicting friendly piece
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <returns>Returns the future location of the piece</returns>
        private string AskForMove(string currentLocation, out string code)
        {
            bool keepGoing = true;
            do
            {
                try
                {
                    string direction = AskForStringUpperCase("\nYou can move your piece (U)p, (D)own, (L)eft, or (R)ight? " +
                        "\nEnter (x) to exit" +
                        "\n\nWhich direction would you like to move your piece? ");

                    // send the prospective move to the board with the current location
                    // if it's okay, get back the future location coordinates
                    if (direction == "U" || direction == "D" || direction == "L" || direction == "R")
                    {
                        string futurePosition = Game.ConfirmMoveIsValid(currentLocation, direction);
                        keepGoing = false;
                        code = "";
                        return futurePosition;
                    }
                    else if (direction == "X")
                    {
                        keepGoing = false;
                        code = "XX"; 
                    }
                }
                catch (DirectionChosenWrongException dcwe)
                {
                    Console.WriteLine(dcwe.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("You have entered an incorrect value. Please try again.\n");
                }

            } while (keepGoing);

            code = "";
            return "";
        }

        /// <summary>
        /// Asks for confirmation. Used in forfeit protocol and attack
        /// </summary>
        /// <returns>Returns true if confirmation of action</returns>
        private string AreYouSure(string message)
        {
            string continuing = "\nReturning to the previous menu.\n";
            try
            {
                string confirmation = AskForStringUpperCase(message);
                if (confirmation == "Y")
                {
                    return "Y";
                }
                else if (confirmation == "N")
                {
                    Console.WriteLine(continuing);
                    return "N";
                }
                else
                {
                    throw new EntryWrongException();
                }
            }
            catch (EntryWrongException ewe)
            {
                Console.WriteLine(ewe.Message);
                Console.WriteLine(continuing);
            }
            return "";
        }

        /// <summary>
        /// Stages the combat before the player commits
        /// Shows the two pieces on the same square!
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <param name="futureLocation"></param>
        /// <returns>Returns messages regarding outcome</returns>
        public bool StageAttack(string currentLocation, string futureLocation, Piece currentPiece, out string code)
        {
            // start with moving it to the new location so both occupy the same square                            
            Game.Battlefields[GetPlayerNumberFromGame()].Grid[futureLocation] = currentPiece;
            Game.Battlefields[GetPlayerNumberFromGame()].Grid[currentLocation] = new Piece(-3);

            SimpleClear();
            // then ask are you sure
            Console.WriteLine("Please view the board carefully.");
            Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));

            int moveComplete = 0;
            if (AreYouSure("Are you sure? Y/N: ") == "Y")
            {
                Console.WriteLine("You are attacking!");
                ContinueAndClear("Press any key when you are ready to continue"); 
                code = "";
                return true; 
            }
            else
            {
                // if no, undo the move step
                // put things back to how they were
                Game.Battlefields[GetPlayerNumberFromGame()].Grid[futureLocation] = new Piece(-3);
                Game.Battlefields[GetPlayerNumberFromGame()].Grid[currentLocation] = currentPiece;

                ContinueAndClear("Press any key to refresh the screen. Your move will be reversed.");
                Console.WriteLine("The current battlefield:");
                Console.WriteLine(BattlefieldDisplay(GetPlayerNumberFromGame()));

                // restart the menu option
                code = "XX";
                return false;
            }  
        }




        private bool Phase4Victory()
        {
            ContinueAndClear($"\n{Green}Victory has been declared!!{Green}");

            Console.WriteLine("Victory has been declared!");

            Console.WriteLine($"\n{Game.Battlefields[Game.Victor].PlayerName} was the winner today! Congratulations!");
            Console.WriteLine($"{Game.VictoryMethod}");

            Console.WriteLine("\nThis is the battlefield as the game ended:\n");
            Console.WriteLine(Phase4BattlefieldDisplay());

            if (Math.Abs((Game.Battlefields[0].PiecesKilled.Count - Game.Battlefields[1].PiecesKilled.Count)) < 4)
            {
                Console.WriteLine($"\nIt was a close victory. {Game.Battlefields[0].PlayerName} lost {Game.Battlefields[0].PiecesKilled.Count} pieces compared to {Game.Battlefields[1].PlayerName}'s losses of {Game.Battlefields[1].PiecesKilled.Count} pieces.");
            }
            else
            {
                Console.WriteLine($"\nOne player showed a mastery of tactics and subterfuge. {Game.Battlefields[0].PlayerName} lost {Game.Battlefields[0].PiecesKilled.Count} pieces compared to {Game.Battlefields[1].PlayerName}'s losses of {Game.Battlefields[1].PiecesKilled.Count} pieces.");
            }
            
            Console.WriteLine(Phase4ListOfPiecesAttacked());

            Console.WriteLine($"\n{Grey}Thank you for playing. I hope you come back again!{Grey}");
            Console.WriteLine("\nBy Aseel Tungekar, October 2023.");

            return true;
        }

        private string Phase4BattlefieldDisplay()
        {

            string output = ""; 
            string MiddleLine = $"{Grey}+-       -+{Grey}";
            string BottomLine = $"{Grey}+---------+{Grey}";

            for (int h = 0; h < 8; h++)
            {
                //top line, has to be like this 
                for (int i = 0; i < 9; i++)
                {
                    output += $"{Green}{Game.Battlefields[0].xPositionToLetter[i]}{h + 1}{Green}{Grey}--------+{Grey}";
                }
                output += "\n";


                //player one line, has to be like this
                for (int i = 0; i < 9; i++)
                {
                    output += $"{Grey}| {Grey}{Red}{Game.Battlefields[0].Grid[$"{Game.Battlefields[0].xPositionToLetter[i]}{h + 1}"].GetShortDisplayName()}{Red}{Grey} |{Grey}";
                }
                output += "\n";

                output += CreateUILine(MiddleLine);

                //player two line, has to be like this
                for (int i = 0; i < 9; i++)
                {
                    output += $"{Grey}| {Grey}{Blue}{Game.Battlefields[1].Grid[$"{Game.Battlefields[1].xPositionToLetter[i]}{h + 1}"].GetShortDisplayName()}{Blue}{Grey} |{Grey}";
                }
                output += "\n";
            }
            output += CreateUILine(BottomLine);
            return output;
        }

        private string Phase4ListOfPiecesAttacked()
        {
            string listOutput = $"These moves occurred over the course of the game:\n" +
                $"{Green}Attacking Player  Location    Attacker                Defender                Outcome{Green}\n";
            for (int i = 0; i < Log.Moves.Count; i++)
            {
                string[] holding = Log.Moves[i];
                listOutput += $"{Grey}{holding[0],-18}{holding[1],-12}{holding[2],-24}{holding[3],-24}{holding[4]}{Grey}\n";
            }
            return listOutput;
        }
    }
}
