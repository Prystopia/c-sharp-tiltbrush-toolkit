using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiltbrushToolkit.Attributes
{
    public class BrushIdAttribute : Attribute
    {
        public Guid BrushGuid { get; private set; }
        public BrushIdAttribute(string brushGuid)
        {
            BrushGuid = Guid.Parse(brushGuid);
        }
    }
}
