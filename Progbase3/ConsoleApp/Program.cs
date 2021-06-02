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

        bool loggedOut = true;
        while (loggedOut)
        {
            EnteringWindow enteringWin = new EnteringWindow();
            enteringWin.SetRepository(userRepository);


            top.Add(enteringWin);
            Application.Run();


            HomeWindow homeWindow = new HomeWindow(enteringWin.loggedUser);
            homeWindow.SetRepository(userRepository, goodRepository);
            top.RemoveAll();
            top.Add(homeWindow);
            Application.Run();
            loggedOut = homeWindow.loggedOut;

        }
    }
}

