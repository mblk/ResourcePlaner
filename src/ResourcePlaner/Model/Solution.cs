using ResourcePlaner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Model
{
    internal class Solution : ISolution
    {
        private readonly List<Mapping> _mappings = new List<Mapping>();

        public Solution(Problem problem)
        {
            this.Problem = problem;
        }

        public Problem Problem { get; private set; }

        public IList<Mapping> Mappings
        {
            get
            {
                return _mappings;
            }
        }
    }
}
