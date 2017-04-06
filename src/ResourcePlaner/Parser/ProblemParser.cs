using ResourcePlaner.Interfaces;
using ResourcePlaner.Model;
using ResourcePlaner.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace ResourcePlaner.Parser
{
    public class ProblemParser
    {
        public ProblemParser()
        {

        }

        public IProblem Parse(string data)
        {
            var problem = new Problem();

            var slotDict = new Dictionary<string, Slot>();
            var constraintDict = new Dictionary<string, Constraint>();

            var xmlDocument = XDocument.Parse(data);
            var xmlProblem = xmlDocument.Element("problem");

            var xmlGroups = xmlProblem.Element("groups");
            var xmlConstraints = xmlProblem.Element("constraints");
            var xmlResources = xmlProblem.Element("resources");

            //
            // parse groups & slots
            //
            var xmlAutofill = xmlGroups.Element("autofill");
            if (xmlAutofill != null)
            {
                var weekGroupDict = new Dictionary<int, Group>(); // WeekOfYear -> Group
                var costModifierDict = new Dictionary<int, double>(); // DayOfWeek -> Cost

                var first = DateTime.Parse(xmlAutofill.Attribute("first").Value);
                var last = DateTime.Parse(xmlAutofill.Attribute("last").Value);

                var xmlAutofillCosts = xmlAutofill.Element("costs");

                var defaultCost = Double.Parse(xmlAutofillCosts.Attribute("default").Value);

                foreach (var xmlAutofillCost in xmlAutofillCosts.Elements("cost"))
                {
                    var index = Int32.Parse(xmlAutofillCost.Attribute("index").Value); // 1=Monday ... 5=Friday
                    var cost = Double.Parse(xmlAutofillCost.Attribute("cost").Value, CultureInfo.InvariantCulture);

                    if (index < 1 || index > 5) throw new Exception($"invalid index {index}");
                    if (cost <= 0) throw new Exception($"invalid cost {cost}");

                    costModifierDict[index] = cost;
                }

                // iterate through days
                for(var currentDay = first; currentDay <= last; currentDay = currentDay.AddDays(1))
                {
                    if (!WorkDayChecker.IsWorkday(currentDay)) continue;

                    var dayOfWeek = (int)currentDay.DayOfWeek;
                    var dayOfYear = currentDay.DayOfYear - 1;
                    var weekOfYear = (dayOfYear / 7) + 1;

                    // get or create group
                    Group group = null;
                    if (weekGroupDict.ContainsKey(weekOfYear))
                    {
                        group = weekGroupDict[weekOfYear];
                    }
                    else
                    {
                        group = new Group("Week " + weekOfYear);
                        problem.Groups.Add(group);
                        weekGroupDict[weekOfYear] = group;
                    }

                    // determine cost
                    double cost = defaultCost;

                    if (costModifierDict.ContainsKey(dayOfWeek))
                    {
                        cost = costModifierDict[dayOfWeek];
                    }

                    // create slot
                    Slot slot = new Slot(currentDay.ToString("dd.MM.yyyy"), cost, group);
                    group.Slots.Add(slot);
                    problem.Slots.Add(slot);
                    slotDict.Add(slot.Identifier, slot);
                }
            }

            foreach (var xmlGroup in xmlGroups.Elements("group"))
            {
                string groupIdentifier = xmlGroup.Attribute("id").Value;

                if (problem.Groups.Any(g => g.Identifier == groupIdentifier))
                {
                    throw new Exception(String.Format("duplicate group '{0}'", groupIdentifier));
                }

                var group = new Group(groupIdentifier);

                var xmlSlots = xmlGroup.Element("slots");

                foreach (var xmlSlot in xmlSlots.Elements("slot"))
                {
                    string slotIdentifier = xmlSlot.Attribute("id").Value;
                    string slotCost = xmlSlot.Attribute("cost").Value;

                    if (problem.Slots.Any(s => s.Identifier == slotIdentifier))
                    {
                        throw new Exception(String.Format("duplicate slot '{0}'", slotIdentifier));
                    }

                    var slot = new Slot(slotIdentifier, Double.Parse(slotCost), group);

                    group.Slots.Add(slot);
                    problem.Slots.Add(slot);
                    slotDict.Add(slotIdentifier, slot);
                }

                problem.Groups.Add(group);
            }

            // link Slots
            var sortedSlots = problem.Slots.OrderBy(s => DateTime.Parse(s.Identifier));

            for(int i=0; i<sortedSlots.Count() - 1; i++)
            {
                var slot1 = sortedSlots.ElementAt(i);
                var slot2 = sortedSlots.ElementAt(i + 1);

                slot1.Next = slot2;
                slot2.Prev = slot1;
            }

            //
            // parse constraints
            //
            foreach (var xmlConstraint in xmlConstraints.Elements("constraint"))
            {
                string constraintIdentifier = xmlConstraint.Attribute("id").Value;

                if(problem.Constraints.Any(c => c.Identifier == constraintIdentifier))
                {
                    throw new Exception(String.Format("duplicate constraint '{0}'", constraintIdentifier));
                }

                var constraint = new Constraint(constraintIdentifier);

                var xmlRestrictions = xmlConstraint.Element("restrictions");

                var xmlRestrictionAutofill = xmlRestrictions.Element("autofill");

                if (xmlRestrictionAutofill != null)
                {
                    var first = DateTime.Parse(xmlRestrictionAutofill.Attribute("first").Value);
                    var last = DateTime.Parse(xmlRestrictionAutofill.Attribute("last").Value);

                    for(var currentDay = first; currentDay <= last; currentDay = currentDay.AddDays(1))
                    {
                        if (!WorkDayChecker.IsWorkday(currentDay)) continue;

                        var slotIdentifier = currentDay.ToString("dd.MM.yyyy");

                        if (slotDict.ContainsKey(slotIdentifier))
                        {
                            var restriction = new SlotRestriction(slotDict[slotIdentifier]);

                            constraint.Restrictions.Add(restriction);
                        }
                        else
                        {
                            Debug.WriteLine($"invalid slot identifier {slotIdentifier} for autofill constraint {constraintIdentifier}");
                        }
                    }
                }

                foreach(var xmlRestriction in xmlRestrictions.Elements("slotRestriction"))
                {
                    string slotIdentifier = xmlRestriction.Attribute("slot").Value;

                    if (slotDict.ContainsKey(slotIdentifier))
                    {
                        var restriction = new SlotRestriction(slotDict[slotIdentifier]);

                        constraint.Restrictions.Add(restriction);
                    }
                    else
                    {
                        Debug.WriteLine($"invalid slot identifier {slotIdentifier} for constraint {constraintIdentifier}");
                    }

                }

                problem.Constraints.Add(constraint);
                constraintDict.Add(constraintIdentifier, constraint);
            }

            //
            // parse resources
            //
            foreach(var xmlResource in xmlResources.Elements("resource"))
            {
                string resourceIdentifier = xmlResource.Attribute("id").Value;

                if(problem.Resources.Any(r => r.Identifier == resourceIdentifier))
                {
                    throw new Exception(String.Format("duplicate resource '{0}'", resourceIdentifier));
                }

                var xmlResourceConstraints = xmlResource.Element("constraints");

                var resourceConstraints = new List<Constraint>();

                foreach (var xmlResourceConstraint in xmlResourceConstraints.Elements("constraint"))
                {
                    string resourceConstraintIdentifier = xmlResourceConstraint.Attribute("id").Value;

                    var constraint = problem.Constraints.Where(c => c.Identifier == resourceConstraintIdentifier).FirstOrDefault();

                    if (constraint != null)
                    {
                        if(resourceConstraints.Contains(constraint))
                        {
                            throw new Exception(String.Format("duplicate constraint '{0}' for resource '{1}'", resourceConstraintIdentifier, resourceIdentifier));
                        }

                        resourceConstraints.Add(constraint);
                    }
                    else
                    {
                        Debug.WriteLine($"invalid constraint {resourceConstraintIdentifier} on resource {resourceIdentifier}");
                    }
                }

                var resource = new Resource(resourceIdentifier, resourceConstraints);

                problem.Resources.Add(resource);
            }

            return problem;
        }
    }
}
