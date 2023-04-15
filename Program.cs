using System;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string select = "";
            while (select.ToLower() != "выход")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Для выхода из программы введите \"выход\"");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Выберите режим:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1.Сотрудник\t2.Пассажир");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Режим(введите число): ");
                select = Console.ReadLine();
                uint number;
                if (uint.TryParse(select, out number) & number == 1)
                {
                    Employer e1 = new Employer();
                    e1.Select();
                }
                else if (uint.TryParse(select, out number) & number == 2)
                {
                    Passager p1 = new Passager();
                    p1.Select();
                }
            }
        }
    }
}
