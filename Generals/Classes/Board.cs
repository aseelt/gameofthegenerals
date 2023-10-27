using Generals.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Generals.Classes
{
    public class Board
    {
        // holds the army and the grid reference

        // properties
        // battlefield name is who owns it, the player name
        // use this in a queue to manage the battlefield toggle
        // no set, once created by constructor it can't be changed
        public string PlayerName { get; }

        // array of positions to letters reference 
        public string[] xPositionToLetter = { "A", "B", "C", "D", "E", "F", "G", "H", "I" };

        // grid creation for reference
        public string[] GridReference = new string[72];

        // need an x axis and a y axis
        // the piece calls on the dictionary key values
        // movement adjusts the dictionary key values
        // if two pieces have the same key, trigger attack
        // i don't recall how to make this private and provide a copy - it works for now
        // the the values are blank, waiting to be filled
        // derived when the class is created
        // all 72 positions in one dictionary
        public Dictionary<string, Piece> Grid = new Dictionary<string, Piece>();


        // hold the army in here
        // needs to be set, we'll be changing the army (alive/dead)
        // no set, only the constructor can build the army
        // public get, want to access this outside of the class
        public List<Piece> Army { get; } = new List<Piece>();

        // list of pieces NOT on the board (NOT placed AND alive)
        // no set, created at time of board creation
        // public get, we want to access this with the game or the UI
        public List<Piece> PiecesNotOnBoard { get; } = new List<Piece>();

        // list of pieces on the board (placed and alive)
        // private set, only want the board changing this
        // public get, we want to access this with the game or the UI
        public List<Piece> PiecesOnBoard { get; private set; } = new List<Piece>();

        // list of pieces killed
        // private set, only want the board changing this
        // public get, we want to access this with the game or the UI
        public List<Piece> PiecesKilled { get; private set; } = new List<Piece>();

        // victory counters
        private int VictoryCount { get; set; } = 0;

        // set up the constructor, it'll create the battlefield
        // public   
        public Board(string name)
        {
            // get the name set up
            PlayerName = name;
            Grid = BuildGrid();

            // 1x extra spy and 5x extra privates
            for (int i = 0; i < 5; i++)
            {
                Piece piece = new Piece(0);
                Army.Add(piece);
            }
            Army.Add(new Piece(-1));
            // rest of the army, skipping 1
            for (int i = -2; i < 1; i++)
            {
                Piece piece = new Piece(i);
                Army.Add(piece);
            }
            for (int i = 2; i < 14; i++)
            {
                Piece piece = new Piece(i);
                Army.Add(piece);
            }
            // reorder the pieces so it makes more sense
            // have to do this manually unfortunately
            Piece holding = new Piece(-3);
            // flag first
            holding = Army[6];
            Army[6] = Army[0];
            Army[0] = holding;
            // spy1 2nd
            holding = Army[5];
            Army[5] = Army[1];
            Army[1] = holding;
            // spy2 3rd
            holding = Army[7];
            Army[7] = Army[2];
            Army[2] = holding;

            PiecesNotOnBoard = new List<Piece>(Army);
        }

        //methods

        /// <summary>
        /// Builds the grid for play
        /// </summary>
        /// <returns>Returns a dictionary grid of the field of play</returns>
        private Dictionary<string, Piece> BuildGrid()
        {
            Dictionary<string, Piece> grid = new Dictionary<string, Piece>();
            for (int i = 0; i < 9; i++)
            {
                // for each position in the xAxis array
                // add a number to it so it reads A1, A2, A3
                // label for the y axis 
                for (int j = 0; j < 8; j++)
                {
                    string cellName = (xPositionToLetter[i]) + (j + 1);

                    // then add it to the overall battlefield as a key
                    grid.Add(cellName, new Piece(-3));

                    // add it to the reference array
                    GridReference[(i * 8) + j] = cellName;
                }
            }
            return grid;
        }


        /// <summary>
        /// Provides the list of pieces not yet on the board
        /// </summary>
        /// <returns>String of pieces not yet on the board</returns>
        public string PiecesToPlace()
        {
            string output = $"\nThese {PiecesNotOnBoard.Count} pieces have not been placed yet:\n\n";

            for (int i = 0; i < PiecesNotOnBoard.Count; i++)
            {
                output += $"{i + 1}. {PiecesNotOnBoard[i].GetName()}\n";
            }
            return output;
        }

        /// <summary>
        /// Places the piece on the location of the board specified, checking to make sure it is blank
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public string SetupPlacePieceBoard(int piece, string location)
        {
            // check the location to see if it doesn't contain a piece other than ""
            // if no, place
            // if yes, return error

            if (Grid[location].GetName() == "Blank")
            {
                // for the grid location, if blank
                // then place that peice's not on board location there
                Grid[location] = PiecesNotOnBoard[piece - 1];

                // update the piece to say its on the board
                Grid[location].IsPlaced();

                //need to store this string before removal
                string output = $"\nYour {PiecesNotOnBoard[piece - 1].GetName()} has been placed on {location}.";
                PiecesNotOnBoard.RemoveAt(piece - 1);

                return output;
            }
            else
            {
                return "A piece already exists at this location. Please try again.";
            }

        }
        /// <summary>
        /// Randomly assigns locations to the remaining pieces not yet on the board
        /// </summary>
        /// <param name="playerToggle"></param>
        /// <returns>String message if successful, by piece</returns>
        public string SetupPiecePlacementRandomizerBoard(int playerToggle)
        {
            // shuffle the pieces not on board list
            // go through each one by one
            // get a random location based on the player number
            // make sure the locations available are blank
            // store this in an array to make sure there are no dupes
            // if the player toggle is 0, grid references are 0,1,2 x9
            // if the player toggle is 1, grid references are 5,6,7 x9

            string piecePlacementMessage = "The following pieces have randomly been placed on the board. Please adjust as necessary.\n";

            List<string> randomizedBlankLocations = new List<string>(LocationRandomizer(playerToggle));

            // then assign locations to the pieces not on the board
            // this needs to be a while
            while (PiecesNotOnBoard.Count > 0)
            {
                // go through the pieces not on board
                // find it's place in the shuffled piece's list, and return that number 

                // need a 1 because setup place piece board takes user input of actual value and changes it to zero based indexing
                piecePlacementMessage += SetupPlacePieceBoard(1, randomizedBlankLocations[0]);

                // remove from shuffledPieces and blankLocations lists so we don't have conflicts
                randomizedBlankLocations.RemoveAt(0);
            }
            return piecePlacementMessage;
        }
        /// <summary>
        /// Helper method to randomize loacations to place pieces on if player does not want to manually
        /// </summary>
        /// <param name="gridReference"></param>
        /// <param name="playerToggle"></param>
        /// <returns>List of randomized, valid, locations, for the current player</returns>
        private List<string> LocationRandomizer(int playerToggle)
        {
            // based this method on the above            
            List<string> selectedLocations = new List<string>();
            List<string> randomizedSelectedLocations = new List<string>();
            Random rnd = new Random();

            // holding numbers to start our cycle
            int i;
            int j;
            int k;

            //if player toggle is 0, want positions 0, 1, 2 and 9x multiples of it
            if (playerToggle == 0)
            {
                // pull the blank locations from the grid and put them in the list
                //pull the first 24 positions out of the grid reference
                // have to do this 3 times...
                i = 0;
                j = 1;
                k = 2;

                LocationChooserHelper(selectedLocations, i);
                LocationChooserHelper(selectedLocations, j);
                LocationChooserHelper(selectedLocations, k);
            }
            else //otherwise, want positions 5, 6, 7 and 8x multiples of it
            {
                i = 5;
                j = 6;
                k = 7;

                LocationChooserHelper(selectedLocations, i);
                LocationChooserHelper(selectedLocations, j);
                LocationChooserHelper(selectedLocations, k);
            }

            // when the selected locations list of blanks is built, randomize it 
            while (selectedLocations.Count > 0)
            {
                int index = rnd.Next(0, selectedLocations.Count); // give me a random number between 0 and the end of the list
                randomizedSelectedLocations.Add(selectedLocations[index]); //add the old list at that random position to the new list, one by one
                selectedLocations.RemoveAt(index); //remove the old list item so it's not pulled in again
            }
            return randomizedSelectedLocations;
        }
        /// <summary>
        /// Helper method for the Location Randomizer method. Obtains the list of valid positions for that player
        /// </summary>
        /// <param name="selectedLocations"></param>
        /// <param name="counter"></param>
        /// <returns>List of valid locations for that player's starting lineup.</returns>
        private List<string> LocationChooserHelper(List<string> selectedLocations, int counter)
        {
            // pull the blank locations from the grid and put them in the list
            //pull the first 27 positions out of the grid reference 
            for (int i = counter; i < 72; i += 8)
            {
                // pull the location
                string location = GridReference[i];
                // check if it's empty
                if (Grid[location].GetRank() == -3)
                {
                    //if empty, add it to the selected locations list
                    selectedLocations.Add(location);
                }
            }
            return selectedLocations;
        }

        public bool SetupChangePieceLocation(string locationChosen)
        {
            // change the status of the piece to not on board
            // put it in a holding piece just for this method
            // add it back to the board's PiecesNotOnBoard list
            // replace that current spot with a blank piece
            Grid[locationChosen].RemoveFromBoard();
            Piece holdingPiece = Grid[locationChosen];
            PiecesNotOnBoard.Add(holdingPiece);
            Grid[locationChosen] = new Piece(-3);

            //return false when done so the SetupChangePieceLocation loop breaks in UI
            return false;
        }
        /// <summary>
        /// List of deaths in a player's army
        /// </summary>
        /// <returns>String of pieces no longer alive and removed from the board</returns>
        public string ListOfDeaths()
        {
            string output = $"\nYou have lost the following {PiecesKilled.Count} pieces:\n\n";

            if (PiecesKilled.Count == 0)
            {
                return "You have not lost any pieces... yet...";
            }
            else
            {
                for (int i = 0; i < PiecesKilled.Count; i++)
                {
                    output += $"{i + 1}. {PiecesKilled[i].GetName()}\n";
                }
            }
            return output;
        }

        /// <summary>
        /// Checks whether the location has a playable (i.e. not blank) piece
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Returns back the location if the piece is playable, otherwise throws exception</returns>
        public string ConfirmLocationHasValidPiece(string location)
        {
            if (Grid[location].GetName() != "Blank")
            {
                return location;
            }
            else
            {
                throw new LocationChosenWrongException();
            }
        }

        /// <summary>
        /// Checks whether the move the player is trying to make is valid (i.e. won't send a piece off the board)
        /// Also checks it won't bump into an existing piece
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <param name="direction"></param>
        /// <returns>Returns back the future location if the move is valid, otherwise throws exception</returns>
        public string ConfirmMoveIsValid(string currentLocation, string direction)
        {
            string futureLocation = "";
            // future locations can affect the grid reference index position in the following ways:
            // Up (-1)
            // Down (+1)
            // Left (-8)
            // Right (+8)
            // have to make sure the direction results in a valid location (not off the board)
            // have to make sure up/down doesn't result in crossing rows (i.e. from row 8 moves down to next column's row 1)
            // have to make sure we don't bump into an existing peice

            // check the direction is valid first 
            switch (direction)
            {
                case "U":
                    futureLocation = MoveUpCheckHelper(currentLocation);
                    break;
                case "D":
                    futureLocation = MoveDownCheckHelper(currentLocation);
                    break;
                case "L":
                    futureLocation = MoveLeftCheckHelper(currentLocation);
                    break;
                case "R":
                    futureLocation = MoveRightCheckHelper(currentLocation);
                    break;
            }
            // check if we're bumping into another piece
            if (ConfirmLocationIsEmpty(futureLocation))
            {
                return futureLocation;
            }
            else
            {
                throw new DirectionChosenWrongException();
            }
        }

        /// <summary>
        /// Helper for ConfirmMoveIsValid method. Checks if the Up direction is a valid move
        /// </summary>
        /// <param name="currentLocation"></param> 
        /// <returns>Returns the future location coordinates if the move is valid. Otherwise, throws exception.</returns>
        private string MoveUpCheckHelper(string currentLocation)
        {
            string futureLocation = "";
            //extract the last character of the current location
            string lastCharacter = currentLocation.Substring(1);

            // if the last character is a 1, the location is already the top row
            // so only return the future location if it's not a 1
            if (lastCharacter != "1")
            {
                // find the index position of the current location
                int indexOfCurrentLocation = Array.IndexOf(GridReference, currentLocation);

                // get the future location's index by subtracting 1
                futureLocation = GridReference[indexOfCurrentLocation - 1];
            }
            else
            {
                throw new DirectionChosenWrongException();
            }
            return futureLocation;
        }

        /// <summary>
        /// Helper for ConfirmMoveIsValid method. Checks if the Down direction is a valid move
        /// </summary>
        /// <param name="currentLocation"></param> 
        /// <returns>Returns the future location coordinates if the move is valid. Otherwise, throws exception.</returns>
        private string MoveDownCheckHelper(string currentLocation)
        {
            string futureLocation = "";
            //extract the last character of the current location
            string lastCharacter = currentLocation.Substring(1);

            // if the last character is a 8, the location is already the bottom row
            // so only return the future location if it's not a 8
            if (lastCharacter != "8")
            {
                // find the index position of the current location
                int indexOfCurrentLocation = Array.IndexOf(GridReference, currentLocation);

                // get the future location's index by subtracting 1
                futureLocation = GridReference[indexOfCurrentLocation + 1];
            }
            else
            {
                throw new DirectionChosenWrongException();
            }
            return futureLocation;
        }

        /// <summary>
        /// Helper for ConfirmMoveIsValid method. Checks if the Left direction is a valid move
        /// </summary>
        /// <param name="currentLocation"></param> 
        /// <returns>Returns the future location coordinates if the move is valid. Otherwise, throws exception.</returns>
        private string MoveLeftCheckHelper(string currentLocation)
        {
            string futureLocation = "";
            // find the index position of the current location
            int indexOfCurrentLocation = Array.IndexOf(GridReference, currentLocation);

            // if lowering the index position by 8 (a column left) will still be zero and above, allow
            if ((indexOfCurrentLocation - 8) >= 0)
            {
                // get the future location's index by subtracting 1
                futureLocation = GridReference[indexOfCurrentLocation - 8];
            }
            else
            {
                throw new DirectionChosenWrongException();
            }
            return futureLocation;
        }

        /// <summary>
        /// Helper for ConfirmMoveIsValid method. Checks if the Right direction is a valid move
        /// </summary>
        /// <param name="currentLocation"></param> 
        /// <returns>Returns the future location coordinates if the move is valid. Otherwise, throws exception.</returns>
        private string MoveRightCheckHelper(string currentLocation)
        {
            string futureLocation = "";
            // find the index position of the current location
            int indexOfCurrentLocation = Array.IndexOf(GridReference, currentLocation);

            // if increasing the index position by 8 (a column left) will still be 71 and below, allow
            if ((indexOfCurrentLocation + 8) <= 71)
            {
                // get the future location's index by subtracting 1
                futureLocation = GridReference[indexOfCurrentLocation + 8];
            }
            else
            {
                throw new DirectionChosenWrongException();
            }
            return futureLocation;
        }

        /// <summary>
        /// Checks whether the location is empty (i.e. doesn't have a piece already)
        /// </summary>
        /// <param name="futureLocation"></param>
        /// <returns>Returns true if the location is empty, otherwise false</returns>
        private bool ConfirmLocationIsEmpty(string futureLocation)
        {
            return Grid[futureLocation].GetName() == "Blank";
        }

        /// <summary>
        /// Makes the move, once other methods have confirmed the move is valid
        /// Sends back the piece's name - if Flag or other
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <param name="futureLocation"></param>
        /// <returns>Returns true if the move has occurred, otherwise throws exception</returns>
        public string BoardExecuteMove(string currentLocation, string futureLocation)
        {
            // grab the piece in the current location
            Piece holdingPiece = Grid[currentLocation];

            // double check the future location is empty because why not
            if (ConfirmLocationIsEmpty(futureLocation))
            {
                // make the future location the current location's peice
                Grid[futureLocation] = holdingPiece;

                // replace the old position with a blank piece
                Grid[currentLocation] = new Piece(-3);

                if (holdingPiece.GetName() == "Flag")
                {
                    return "Flag";
                }
                return "Other"; 
            }
            else
            {
                throw new EntryWrongException();
            }
        }

        /// <summary>
        /// Gets the Flag's location
        /// </summary>
        /// <returns>Returns the flag's location.</returns>
        public string BoardCheckFlagLocation()
        {
            // container for the flag location
            string flagLocation = "";

            // find what that location is
            foreach (KeyValuePair<string, Piece> item in Grid)
            {
                if (item.Value.GetName() == "Flag")
                {
                    flagLocation = item.Key;
                }
            }
            // extract the last character and return it
            return flagLocation.Substring(1);
        }

        /// <summary>
        /// Checks the neighbours of the Flag to see if they are empty
        /// Inclusive of if the Flag is in a corner
        /// If both empty, leads to immediate victory
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Returns true if the Flag is at the other end of the board (checked in Game) and neighbours are blank.</returns>
        public string CheckFlagNeighbours(string location)
        {
            // get the index position of the right and left locations
            // they may throw out of bounds, that's okay
            Piece leftNeighbour;
            Piece rightNeighbour;

            try
            {
                string leftNeighbourKey = MoveLeftCheckHelper(location);
                leftNeighbour = Grid[leftNeighbourKey];
            }
            catch (Exception)
            {
                // return blank if there's an exception, that's okay
                leftNeighbour = new Piece(-3);
            }

            try
            {
                string rightNeighbourKey = MoveRightCheckHelper(location);
                rightNeighbour = Grid[rightNeighbourKey];
            }
            catch (Exception)
            {
                // return blank if there's an exception, that's okay
                rightNeighbour = new Piece(-3);
            }

            // both left and right neighbour pieces need to have the name blank
            if (leftNeighbour.GetName() == "Blank" && rightNeighbour.GetName() == "Blank")
            {
                return "Immediate";
            }
            return "Wait";
        }

        /// <summary>
        /// Increments the victory counter for each player.
        /// Flag needs to reach the opposite end of the board and stay alive for one turn
        /// before victory can be declared.
        /// </summary>
        /// <returns>Always returns true.</returns>
        public bool IncrementVictoryCounter()
        {
            VictoryCount++;

            return true;
        }

        /// <summary>
        /// Method that declares this team's victory
        /// Flag needs to reach the opposite end of the board and stay alive for one turn
        /// before victory can be declared.
        /// </summary>
        /// <returns>Returns true if victory is to be declared</returns>
        public bool DeclareReachedOtherSideVictory()
        {
            if (Army[0].GetLifeStatus() == true && VictoryCount == 1)
            {
                return true;
            }
            return false;
        }

        // need a ToString override so I know what I'm working with
        public override string ToString()
        {
            return $"{PlayerName}";
        }

    }
}
