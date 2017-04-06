using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlaner.Utils
{
    internal static class WorkDayChecker
    {
        private class NoWorkDay
        {
            public string Name { get; set; }

            public DateTime Date { get; set; }

            public override string ToString()
            {
                return $"{Date.ToString("dd.MM.yyyy")} {Name}";
            }
        }

        private static IList<NoWorkDay> _noWorkDays = new List<NoWorkDay>();

        static WorkDayChecker()
        {
            Populate();
        }

        private static DateTime GetOsterSonntag(int year)
        {
            // https://de.wikipedia.org/wiki/Gau%C3%9Fsche_Osterformel#Eine_erg.C3.A4nzte_Osterformel

            int g, h, c, j, l, i;

            g = year % 19;
            c = year / 100;
            h = ((c - (c / 4)) - (((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
            i = h - (h / 28) * (1 - (29 / (h + 1)) * ((21 - g) / 11));
            j = (year + (year / 4) + i + 2 - c + (c / 4)) % 7;

            l = i - j;
            int month = (int)(3 + ((l + 40) / 44));
            int day = (int)(l + 28 - 31 * (month / 4));

            return new DateTime(year, month, day);
        }

        private static void Populate()
        {
            _noWorkDays.Clear();

            int year = DateTime.Now.Year;

            AddNoWorkDay(new DateTime(year, 1, 1), "Neujahr");
            AddNoWorkDay(new DateTime(year, 1, 6), "Heilige Drei Könige");
            AddNoWorkDay(new DateTime(year, 5, 1), "Tag der Arbeit");
            AddNoWorkDay(new DateTime(year, 8, 15), "Mariä Himmelfahrt");
            AddNoWorkDay(new DateTime(year, 10, 3), "Tag der dt. Einheit");
            AddNoWorkDay(new DateTime(year, 10, 31), "Reformationstag");
            AddNoWorkDay(new DateTime(year, 11, 1), "Allerheiligen ");
            AddNoWorkDay(new DateTime(year, 12, 25), "1. Weihnachtstag");
            AddNoWorkDay(new DateTime(year, 12, 26), "2. Weihnachtstag");

            DateTime osterSonntag = GetOsterSonntag(year);

            AddNoWorkDay(osterSonntag, "Ostersonntag");
            AddNoWorkDay(osterSonntag.AddDays(-3), "Gründonnerstag");
            AddNoWorkDay(osterSonntag.AddDays(-2), "Karfreitag");
            AddNoWorkDay(osterSonntag.AddDays(1), "Ostermontag");
            AddNoWorkDay(osterSonntag.AddDays(39), "Christi Himmelfahrt");
            AddNoWorkDay(osterSonntag.AddDays(49), "Pfingstsonntag");
            AddNoWorkDay(osterSonntag.AddDays(50), "Pfingstmontag");
            AddNoWorkDay(osterSonntag.AddDays(60), "Fronleichnam");
        }

        private static void AddNoWorkDay(DateTime date, string name)
        {
            if (_noWorkDays.Any(d => d.Date.Year == date.Year && d.Date.DayOfYear == date.DayOfYear))
            {
                throw new Exception("internal error in WorkDayChecker");
            }

            _noWorkDays.Add(new NoWorkDay
            {
                Name = name,
                Date = date
            });
        }

        private static bool IsNoWorkDay(DateTime date)
        {
            var noWorkDay = _noWorkDays.SingleOrDefault(d => d.Date.Year == date.Year && d.Date.DayOfYear == date.DayOfYear);

            return (noWorkDay != null);
        }


        public static bool IsWorkday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday) return false;
            if (date.DayOfWeek == DayOfWeek.Sunday) return false;

            if (IsNoWorkDay(date)) return false;

            // ...

            return true;
        }
    }
}
