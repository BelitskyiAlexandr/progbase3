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
                enteringWin.SetRepository(userRepository, orderRepository);


                top.Add(enteringWin);
                Application.Run();

                if (enteringWin.loggedUser.role == "user")
                {
                    HomeWindow homeWindow = new HomeWindow();
                    homeWindow.SetRepository(enteringWin.loggedUser, userRepository, goodRepository, orderRepository);
                    top.RemoveAll();
                    top.Add(homeWindow);
                    Application.Run();
                    loggedOut = homeWindow.loggedOut;

                }
                else if (enteringWin.loggedUser.role == "moderator")
                {
                    ModeratorHomeWindow homeWindow = new ModeratorHomeWindow();
                    homeWindow.SetRepository(enteringWin.loggedUser, userRepository, goodRepository, orderRepository);
                    top.RemoveAll();
                    top.Add(homeWindow);
                    Application.Run();
                    loggedOut = homeWindow.loggedOut;
    
                }
        }
    }
}

