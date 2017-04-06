using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal abstract class Restriction
    {
        public abstract bool AppliesToSlot(Slot slot);
    }
}
