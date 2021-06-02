using System;
using Terminal.Gui;

public class HomeWindow : Window
{
    public bool loggedOut;
    private UserRepository userRepository;
    private GoodRepository goodRepository;
    protected TextField loggedAsInput;
    public HomeWindow(User user)
    {
        this.Title = "Home Window";

       // int rightColumnX = 20;

        Label logedAsLbl = new Label(40, 1, "Logged as: ");
        loggedAsInput = new TextField(user.username)
        {
            X = Pos.Right(logedAsLbl),
            Y = Pos.Top(logedAsLbl),
            Width = 25,
            ReadOnly = true,
        };
        this.Add(logedAsLbl, loggedAsInput);

        Button logOut = new Button(60, 3, "Log out");
        logOut.Clicked += OnLogOut;
        this.Add(logOut);

        Button exit = new Button(60, 5, "Exit");
        exit.Clicked += ClickQuit;
        this.Add(exit);

        Button doOrder = new Button(2,2,"Do order");
        doOrder.Clicked += OpenOrderInterface;
        this.Add(doOrder);
        

    }
    private void OpenOrderInterface()
    {
        OrderWindow orderWindow = new OrderWindow();
        orderWindow.SetRepository(goodRepository);
        Toplevel top = Application.Top;
        top.RemoveAll();
        top.Add(orderWindow);
        Application.Run();
    }
    private void OnLogOut()
    {
        this.loggedOut = true;
        Application.RequestStop();
    }
    public void SetRepository(UserRepository repository, GoodRepository goodRepository)
    {
        this.userRepository = repository;
        this.goodRepository = goodRepository;
    }

    private void ClickQuit()
    {
        Application.Shutdown();
        Environment.Exit(0);
    }
}