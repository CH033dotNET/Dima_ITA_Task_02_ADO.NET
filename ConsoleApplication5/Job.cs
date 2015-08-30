using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApplication5
{
    public class Job
    {
        [XmlElement("time")]
        public DateTime Time { get; set; }

        [XmlElement("decription")]
        public string Decription { get; set; }

        [XmlElement("phone")]
        public string Phone { get; set; }

        [XmlElement("userId")]
        public string UserId { get; set; }

        public Job()
        {
        }

        public Job(DateTime time, string decription, string phone, string userId)
        {
            Time = time;
            Decription = decription;
            Phone = phone;
            UserId = userId;

        }

    }
}
