using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(160, 60);
            Console.WriteLine("ResourcePlaner");

            string problemPath = @"..\..\..\problem1.xml";
            string problemData = File.ReadAllText(problemPath);




            var parser = new Parser.ProblemParser();

            var problem = parser.Parse(problemData);

            var solver = new Solver.ProblemSolver(problem);

            var solution = solver.Solve();



            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
