using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class Constraint
    {
        private readonly List<Restriction> _restrictions = new List<Restriction>();

        public Constraint(string identifier)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; private set; }

        public IList<Restriction> Restrictions
        {
            get
            {
                return _restrictions;
            }
        }

        public override string ToString()
        {
            return "Constraint: " + this.Identifier;
        }
    }
}
