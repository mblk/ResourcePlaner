using ResourcePlaner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class Problem : IProblem
    {
        private readonly List<Group> _groups = new List<Group>();
        private readonly List<Slot> _slots = new List<Slot>();
        private readonly List<Resource> _resources = new List<Resource>();
        private readonly List<Constraint> _constraints = new List<Constraint>();

        public Problem()
        {

        }

        public IList<Group> Groups
        {
            get
            {
                return _groups;
            }
        }

        public IList<Slot> Slots
        {
            get
            {
                return _slots;
            }
        }

        public IList<Resource> Resources
        {
            get
            {
                return _resources;
            }
        }

        public IList<Constraint> Constraints
        {
            get
            {
                return _constraints;
            }
        }
    }
}
