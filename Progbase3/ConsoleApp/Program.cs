using System;
using Microsoft.Data.Sqlite;

// insert, delete, get(all, one)


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string databaseFileName = "./data/shop";
            SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
            UserRepository userRepository = new UserRepository(connection);
            Console.WriteLine("Hello World!");
            
        }
    }
}
