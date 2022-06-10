using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FeatureTouring.Models;

namespace HCI_Projekat.touring
{
    internal static class TourStarter
    {

        public static void StartTimetableTour()
        {
            var tour = new Tour
            {
                Name = "Timetable tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.ComboBoxFrom, "Choose start station", "Choose \"Beograd\". "),
                    new Step(ElementID.ComboBoxTo, "Choose end station", "Choose \"Novi Sad\". "),
                    new Step(ElementID.RadioWeekend, "Choose departure day", "Choose \"Weekend\". "),
                    new Step(ElementID.ButtonSearch, "Search", "Press the button to search in the timetable."),
                }
            };

            tour.Start();
        }
    }
}
