using System;
using Microsoft.Data.Sqlite;
using Terminal.Gui;

class Program
{
    static void Main(string[] args)
    {
        string databaseFileName = @"../../data/shop";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        UserRepository userRepository = new UserRepository(connection);
        GoodRepository goodRepository = new GoodRepository(connection);
        OrderRepository orderRepository = new OrderRepository(connection);
        XmlProcess export = new XmlProcess();


        Application.Init();

        Toplevel top = Application.Top;

        EnteringWindow win = new EnteringWindow();
        win.SetRepository(userRepository);



        top.Add(win);


        Application.Run();

    }
}

