using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals.Exceptions
{
    public class EntryWrongException : Exception
    {
        public EntryWrongException() : base("You've entered an incorrect value.")
        {
        }
    }
}
