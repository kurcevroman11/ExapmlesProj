using System;
using System.Text.RegularExpressions;

namespace ConsoleApp3
{
    class Passager : User, ISelect
    {
        public void Select()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Для выхода из режима \"пассажира\" введите \"выход\"");
            Console.WriteLine("Вы выбрали режим пассажира.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Выберете функцию:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("1.Купить билет\t2.Посмотреть расписание\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Введите номер функции: ");
            string select = Console.ReadLine().ToLower();
            BD(); // создаю подключение к БД
            Console.WriteLine();
            switch (select)
            {
                case "1":
                    BuyTicket();
                    break;
                case "2":
                    CheckRecords();
                    break;
                case "выход":
                    Console.Clear();
                    return;
                    break;
                default:
                    Console.WriteLine("Введено некорректное значение!");
                    break;
            }
        }
        public override void CheckRecords()
        {
            base.CheckRecords();
            Console.WriteLine($"Место прибытия: {Place}");
            Console.WriteLine($"Цена билета: {Cost}");
            Console.WriteLine($"Количество мест: {Amount_place}");
           
            Console.WriteLine("Расписание автобуса:\nВремя отправления\tВремя прибытия\t");
            while (Departure_time <= Arrival_time)
            {
                Console.WriteLine($"{Departure_time.ToShortTimeString()} \t\t\t {Departure_time.AddMinutes(Interval).ToShortTimeString()}");
                Departure_time = Departure_time.AddMinutes(Interval);
            }
        }
        private void BuyTicket()
        {
            CheckRecords();
            bool is_time = false;
            string StringTime;  // переменая для ввода времени пользователем
            Regex regex = new Regex(@"[0-9]*[0-9]");
            while (is_time != true) // обработка исключения ввода времени
            {
                Console.Write("Введите время отправления (к примеру 6:25): ");
                StringTime = Console.ReadLine();
                MatchCollection matches = regex.Matches(StringTime);
                if (matches.Count > 0)
                {
                    int count = 0;
                    foreach (Match match in matches)
                    {
                        if (count == 0)
                        {
                            if (Convert.ToInt32(match.Value) > 23 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода часа
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                break;
                            }                           
                            count++;
                        }
                        else if (count == 1)
                        {
                            if (Convert.ToInt32(match.Value) > 59 || (Convert.ToInt32(match.Value) < 0)) // обработка ввода минут
                            {
                                Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");
                                break;
                            }
                            is_time = true;
                            command.CommandText = $"UPDATE [Schedule] SET Amount_place = @place WHERE Place_arrival = N'{place}'";
                            uint amount_place = Amount_place - 1;
                            command.Parameters.AddWithValue("@place", amount_place);
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("Билет приобретён!");
                }
                else             
                    Console.WriteLine("Введённ некорректный формат времени(введите, к примеру 6:25)");              
            }
        }
    }
}
