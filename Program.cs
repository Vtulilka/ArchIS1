
using LB1;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

class AsyncUdpClient
{
    //private const int Port = 8001;
    //private const string ServerIp = "127.0.0.1";

    static void Main()
    {
        PrintMenu();
        
        while (true)
        {               
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    _ = Client.SendRequest("1");
                    PrintMenu();
                    break;
                case ConsoleKey.D2:
                    {
                        Console.Write("\nВведите номер записи: ");
                        string command = Console.ReadLine();
                        Console.Clear();
                        _ = Client.SendRequest("2" + "," + command);
                        PrintMenu();
                    }
                    break;
                case ConsoleKey.D3:
                    {
                        Console.Write("\nВведите номер записи: ");
                        string command = Console.ReadLine();
                        Console.Clear();
                        _ = Client.SendRequest("3" + "," + command);
                        PrintMenu();
                    }
                    break;
                case ConsoleKey.D4:
                    {
                        Console.WriteLine("\nВведите данные");
                        Console.Write("\nИмя: ");
                        string Name = Console.ReadLine();
                        Console.Write("\nФамилия: ");
                        string Surname = Console.ReadLine();
                        Console.Write("\nВозраст: ");
                        string Age = Console.ReadLine();
                        Console.Write("\nЕсть ли двойки в четверти?: ");
                        string IsBad = Console.ReadLine();
                        if (IsBad == "да" || IsBad == "Да")
                            IsBad = "true";
                        else if (IsBad == "нет" || IsBad == "Нет")
                            IsBad = "false";
                        Console.Clear();
                        _ = Client.SendRequest("4" + "," + Name + "," + Surname + "," + Age + "," + IsBad);
                        PrintMenu();
                    }
                    break;
                case ConsoleKey.D5:
                    {
                        Console.Clear();
                        _ = Client.SendRequest("5");
                        PrintMenu();
                        break;
                    }
                case ConsoleKey.Escape: { return ; }
                default:
                    break;
            }
        }        
    }

    static void PrintMenu()
    {
        Console.WriteLine("Введите команду:");
        Console.WriteLine("1) Вывести все записи");
        Console.WriteLine("2) Вывести запись по номеру");
        Console.WriteLine("3) Удалить запись по номеру");
        Console.WriteLine("4) Добавить запись");
        Console.WriteLine("5) Очистить файл");
        Console.WriteLine("esc) Выйти из программы");
    }
}