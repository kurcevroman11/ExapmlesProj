using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ConsoleApp3
{
    class Employer : User, ISelect
    {     
        public void Select()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Для выхода из режима \"сотрудник\" введите \"выход\"");
            Console.WriteLine("Вы выбрали режим сотрудника.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Выберете функцию:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("1.Добавить\t2.Изменить\t3.Удалить\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Введите номер функции: ");
            string select = Console.ReadLine().ToLower();
            BD(); // создаю подключение к БД
            switch (select)
            {
                case "1":
                    Add();
                    break;
                case "2":
                    Update();
                    break;
                case "3":
                    Delete();
                    break;
                case "выход":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("Введено некорректное значение!");
                    break;
            }           
        }
        private void Add()
        {                     
            command = new 
                SqlCommand($"INSERT INTO [Schedule] (Place_arrival, Cost, Amount_Place, Departure_time, Arrival_time, Interval) " +
                $"VALUES (@Place_arrival, @Cost, @Amount_Place, @Departure_time, @Arrival_time, @Interval)", SqlConnection);

            Console.Write("Введите место отправления: ");
            bool is_string = false;
            string str = Console.ReadLine();
            int number; // number - это число, которое удалось преобразовать из строки
            while (is_string != true) // обработка исключения неправильного ввода строки
            {
                if (int.TryParse(str, out number))
                {
                    Console.WriteLine("Введите корректное значение!");
                    Console.Write("Введите место отправления: ");
                    str = Console.ReadLine();
                }
                else
                {
                    is_string = true;
                    command.Parameters.AddWithValue("Place_arrival", str);
                }
            }

            Console.WriteLine();

            bool is_int = false;
            while (is_int != true) // обработка исключения ввода числа
            {
                try
                {
                    Console.Write("Введите стоимость билета: ");
                    cost = uint.Parse(Console.ReadLine());
                    while (cost < 0)
                    {
                        Console.WriteLine("Цена билета должно быть больше нуля!");
                        Console.Write("Введите стоимость билета: ");
                        cost = uint.Parse(Console.ReadLine());
                    }
                    is_int = true;
                    command.Parameters.AddWithValue("Cost", cost);
                }
                catch (Exception)
                {
                    Console.WriteLine("Введите число цифарми!");
                }
            }

            Console.WriteLine();

            is_int = false;
            while (is_int != true) // обработка исключения ввода числа
            {
                try
                {
                    Console.Write("Введите количетсво мест: ");
                    amount_place = uint.Parse(Console.ReadLine());
                    while (amount_place < 0)
                    {
                        Console.WriteLine("Количетсво мест должно быть больше нуля!");
                        Console.Write("Введите количетсво мест: ");
                        amount_place = uint.Parse(Console.ReadLine());
                    }
                    is_int = true;
                    command.Parameters.AddWithValue("Amount_Place", amount_place);
                }
                catch (Exception)
                {
                    Console.WriteLine("Введите число цифарми!");
                }
            }

            Console.WriteLine();

            DateTime time = new DateTime(); // текущее время
            bool is_time = false;
            time = DateTime.Today; // пример даты: 20.07.2015 0:00:00
            string StringTime;  // переменая для ввода времени пользователем
            Regex regex = new Regex(@"[0-9]*[0-9]");
            while (is_time != true) // обработка исключения ввода времени
            {
                Console.Write("Введите время 1-го рейса (к примеру 6:25): ");
                StringTime = Console.ReadLine();
                MatchCollection matches = regex.Matches(StringTime);
                if (matches.Count > 0)
                {
                    int count = 0;
                    time = DateTime.Today;
                    foreach (Match match in matches)
                    {
                        if (count == 0)
                        {
                            if (Convert.ToInt32(match.Value) > 23 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода часа
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                break;
                            }
                            time = time.AddHours(Convert.ToInt32(match.Value));
                            count++;
                        }
                        else if (count == 1)
                        {
                            if (Convert.ToInt32(match.Value) > 59 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода минут
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                break;
                            }
                            time = time.AddMinutes(Convert.ToInt32(match.Value));
                            is_time = true; 
                            command.Parameters.AddWithValue("Departure_time", time);                            
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                }
            }

            Console.WriteLine();

            time = DateTime.Today;
            is_time = false; // устанавливаю правилность введёного времени
            StringTime = "";
            while (is_time != true)
            {
                Console.Write("Введите время последнего рейса (к примеру 18:25): ");
                StringTime = Console.ReadLine();
                MatchCollection matches = regex.Matches(StringTime);
                if (matches.Count > 0)
                {
                    int is_minutes = 0;
                    time = DateTime.Today;
                    foreach (Match match in matches)
                    {
                        if (is_minutes == 0)
                        {
                            if (Convert.ToInt32(match.Value) > 23 || (Convert.ToInt32(match.Value) < 0))
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 18:25)");
                                break;
                            }
                            time = time.AddHours(Convert.ToInt32(match.Value));
                            is_minutes++;
                        }
                        else if (is_minutes == 1)
                        {
                            if (Convert.ToInt32(match.Value) > 59 || (Convert.ToInt32(match.Value) < 0))
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 18:25)");
                                break;
                            }
                            time = time.AddMinutes(Convert.ToInt32(match.Value));
                            is_time = true;
                            command.Parameters.AddWithValue("Arrival_time", time);
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                }
            }

            Console.WriteLine();

            is_int = false;
            while (is_int != true) // обработка исключения ввода числа
            {
                try
                {
                    Console.Write("Введите интервал отправления: ");
                    int inrterval = int.Parse(Console.ReadLine());
                    while (inrterval < 0)
                    {
                        Console.WriteLine("Введите интервал отправления должнен быть больше нуля!");
                        Console.Write("Введите интервал отправления: ");
                        inrterval = int.Parse(Console.ReadLine());
                    }
                    is_int = true;
                    command.Parameters.AddWithValue("Interval", inrterval);
                }
                catch (Exception)
                {
                    Console.WriteLine("Введите число цифарми!");
                }
            }
            command.ExecuteNonQuery().ToString();
        }

        public override void CheckRecords()
        {
            base.CheckRecords();
            Console.WriteLine($"Место прибытия: {Place}");
            Console.WriteLine($"Цена билета: {Cost}");
            Console.WriteLine($"Количество мест: {Amount_place}");
            Console.WriteLine($"Время 1-го рейса: {Departure_time}");
            Console.WriteLine($"Время последнего рейса: {Arrival_time}");
            Console.WriteLine($"Интервал рейсов: {Interval}");
        }
        private void Update()
        {
            CheckRecords();
            string str = "";
            while (str.ToLower() != "выход")
            {              
                Console.Write("Введите название записи, которую хотите изменить(к примеру, \"Цена билета\"): ");
                str = Console.ReadLine().ToLower();
                switch (str)
                {
                    case "место прибытия":                      
                        Console.Write("Место прибытия: ");
                        str = Console.ReadLine();
                        bool is_string = false;                        
                        int number; // number - это число, которое удалось преобразовать из строки
                        while (is_string != true) // обработка исключения неправильного ввода строки
                        {
                            if (int.TryParse(str, out number))
                            {
                                Console.WriteLine("Введите корректное значение!");
                                Console.Write("Введите место отправления: ");
                                str = Console.ReadLine();
                            }
                            else
                            {
                                sql = $"UPDATE [Schedule] SET Place_arrival = N'{str}' WHERE Place_arrival = N'{place}'";
                                command = new SqlCommand(sql, SqlConnection);
                                command.ExecuteNonQuery();
                                is_string = true;
                            }
                        }                     
                        break;
                    case "цена билета":
                        bool is_int = false;
                        while (is_int != true) // обработка исключения ввода числа
                        {
                            try
                            {
                                Console.Write("Введите стоимость билета: ");
                                int cost = int.Parse(Console.ReadLine());
                                while (cost < 0)
                                {
                                    Console.WriteLine("Цена билета должно быть больше нуля!");
                                    Console.Write("Введите стоимость билета: ");
                                    cost = int.Parse(Console.ReadLine());
                                }
                                is_int = true;
                                sql = $"UPDATE [Schedule] SET Cost = {cost} WHERE Place_arrival = N'{place}'";
                                command = new SqlCommand(sql, SqlConnection);
                                command.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Введите число цифарми!");
                            }
                        }                   
                        break;
                    case "количество мест":
                        is_int = false;
                        while (is_int != true) // обработка исключения ввода числа
                        {
                            try
                            {
                                Console.Write("Введите количетсво мест: ");
                                int amount = int.Parse(Console.ReadLine());
                                while (amount < 0)
                                {
                                    Console.WriteLine("Количетсво мест должно быть больше нуля!");
                                    Console.Write("Введите количетсво мест: ");
                                    amount = int.Parse(Console.ReadLine());
                                }
                                is_int = true;
                                sql = $"UPDATE [Schedule] SET Amount_place = {amount} WHERE Place_arrival = N'{place}'";
                                command = new SqlCommand(sql, SqlConnection);
                                command.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Введите число цифарми!");
                            }
                        }                       
                        break;
                    case "время 1-го рейса":
                        DateTime time = new DateTime(); // текущее время
                        bool is_time = false;
                        time = DateTime.Today; // пример даты: 20.07.2015 0:00:00
                        string StringTime;  // переменая для ввода времени пользователем
                        Regex regex = new Regex(@"[0-9]*[0-9]");
                        while (is_time != true) // обработка исключения ввода времени
                        {
                            Console.Write("Введите время 1-го рейса (к примеру 6:25): ");
                            StringTime = Console.ReadLine();
                            MatchCollection matches = regex.Matches(StringTime);
                            if (matches.Count > 0)
                            {
                                int count = 0;
                                time = DateTime.Today;
                                foreach (Match match in matches)
                                {
                                    if (count == 0)
                                    {
                                        if (Convert.ToInt32(match.Value) > 23 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода часа
                                        {
                                            Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                            break;
                                        }
                                        time = time.AddHours(Convert.ToInt32(match.Value));
                                        count++;
                                    }
                                    else if (count == 1)
                                    {
                                        if (Convert.ToInt32(match.Value) > 59 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода минут
                                        {
                                            Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                            break;
                                        }
                                        time = time.AddMinutes(Convert.ToInt32(match.Value));
                                        is_time = true;                                        
                                        command.CommandText = $"UPDATE [Schedule] SET Departure_time = @time WHERE Place_arrival = N'{place}'";
                                        command.Parameters.AddWithValue("@time", time);                                       
                                        command.ExecuteNonQuery();
                                        
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                            }
                        }                       
                        break;
                    case "время последнего рейса":
                        time = new DateTime(); // текущее время
                        is_time = false;
                        time = DateTime.Today; // пример даты: 20.07.2015 0:00:00
                        StringTime = "";  // переменая для ввода времени пользователем
                        regex = new Regex(@"[0-9]*[0-9]");
                        while (is_time != true) // обработка исключения ввода времени
                        {
                            Console.Write("Введите время 1-го рейса (к примеру 6:25): ");
                            StringTime = Console.ReadLine();
                            MatchCollection matches = regex.Matches(StringTime);
                            if (matches.Count > 0)
                            {
                                int count = 0;
                                time = DateTime.Today;
                                foreach (Match match in matches)
                                {
                                    if (count == 0)
                                    {
                                        if (Convert.ToInt32(match.Value) > 23 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода часа
                                        {
                                            Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                            break;
                                        }
                                        time = time.AddHours(Convert.ToInt32(match.Value));
                                        count++;
                                    }
                                    else if (count == 1)
                                    {
                                        if (Convert.ToInt32(match.Value) > 59 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода минут
                                        {
                                            Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                            break;
                                        }
                                        time = time.AddMinutes(Convert.ToInt32(match.Value));
                                        is_time = true;
                                        command.CommandText = $"UPDATE [Schedule] SET Arrival_time = @time2 WHERE Place_arrival = N'{place}'";
                                        command.Parameters.AddWithValue("@time2", time);
                                        command.ExecuteNonQuery();

                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                            }
                        }
                        break;
                    case "интервал рейсов":
                        is_int = false;
                        while (is_int != true) // обработка исключения ввода числа
                        {
                            try
                            {
                                Console.Write("Введите интервал отправления: ");
                                int inrterval = int.Parse(Console.ReadLine());
                                while (inrterval < 0)
                                {
                                    Console.WriteLine("Введите интервал отправления должнен быть больше нуля!");
                                    Console.Write("Введите интервал отправления: ");
                                    inrterval = int.Parse(Console.ReadLine());
                                }
                                is_int = true;
                                sql = $"UPDATE [Schedule] SET Interval = {inrterval} WHERE Place_arrival = N'{place}'";
                                command = new SqlCommand(sql, SqlConnection);
                                command.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Введите число цифарми!");
                            }
                        }                       
                        break;
                    case "выход":                        
                        break;
                    default:
                        Console.WriteLine("Введено некорректное значение!");
                        break;
                }
            }
        }
        private void Delete()
        {
            Console.Write("Введите место прибытия для удаления рейса: ");
            string place = Console.ReadLine();
            string sqlExpression = $"DELETE FROM [Schedule] WHERE Place_arrival = N'{place}'";
            command = new SqlCommand(sqlExpression, SqlConnection);
            command.ExecuteNonQuery();
            Console.WriteLine("Рейс удалён!");
        }
    }
}
