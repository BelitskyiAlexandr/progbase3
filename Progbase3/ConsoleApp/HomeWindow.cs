using System;
using System.Collections.Generic;
using Terminal.Gui;

public class HomeWindow : Window
{
    private ListView allOrdersView;
    public User user;
    public bool loggedOut;
    protected OrderRepository orderRepository;
    protected UserRepository userRepository;
    protected GoodRepository goodRepository;
    protected TextField loggedAsInput;
    public HomeWindow()
    {
        this.Title = "Home Window";


        Label logedAsLbl = new Label(40, 2, "Logged as: ");
        loggedAsInput = new TextField()
        {
            X = Pos.Right(logedAsLbl),
            Y = Pos.Top(logedAsLbl),
            Width = 25,
            ReadOnly = true,
        };
        this.Add(logedAsLbl, loggedAsInput);

        Button logOut = new Button(60, 4, "Log out");
        logOut.Clicked += OnLogOut;
        this.Add(logOut);

        Button exit = new Button(60, 6, "Exit");
        exit.Clicked += ClickQuit;
        this.Add(exit);

        Button doOrder = new Button(2, 2, "Do order");
        doOrder.Clicked += OpenOrderInterface;
        this.Add(doOrder);

        allOrdersView = new ListView(new List<Order>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        allOrdersView.OpenSelectedItem += OpenOrder;

        FrameView frameView = new FrameView("Orders")
        {
            X = 2,
            Y = 5,
            Width = 55,
            Height = 15,
        };
        frameView.Add(allOrdersView);
        this.Add(frameView);



    }
    private List<Order> GetPage()
    {
        List<Order> orders = new List<Order>();

        for (int i = 0; i < orderRepository.GetAllUserOrdersById(user.id).Length; i++)
        {
            if (orderRepository.GetAllUserOrdersById(user.id).Length == i)
            {
                break;
            }
            orders.Add(orderRepository.GetAllUserOrdersById(user.id)[i]);
        }

        return orders;
    }
    private void OpenOrder(ListViewItemEventArgs args)
    {
        Order order = (Order)args.Value;
        order.goods = goodRepository.GetGoodsByOrderId(order.id);
        OpenOrderDialog openOrderDialog = new OpenOrderDialog(order);

        openOrderDialog.SetOrder(order);

        Application.Run(openOrderDialog);
    }
    private void OpenOrderInterface()
    {
        OrderWindow orderWindow = new OrderWindow(this.user, new Order(user.id));
        orderWindow.SetRepository(goodRepository, orderRepository, userRepository);
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
    public void SetRepository(User user, UserRepository repository, GoodRepository goodRepository, OrderRepository orderRepository)
    {
        this.userRepository = repository;
        this.goodRepository = goodRepository;
        this.orderRepository = orderRepository;
        this.user = user;
        loggedAsInput.Text = user.username; 
        this.allOrdersView.SetSource(GetPage());
    }

    private void ClickQuit()
    {
        Application.Shutdown();
        Environment.Exit(0);
    }
}