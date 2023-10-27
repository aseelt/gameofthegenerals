using Generals.Classes;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Generals
{
    public class Program
    {
        static void Main(string[] args)
        {
            // start the game
            UI game = new UI();
            game.InitializeUIRuntime();

            // ui handles the user text to/from
            // ui calls the actions class, that stores the board, the move action etc
            // the actions calls the battlefield or the piece and they update themselves
            //  action toggles between battlefields

            // piece class is done

            
            

            



            
        }
        
    }
}