using ResourcePlaner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Solver
{
    internal class AvailableResourceSolver
    {
        public AvailableResourceSolver(Problem problem)
        {
            this.Problem = problem;
        }

        public Problem Problem { get; private set; }

        private IEnumerable<Resource> FindResourcesForSlot(Slot slot)
        {
            //return this.Problem.Resources.Where(r => r.Constraint == null || r.Constraint.Restrictions.Any(c => c.AppliesToSlot(slot)) == false);

            return this.Problem.Resources.Where(res => res.Constraints.Any(con => con.Restrictions.Any(r => r.AppliesToSlot(slot))) == false);

        }

        public void FindAvailableResources()
        {
            foreach(var group in this.Problem.Groups)
            {
                foreach(var slot in group.Slots)
                {
                    var availableResources = this.FindResourcesForSlot(slot);

                    foreach(var availableResource in availableResources)
                    {
                        slot.AvailableResources.Add(availableResource);
                    }
                }
            }
        }
    }
}
