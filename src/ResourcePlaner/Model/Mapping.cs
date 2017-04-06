using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class Mapping
    {
        public Mapping(Slot slot, Resource resource)
        {
            this.Slot = slot;
            this.Resource = resource;
        }

        public Slot Slot { get; private set; }
        public Resource Resource { get; private set; }
    }
}
