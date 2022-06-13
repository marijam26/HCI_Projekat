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
                    new Step(ElementID.ComboBoxFrom, "Choose start station", "Choose \"Novi Sad\". "),
                    new Step(ElementID.ComboBoxTo, "Choose end station", "Choose \"Beograd\". "),
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


        public static void StartTrainTour()
        {
            var tour = new Tour
            {
                Name = "Train tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.TrainButtonAdd, "Add a new train", "Click to add a new train"),

                }
            };

            tour.Start();
        }

        public static void StartAddTrainTour()
        {
            var tour = new Tour
            {
                Name = "Train add tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.TrainName, "Choose a train name", "Try entering \"Soko5\""),
                    new Step(ElementID.TrainRang, "Choose a train rang", "Choose the \"soko\" rang."),
                    new Step(ElementID.TrainButtonAdd, "Next", "Click the button to go to the next step. "),
                }
            };

            tour.Start();
        }

        public static void StartAddWagonTour()
        {
            var tour = new Tour
            {
                Name = "Wagon add tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.WagonCapacity, "Choose the wagon capacity", "Try entering \"40\""),
                    new Step(ElementID.WagonClass, "Choose the wagon class", "Check the option \"First\""),
                    new Step(ElementID.WagonButtonAdd, "Add the wagon", "Click the button to add a new wagon"),
                    new Step(ElementID.ButtonFinish, "Finish", "Click the button to finally add the train. "),

                }
            };

            tour.Start();
        }

        public static void StartTicketTour()
        {
            var tour = new Tour
            {
                Name = "Ticket tour",
                ShowNextButtonDefault = false,
                Steps = new[]
                {
                    new Step(ElementID.TicketFrom, "Choose start station", "Choose \"Novi Sad\". "),
                    new Step(ElementID.TicketTo, "Choose end station", "Choose \"Beograd\". "),
                    new Step(ElementID.TicketDate, "Choose the date", "Choose a date for the ride. Try \"20-Jun-2022\""),
                    new Step(ElementID.TicketSearch, "Search", "Click the button to search for departures. "),
                    new Step(ElementID.ChooseSeat, "Choose the seat", "Click the button to continue."),
                    new Step(ElementID.Wagon1, "Choose the wagon", "Click the button to choose the wagon."),
                    new Step(ElementID.Seat1, "Choose the seat", "Click the button to choose the seat."),
                    new Step(ElementID.BuyTicketButton, "Buy", "Click the button to buy the ticket."),
                    new Step(ElementID.ConfirmPurchase, "Confirm", "Click the button to confirm the purchase."),


                }
            };

            tour.Start();
        }

    }
}
