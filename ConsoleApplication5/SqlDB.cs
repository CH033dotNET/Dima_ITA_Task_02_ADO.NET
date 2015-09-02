using System;
using System.Collections.Generic;
using System.Data;
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
        private SqlCommand cmd;
        private string[] namesTables = { "Items", "Job", "Position" };

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
                                         " Items (Id int  NOT NULL PRIMARY KEY" +
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
                                         ", Time DateTime not null," +
                                         " Decription varchar(100) not null," +
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


            Console.WriteLine("Таблица создана успешно");
            Console.ReadKey();
        }
        public void DropTableDB()
        {
            string[] drops = {  "DROP TABLE Position", "DROP TABLE Job","DROP TABLE Items"};
            for (int i = 0; i < drops.Length; i++)
            {
                AddCommand(drops[i]);
            }
            Console.WriteLine("Table delited sucsefully!");
            Console.ReadKey();
        }
        public void InsertValueDb()
        {
            
            string comandItem ;
            string comandPosition;
            string comandJob;
            
            List<Item> items = xmlFile.Deserialize();

            
            
            foreach (Item item in items )
            {
                comandItem =
                    "INSERT Items VALUES ('" + item.Id + "','" + item.FirstName + "', '" + item.LastName + "')";

                AddCommand(comandItem);
                
                foreach (Position position in item.positionsHistory)
                {
                    comandPosition = "INSERT Position VALUES ('" + position.Latitude + "', '" + position.Longitude + "', '" + position.Accuracy + "', '" + position.Time + "', '" + item.Id + "')";
                    AddCommand(comandPosition);
                }

                foreach (Job job in item.jobHistory)
                {
                    comandJob = "INSERT Job VALUES ('" + job.Time + "', '" + job.Decription + "', '" + job.Phone + "', '" + job.UserId + "')";
                    AddCommand(comandJob);
                }
            }

            
           
            Console.WriteLine("Данные успешно внесенны!");
            Console.ReadKey();
        }

        public void GetAllTables()
        {
            for (int i = 0; i < namesTables.Length; i++)
            {
                GetTable(namesTables[i]);
                Console.WriteLine("\n");
            }
        }
        public void GetTable(string nameTable)
        {
            cmd = new SqlCommand("SELECT * FROM " + nameTable, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            DataTable table = CreateSchemaFromReader(reader, nameTable);

            WriteDataFromReader(table, reader);
            Console.WriteLine( "Name table: "+nameTable);
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                    Console.WriteLine("{0}: {1}", column.ColumnName, row[column]);

                Console.WriteLine();
            }
            reader.Close();
            Console.ReadKey();
        }
        private static DataTable CreateSchemaFromReader(SqlDataReader reader, string tableName)
        {
            DataTable table = new DataTable(tableName);

            for (int i = 0; i < reader.FieldCount; i++)
                table.Columns.Add(new DataColumn(reader.GetName(i), reader.GetFieldType(i)));

            return table;
        }
        private static void WriteDataFromReader(DataTable table, SqlDataReader reader)
        {
            while (reader.Read())
            {
                DataRow row = table.NewRow();

                for (int i = 0; i < reader.FieldCount; i++)
                    row[i] = reader[i];

                table.Rows.Add(row);
            }
        }
        public void AddCommand( string command)
        {
            using ( cmd = new SqlCommand(command, conn))
            {
                //посылаем запрос
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Ошибка команды");
                    Console.ReadKey();
                    
                }
                
            }

        }
        public void CloseConnection()
        {
            conn.Close();
            conn.Dispose();
        }

            
        
      

    }
}
