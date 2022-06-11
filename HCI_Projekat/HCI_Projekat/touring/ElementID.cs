using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.touring
{
    public static class ElementID
    {
        // Timetable
        public static readonly string ComboBoxFrom = "ComboBoxFrom";
        public static readonly string ComboBoxTo = "ComboBoxTo";
        public static readonly string RadioWeekend = "RadioWeekend";
        public static readonly string ButtonSearch = "ButtonSearch";

        // TrainLine
        public static readonly string TrainLineTable = "TrainLineTable";
        public static readonly string TrainLineButtonAdd = "TrainLineButtonAdd";
        public static readonly string TrainLineButtonEdit = "TrainLineButtonEdit";
        public static readonly string TrainLineButtonDelete = "TrainLineButtonDelete";
        public static readonly string PinStart = "PinStart";
        public static readonly string PinEnd = "PinEnd";
        public static readonly string Pin = "Pin";
        public static readonly string AddTrainLineButtonSave = "AddTrainLineButtonSave";


        // Train
        public static readonly string TrainButtonAdd = "TrainButtonAdd";

        public static readonly string TrainName = "TrainName";
        public static readonly string TrainRang = "TrainRang";
        public static readonly string TrainButtonNext = "TrainButtonNext";


        // Wagons
        public static readonly string WagonButtonAdd = "WagonButtonAdd";
        public static readonly string WagonCapacity = "WagonCapacity";
        public static readonly string WagonClass = "WagonClass";
        public static readonly string ButtonFinish = "ButtonFinish";

    }
}
