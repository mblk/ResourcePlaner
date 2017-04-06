using ResourcePlaner.Interfaces;
using ResourcePlaner.Model;
using ResourcePlaner.Solver.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Solver
{
    public class ProblemSolver
    {
        private readonly Problem _problem;

        public ProblemSolver(IProblem problem)
        {
            if(!(problem is Problem))
            {
                throw new ArgumentException("problem");
            }

            _problem = problem as Problem;
        }

        public ISolution Solve()
        {
            AvailableResourceSolver availableResourceSolver = new AvailableResourceSolver(_problem);
            availableResourceSolver.FindAvailableResources();

            RandomSolver solver = new RandomSolver(_problem);

            var solution = solver.Solve();



            return null;
        }
    }
}
