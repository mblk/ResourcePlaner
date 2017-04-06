using ResourcePlaner.Model;
using ResourcePlaner.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Writer
{
    internal class SolutionWriter
    {
        public SolutionWriter()
        {
        }

        public string WriteSolution(Solution solution)
        {
            var costSolver = new CostSolver();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Solution:");

            foreach(var slot in solution.Problem.Slots)
            {
                var mappings = solution.Mappings.Where(m => m.Slot == slot);

                sb.Append(String.Format("'{0}' -> ", slot.Identifier));

                if (mappings.Count() == 1)
                {
                    sb.AppendLine(String.Format("'{0}'", mappings.First().Resource.Identifier));
                }
                else
                {
                    sb.AppendLine("Invalid");
                }
            }

            foreach(var resource in solution.Problem.Resources)
            {
                var mappings = solution.Mappings.Where(m => m.Resource == resource);
                var cost = costSolver.CalculateResourceCost(solution, resource);

                sb.AppendLine(String.Format("Resource '{0}' -> {1} mappings, {2} cost", resource.Identifier, mappings.Count(), cost));
            }

            return sb.ToString();
        }
    }
}
