using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApplication5
{
    public class Item
    {
        public int id;
        public string firstName;
        public string lastName;



        [XmlArray("positionsHistory")]
        public List<Position> positionsHistory;

        [XmlArray("jobHistory")]
        public List<Job> jobHistory;
        // стандартный конструктор без параметров
        
         
        
        
        public Item()
        { }

        public Item(int id, string fName, string lName, List<Position> positions, List<Job> jobHistories)
        {
            this.id = id;
            firstName = fName;
            lastName = lName;
            positionsHistory = positions;
            jobHistory = jobHistories;
        }

        [XmlIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [XmlIgnore]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [XmlIgnore]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
    }
}
