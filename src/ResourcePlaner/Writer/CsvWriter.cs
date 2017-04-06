using ResourcePlaner.Model;
using ResourcePlaner.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Writer
{
    internal class CsvWriter
    {
        public CsvWriter()
        {
        }

        public string WriteSolution(Solution solution)
        {
            var costSolver = new CostSolver();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("KW,Datum,Montag,Dienstag,Mittwoch,Donnerstag,Freitag");

            foreach(var group in solution.Problem.Groups)
            {
                Mapping[] groupMappings = new Mapping[5];

                foreach(var slot in group.Slots)
                {
                    var mapping = solution.Mappings.Single(m => m.Slot == slot);
                    var slotDate = DateTime.Parse(slot.Identifier);
                    int slotIndex = (int)slotDate.DayOfWeek - 1;
                    if (slotIndex < 0 || slotIndex > 4) throw new Exception("xxx");

                    groupMappings[slotIndex] = mapping;
                }

                var values = new List<string>();

                values.Add(group.Identifier);

                var minDate = groupMappings.Where(m => m != null).Min(m => DateTime.Parse(m.Slot.Identifier));
                var maxDate = groupMappings.Where(m => m != null).Max(m => DateTime.Parse(m.Slot.Identifier));

                values.Add($"{minDate.ToString("dd.MM.yyyy")}-{maxDate.ToString("dd.MM.yyyy")}");

                for (int i=0; i<5; i++)
                {
                    if(groupMappings[i] != null)
                    {
                        values.Add(groupMappings[i].Resource.Identifier);
                    }
                    else
                    {
                        values.Add("-");
                    }
                }

                sb.AppendLine(String.Join(",", values));
            }

            //sb.AppendLine("Solution:");

            //foreach (var slot in solution.Problem.Slots)
            //{
            //    var mappings = solution.Mappings.Where(m => m.Slot == slot);

            //    sb.Append(String.Format("'{0}' -> ", slot.Identifier));

            //    if (mappings.Count() == 1)
            //    {
            //        sb.AppendLine(String.Format("'{0}'", mappings.First().Resource.Identifier));
            //    }
            //    else
            //    {
            //        sb.AppendLine("Invalid");
            //    }
            //}

            //foreach (var resource in solution.Problem.Resources)
            //{
            //    var mappings = solution.Mappings.Where(m => m.Resource == resource);
            //    var cost = costSolver.CalculateResourceCost(solution, resource);

            //    sb.AppendLine(String.Format("Resource '{0}' -> {1} mappings, {2} cost", resource.Identifier, mappings.Count(), cost));
            //}

            return sb.ToString();
        }



    }
}
