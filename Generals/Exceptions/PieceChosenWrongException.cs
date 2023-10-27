using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals.Exceptions
{
    public class PieceChosenWrongException : Exception
    {
        public PieceChosenWrongException() : base("Please retry to select a piece.")
        {
        }
    }
}
