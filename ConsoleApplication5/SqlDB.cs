using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    public class SqlDB
    {
        private string connStr;
        private SqlConnection conn;
        private CustomXmlFile xmlFile;



        public SqlDB()
        {
            xmlFile = new CustomXmlFile();
        }

        public void CreateDb()
        {
             connStr = @"Data Source=TORTOISE\MESSAP;Initial Catalog=ItaWork;Integrated Security=True";
             conn = new SqlConnection(connStr);

            try
            {
                //пробуем подключится
                conn.Open();
            }
            catch (SqlException se)
            {
                
                if (se.Number == 4060)
                {
                    Console.WriteLine("Подождите, идет создание БД");
                 
                    conn.Close();
                   
                    conn = new SqlConnection(@"Data Source=TORTOISE\MESSAP;Integrated Security=True");
                 
                    SqlCommand cmdCreateDataBase = new SqlCommand(string.Format("CREATE DATABASE [{0}]", "Test"), conn);
                   
                    conn.Open();
                  
                    Console.WriteLine("Посылаем запрос");
                    cmdCreateDataBase.ExecuteNonQuery();
                  
                    conn.Close();
                   
                    Thread.Sleep(5000);
                    conn = new SqlConnection(connStr);
                   }
            }


            finally
            {
                Console.WriteLine("Соедение успешно произведено"); 
                Console.ReadKey();
            }
        }

        public void CreateTable()
        {
            string createItemsTable = "CREATE TABLE " +
                                         " Items (Id int IDENTITY NOT NULL PRIMARY KEY" +
                                         ", FirstName Varchar(60) not null," +
                                         " LastName Varchar(60) not null)";

            string createPositionTable = "CREATE TABLE " +
                                         " Position (Id int IDENTITY NOT NULL" +
                                         ", Latitude int not null," +
                                         " Longitude int not null," +
                                         " Accuracy int not null," +
                                         " Time DateTime2 not null," +
                                         " UserId int NOT NULL FOREIGN KEY REFERENCES Items(Id))";
            
            string createJobTable ="CREATE TABLE " +
                                         " Job (Id int IDENTITY NOT NULL" +
                                         ", Time DateTime2 not null," +
                                         " Decription nvarchar not null," +
                                         " Phone varchar(17) not null," +
                                         " UserId int NOT NULL FOREIGN KEY REFERENCES Items(Id))";
            
            string[] sqlStrings = {createItemsTable,createPositionTable,createJobTable};

            for (int i = 0; i < sqlStrings.Length; i++)
            {

                using (SqlCommand cmdCreateTable = new SqlCommand(sqlStrings[i], conn))
                {

                    //посылаем запрос
                    try
                    {
                        cmdCreateTable.ExecuteNonQuery();
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка при создании таблицы или таблица уже создана");
                        Console.ReadKey();
                        return;
                    }
                    Console.WriteLine("Таблицы успешно созданы");
                }
            }




           conn.Close();
           conn.Dispose();
 
            Console.WriteLine("Таблица создана успешно");
            Console.ReadKey();
        }


        public void InsertValueDb()
        {
            
            string comandItem ;
            string comandJob;
            string valuesItems;
            List<Item> items = xmlFile.Deserialize();

            foreach (Item item in items )
            {
                comandItem =
                    "INSERT Items VALUES ('" + item.FirstName + "', '" + item.LastName + "')";

                AddCommand(comandItem);
                
                foreach (var position in item.positionsHistory)
                {
                    comandJob = "INSERT Items VALUES ('" + position.Latitude + "', '" + position.Longitude + "', '" + position.Accuracy + "', '" + position.Time + "')";
                    AddCommand(comandJob);
                }

                foreach (var job in item.jobHistory)
                {
                    
                }
            }
            conn.Close();
            conn.Dispose();
        }

        public void AddCommand( string command)
        {
            using (SqlCommand cmdCreateTable = new SqlCommand(command, conn))
            {
                //посылаем запрос
                try
                {
                    cmdCreateTable.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Ошибка при добавленни данных в таблицы ");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Значения в таблицу успешно добавленны");
            }
        }


            
        
      

    }
}
