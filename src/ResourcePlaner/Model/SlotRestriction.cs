using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class SlotRestriction : Restriction
    {
        public SlotRestriction(Slot slot)
        {
            this.Slot = slot;
        }

        public Slot Slot { get; private set; }

        public override bool AppliesToSlot(Slot slot)
        {
            return (slot == this.Slot);
        }
    }
}
