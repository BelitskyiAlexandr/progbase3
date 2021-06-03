using Terminal.Gui;

public class ModeratorHomeWindow : HomeWindow
{
    protected XmlProcess xmlProcess;
    public ModeratorHomeWindow()
    {
        this.Title = "Moderator Main Window";
        // Toplevel top = Application.Top;

        MenuBar menuBar = new MenuBar(new MenuBarItem[]
            {
            new MenuBarItem("_File", new MenuItem[]
            {
                new MenuItem("_Export", "",this.ClickExport),
                new MenuItem("_Import","",this.ClickImport),
                new MenuItem("_Quit", "", this.ClickQuit),
            }),
            new MenuBarItem("_Help", new MenuItem[]
            {
                new MenuItem("_About","", this.ClickAbout)
            })
            });


        this.Add(menuBar);

        Button allUsers = new Button(60, 8, "All users");
        allUsers.Clicked += SeeAllUsers;
        this.Add(allUsers);

        Button allOrders = new Button(60, 10, "All Orders");
        allOrders.Clicked += SeeAllOrders;
        this.Add(allOrders);

        Button allGoods = new Button(60, 12, "All Goods");
        allGoods.Clicked += SeeAllGoods;
        this.Add(allGoods);
    }
    public void SeeAllGoods()
    {
        GoodsDialog goodsDialog = new GoodsDialog();
        goodsDialog.SetRepository(goodRepository);
        Application.Run(goodsDialog);
    }
    public void SeeAllOrders()
    {
        OrdersDialog ordersDialog = new OrdersDialog();
        ordersDialog.SetRepository(goodRepository, orderRepository);
        Application.Run(ordersDialog);
    }
    public void SeeAllUsers()
    {
        UsersDialog usersDialog = new UsersDialog();
        usersDialog.SetRepository(userRepository);
        Application.Run(usersDialog);
    }
    public void SetRepositories(UserRepository userRepository, GoodRepository goodRepository, OrderRepository orderRepository, XmlProcess xmlProcess)
    {
        this.userRepository = userRepository;
        this.goodRepository = goodRepository;
        this.orderRepository = orderRepository;
        this.xmlProcess = xmlProcess;
    }
    public void ClickAbout()
    {
        Button back = new Button(30, 16, "Back");
        back.Clicked += ClickQuit;

        Dialog dialog = new Dialog("About", back);
        TextView textView = new TextView()
        {
            X = 2,
            Y = 2,
            Width = 65,
            Height = 12,
            ReadOnly = true,
            Text = @"
            Цей додаток призначений для електронного 
                автоматизованого обігу товарів:
            
            
            Знаходиться на ранній стадії розробки


                Автор: Беліцький Олександр  
            
                    ShopApp Beta"

        };

        dialog.Add(textView);
        dialog.AddButton(back);

        Application.Run(dialog);
    }

    public void ClickQuit()
    {
        Application.RequestStop();
    }

    public void ClickExport()
    {

    }
    public void ClickImport()
    {

    }
}