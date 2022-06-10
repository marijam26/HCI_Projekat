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

        public static void StartTrainLineTour()
        {
            var tour = new Tour
            {
                Name = "Train Line tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.TrainLineButtonAdd, "Add a new train line", "Click to add a new train line "),
                   
                }
            };

            tour.Start();
        }

        public static void StartAddTrainLineTour()
        {
            var tour = new Tour
            {
                Name = "Train Line add tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.PinStart, "Choose start station", "Take this pin and drag it to the map where you want to put the start station."),
                    new Step(ElementID.PinEnd, "Choose end station", "Take this pin and drag it to the map where you want to put the end station."),
                    new Step(ElementID.Pin, "Add station", "Drag the pin to the map to add a new station. "),
                    new Step(ElementID.AddTrainLineButtonSave, "Save", "Click the button to add the line."),
                }
            };

            tour.Start();
        }


    }
}
