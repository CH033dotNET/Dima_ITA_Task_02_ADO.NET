using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApplication5
{
    public class CustomXmlFile
    {
        readonly XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfItem));
        public ArrayOfItem deserializeobject;
       
        public void CreateXml()
        {
            Position position1 = new Position(124, 45, 789, DateTime.Today);
            Position position2 = new Position(1284, 452, 4789, DateTime.Today);
           
            List<Position> positions = new List<Position>();
            positions.Add(position1);
            positions.Add(position2);

            var job = new Job(DateTime.Today, "Some decription", "0978263445", "1");
            var job1 = new Job(DateTime.Today, "Some decription2", "0956683441", "1");

            var jobs = new List<Job>();
            jobs.Add(job);
            jobs.Add(job1);

            var item = new Item(1, "Vasya", "Ivanov", positions, jobs);
            var item2 = new Item(2, "Jenya", "Positive", positions, jobs);
            var items = new List<Item>();
            items.Add(item);
            items.Add(item2);

            // объект для сериализации

            var arreyofItem = new ArrayOfItem(items);
            Console.WriteLine("Объект создан");

            // передаем в конструктор тип класса
             

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("Items.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, arreyofItem);

                Console.WriteLine("Объект сериализован");
            }



            Console.ReadLine();
        }

        public List<Item> Deserialize()
        {
            try
         {
                using (var stream = new FileStream("Items.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Восстанавливаем объект из XML-файла.
                   deserializeobject = serializer.Deserialize(stream) as ArrayOfItem;
                   Console.WriteLine("Объект Десериализован!");
                  
                }
         }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();

            
            return deserializeobject.items;
        }

        


        
    }
}
