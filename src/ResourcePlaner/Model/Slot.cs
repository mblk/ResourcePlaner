using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class Slot
    {
        private readonly List<Resource> _availableResources = new List<Resource>();

        public Slot(string identifier, double cost, Group group)
        {
            this.Identifier = identifier;
            this.Cost = cost;
            this.Group = group;
        }

        public string Identifier { get; private set; }
        public double Cost { get; private set; }
        public Group Group { get; private set; }

        public Slot Prev { get; set; }
        public Slot Next { get; set; }

        public IList<Resource> AvailableResources
        {
            get
            {
                return _availableResources;
            }
        }

        public override string ToString()
        {
            return "Slot: " + this.Identifier;
        }
    }
}
