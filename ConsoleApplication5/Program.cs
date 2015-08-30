using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
           
          // new CustomXmlFile().CreateXml();
           new CustomXmlFile().Deserialize();
           SqlDB mySqlDb = new SqlDB();
            mySqlDb.CreateDb();
            //mySqlDb.CreateTable();
            mySqlDb.InsertValueDb();
            

        }


       
    }

  
}
