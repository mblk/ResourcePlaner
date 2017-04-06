using ResourcePlaner.Interfaces;
using ResourcePlaner.Model;
using ResourcePlaner.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Solver.Random
{
    internal class RandomSolver : ISolver
    {
        private readonly System.Random _random = new System.Random();

        public RandomSolver(Problem problem)
        {
            this.Problem = problem;
        }

        public Problem Problem { get; private set; }

        private Solution CreateRandomSolution()
        {
            var solution = new Solution(this.Problem);

            Resource prevResource = null;

            foreach(var slot in this.Problem.Slots)
            {
                int count = slot.AvailableResources.Count();
                

                Resource resource = null;

                for(int t=0; t<10; t++)
                {
                    int index = _random.Next() % count;

                    resource = slot.AvailableResources.ElementAt(index);

                    if (resource != prevResource)
                    {
                        break;
                    }
                }
                prevResource = resource;

                var mapping = new Mapping(slot, resource);

                solution.Mappings.Add(mapping);
            }

            return solution;
        }

        public Solution Solve()
        {
            var costSolver = new CostSolver();
            var solutionWriter = new SolutionWriter();
            var csvWriter = new CsvWriter();
            var csvFile = new FileInfo("solution.csv");

            double minCost = 0;
            Solution bestSolution = null;

            while(true)
            {
                var solution = this.CreateRandomSolution();
                var solutionCost = costSolver.CalculateCost(solution);

                if(bestSolution == null || solutionCost < minCost)
                {
                    minCost = solutionCost;
                    bestSolution = solution;

                    Console.WriteLine("new min cost: " + minCost);

                    Console.WriteLine(solutionWriter.WriteSolution(bestSolution));

                    File.WriteAllText(csvFile.FullName, csvWriter.WriteSolution(bestSolution));
                }
            }

            return null;
        }
    }
}
