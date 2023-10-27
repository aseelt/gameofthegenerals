using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals.Classes
{
    public class Piece
    {
        // properties
        // lets get them on the page first
        // then lets figure out what needs to be get, set, private etc.

        // rank should only be created, it can't be modified after creation
        // we'll need rank for combat, so leave get
        // rank needs to be public for the board to calculate combat
        // but only return the rank on ask, don't want it to be peeked at
        private int Rank { get; }

        // can derive name using a field
        // no need for set or backing variable
        // supply it when asked, but otherwise hidden
        // since it's derived from a dictionary field, simple get works
        // get just returns the dictionary acces

        private string Name
        {
            get
            {
                return RankToName[Rank];
            }
        }

        // need a short name to display in the grid
        // private, return only if asked
        private string DisplayName
        {
            get
            {
                return RankToDisplayName[Rank];
            }
        }

        // need a hidden name to display in the grid so the opposing player can't see it
        // no set, it'll never change
        // lets make it private, why not, and return with a method
        private string HiddenName
        {
            get
            {
                if(Rank == -3)
                {
                    return "       ";
                }
                else
                {
                    return " oo oo ";
                }
            }
        }


        // like Name, it's derived based on the Rank
        // like Name, it can't be changed
        // definitely needs to be private, want to hide how strong a piece is
        // the board will need it to calculate combat 
        public int ForceMultiplier
        {
            get
            {
                if (Rank == -1) // rank of spy, most restrictive
                {
                    return -1;
                }
                else // everything else should be a 1
                {
                    return 1;
                }

            }
        }

        // definitely private
        // call using a method when needed for combat
        // will need to change when peices die
        // initial value is true, all pieces are created alive 
        private bool isAlive { get; set; } = true;

        // will need to be public so the board knows if the piece is on the board or not
        // will need to change when peices die
        // initial value is false, all pieces need to be placed
        // then change to true if the piece has been placed
        // if killed, piece needs to be removed from board, ineligible to be moved
        // private, can't edit it directly, public otherwise since other classes need the info
        //TODO need list of eligible alive pieces
        public bool isOnBoard { get; private set; } = false;

        // owning army property will be in the Army class

        // make it private, should only be able to move the piece through an action not set it directly
        // will need to be settable because it'll move
        // no need to put default values, player will place them 
        private string Position { get; set; } = "";

        // constructors
        // pieces must be built with these
        // really all i need is the rank to create the pieces
        // put in some error checking
        // in the team, automate the process of creating the rank
        // so no user can input values and make a custom deck
        public Piece(int rank)
        {
            if (rank >= -3 && rank <= 13) // only create if accurate value supplied for ranks 5* to flag
            {
                Rank = rank;
            }
        }

        // methods
        // gives the rank when asked
        public int GetRank()
        {
            return Rank;
        }
        // gives the full name
        public string GetName()
        {
            return Name;
        }
        // gives the short display name
        public string GetShortDisplayName()
        {
            return DisplayName;
        }
        // gives the display name
        public string GetHiddenName()
        {
            return HiddenName;
        }
        // gives the force multiplier
        public int GetForceMultiplier()
        {
            return ForceMultiplier;
        }
        // get living status
        public bool GetLifeStatus()
        {
            return isAlive;
        }
        // kill the piece when it loses!
        // also add remove from board
        public bool KillPiece()
        {
            isAlive = false;
            RemoveFromBoard();
            return isAlive;
        }
        // on board status change
        public bool IsPlaced()
        {
            isOnBoard = true;
            return isOnBoard;
        }
        public bool RemoveFromBoard()
        {
            isOnBoard = false;
            return isOnBoard;
        }



        // create dictionary field of Rank providing Name
        // public static, the army class needs it. Though... how do I avoid static?
        //TODO do I need a critical value for the flag? A child class for it? or should I add that to a rules class
        public Dictionary<int, string> RankToName = new Dictionary<int, string>()
        {
            { -3, "Blank" },
            { -2, "Flag" },
            { -1, "Spy" },
            { 0, "Private" },
            { 2, "Sergeant" },
            { 3, "Second Lieutenant" },
            { 4, "First Lieutenant" },
            { 5, "Captain" },
            { 6, "Major" },
            { 7, "Lieutenant Colonel" },
            { 8, "Colonel" },
            { 9, "* General" },
            { 10, "** General" },
            { 11, "*** General" },
            { 12, "**** General" },
            { 13, "***** General" }
        };

        public Dictionary<int, string> RankToDisplayName = new Dictionary<int, string>()
        {
            { -3, "       " },
            { -2, " Flag! " },
            { -1, "  Spy  " },
            { 0, "  Pte  " },
            { 2, " Sarge " },
            { 3, " 2. Lt " },
            { 4, " 1. Lt " },
            { 5, "  Cap  " },
            { 6, " Major " },
            { 7, " L.Col " },
            { 8, "  Col  " },
            { 9, "   *   " },
            { 10, "  * *  " },
            { 11, "  ***  " },
            { 12, " ** ** " },
            { 13, " ***** " }
        };

        // need a ToString override so I know what I'm working with
        public override string ToString()
        {
            return $"{Name}";
        }

    }
}
