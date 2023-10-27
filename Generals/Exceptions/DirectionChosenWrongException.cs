using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals.Exceptions
{
    public class DirectionChosenWrongException : Exception
    {
        public DirectionChosenWrongException() : base("You can't move that peice in that direction.")
        {
        }
    }
}
