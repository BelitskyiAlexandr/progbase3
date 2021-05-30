using System;
using Microsoft.Data.Sqlite;

// insert, delete, get(all, one)



class Program
{
    static void Main(string[] args)
    {
        string databaseFileName = "./data/shop";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        UserRepository userRepository = new UserRepository(connection);
        Console.WriteLine("Hello World!");


        Good[] goods = new Good[]
        {
                new Good("laptop", 30.3, true, "good enough"),
                new Good("dd", 44.0, true, "dd"),
        };

        Order order = new Order(0, goods);
        Console.WriteLine(order.ToString());
    }
}

