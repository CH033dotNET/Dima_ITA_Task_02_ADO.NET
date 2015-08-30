using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApplication5
{
    public class Position
    {
        [XmlElement("latitude")]
        public long Latitude { get; set; }

        [XmlElement("longitude")]
        public long Longitude { get; set; }

        [XmlElement("accuracy")]
        public int Accuracy { get; set; }


        [XmlElement("time")]
        public DateTime Time { get; set; }

        public Position() { }

        public Position(long latitude, long longitude, int accuracy, DateTime time)
        {
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
            Time = time;
        }
    }
}
