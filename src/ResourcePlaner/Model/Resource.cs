using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class Resource
    {
        private readonly List<Constraint> _constraints = new List<Constraint>();

        public Resource(string identifier, IEnumerable<Constraint> constraints)
        {
            this.Identifier = identifier;

            if(constraints != null)
            {
                foreach(var constraint in constraints)
                {
                    this.Constraints.Add(constraint);
                }
            }
        }

        public string Identifier { get; private set; }

        public IList<Constraint> Constraints
        {
            get
            {
                return _constraints;
            }
        }

        public override string ToString()
        {
            return "Resource: " + this.Identifier;
        }
    }
}
