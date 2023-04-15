using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApp3
{
    abstract class User 
    {
        public SqlConnection SqlConnection = null;
        protected SqlCommand command = null;
        protected string sql;
        public string place;
        public uint cost;
        public uint amount_place;
        public DateTime departure_Time;
        public DateTime arrival_Time;
        public uint interval;
        public string Place { get => place; set { place = value; } }
        public uint Cost { get => cost;  set { cost = value; } }
        public uint Amount_place { get => amount_place;  set { amount_place = value; } }
        public DateTime Departure_time { get => departure_Time;  set { departure_Time = value; } }
        public DateTime Arrival_time { get => arrival_Time;  set { arrival_Time = value; } }
        public uint Interval { get => interval;  set { interval = value; } }    
        public void BD()
        {
            SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            SqlConnection.Open();
        }   
        public virtual void CheckRecords()
        {
            Console.Write("Введите место отправления: ");
            place = Console.ReadLine();
            sql = $"SELECT * FROM [Schedule] WHERE Place_arrival = N'{place}'";
            command = new SqlCommand(sql, SqlConnection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Place = (string)reader.GetValue(1);
                Cost = Convert.ToUInt32(reader.GetValue(2));
                Amount_place = Convert.ToUInt32(reader.GetValue(3));
                Departure_time = (DateTime)reader.GetValue(4);
                Arrival_time = (DateTime)reader.GetValue(5);
                Interval = Convert.ToUInt32(reader.GetValue(6));
            }
            reader.Close();           
        }
    }
}
