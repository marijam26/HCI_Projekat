using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    [Serializable]
    public class Timetable:ICloneable
    {
        public int id { get; set; }

        public TrainLine line { get; set; }

        public DateTime start { get; set; }

        public DateTime ValidFrom { get; set; }    // od kada vazi red voznje

        public DateTime ValidTo { get; set; }      // do kada vazi


        public Boolean isWeekday { get; set; }

        public Train train { get; set; }


        public Timetable() { }

        public Timetable(int id, TrainLine line, DateTime start, Train train, Boolean isWeekday, DateTime validFrom, DateTime validTo)
        {
            this.id = id;
            this.line = line;
            this.start = start;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
            this.isWeekday = isWeekday;
            this.train = train;
        }
        public object Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (this.GetType().IsSerializable)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    return formatter.Deserialize(stream);
                }
                return null;
            }
        }
    }
}
