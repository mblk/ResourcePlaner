using ResourcePlaner.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Solver
{
    internal class CostSolver
    {
        public CostSolver()
        {

        }

        private bool CheckSolution(Solution solution)
        {
            foreach(var slot in solution.Problem.Slots)
            {
                var mappings = solution.Mappings.Where(m => m.Slot == slot);

                if(mappings.Count() != 1)
                {
                    return false;
                }
            }

            return true;
        }

        public double CalculateResourceCost(Solution solution, Resource resource)
        {
            var mappings = solution.Mappings.Where(m => m.Resource == resource);

            double resourceCost = 0;

            foreach (var mapping in mappings)
            {
                resourceCost += mapping.Slot.Cost * 1d;

                if (mapping.Slot.Next != null)
                {
                    var mappingInRow = solution.Mappings.Where(m => m.Slot == mapping.Slot.Next && m.Resource == resource).FirstOrDefault();
                    if (mappingInRow != null)
                    {
                        resourceCost += 5d;
                    }
                }
            }

            //foreach (var group in solution.Problem.Groups)
            //{
            //    int mappingsInGroup = solution.Mappings.Where(m => m.Resource == resource).Count(m => m.Slot.Group == group);

            //    resourceCost += mappingsInGroup * 0.1d;
            //}

            return resourceCost;
        }

        public double CalculateCost(Solution solution)
        {
            // filter out invalid solutions
            if(this.CheckSolution(solution) == false)
            {
                Debug.WriteLine("found invalid solution");
                return 9999999d;
            }

            // calculate per resource cost
            Dictionary<Resource, double> resourceCostDict = new Dictionary<Resource, double>();

            foreach (var resource in solution.Problem.Resources)
            {
                double resourceCost = CalculateResourceCost(solution, resource);

                resourceCostDict.Add(resource, resourceCost);
            }

            // gather statistics
            double minResourceCost = resourceCostDict.Min(p => p.Value);
            double maxResourceCost = resourceCostDict.Max(p => p.Value);

            // calculate solution cost
            double solutionCost = 0;

            solutionCost += (maxResourceCost - minResourceCost);

            //Debug.WriteLine("solutionCost: " + solutionCost);
            return solutionCost;
        }
    }
}
