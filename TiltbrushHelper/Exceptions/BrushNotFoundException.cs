using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiltbrushToolkit.Exceptions
{
    public class BrushNotFoundException : Exception
    {
        public BrushNotFoundException(string message) : base(message)
        {

        }
    }
}
