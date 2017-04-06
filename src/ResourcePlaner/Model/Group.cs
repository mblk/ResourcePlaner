using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class Group
    {
        private readonly List<Slot> _slots = new List<Slot>();

        public Group(string identifier)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; private set; }

        public IList<Slot> Slots
        {
            get
            {
                return _slots;
            }
        }

        public override string ToString()
        {
            return "Group: " + this.Identifier;
        }
    }
}
