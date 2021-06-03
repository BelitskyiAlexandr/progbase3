
using System.Collections.Generic;
using Terminal.Gui;

public class OrderWindow : Window
{
    private ListView allGoodsView;
    private GoodRepository goodRepository;
    private OrderRepository orderRepository;
    private UserRepository userRepository;
    public Order order;
    private User user;
    private int pageLength = 5;
    private int page = 1;

    private Label totalPagesLabel;
    private Label pageLabel;

    public OrderWindow(User user, Order order)
    {
        this.user = user;
        this.order = order;
        this.Title = "Goods list";

        Rect frame = new Rect(4, 8, 40, 20);
        allGoodsView = new ListView(new List<Good>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        allGoodsView.OpenSelectedItem += OpenGood;


        Button prevPageBtn = new Button(2, 6, "prev");
        prevPageBtn.Clicked += ToPrevPage;
        pageLabel = new Label("?")
        {
            X = Pos.Right(prevPageBtn) + 2,
            Y = Pos.Top(prevPageBtn),
            Width = 5,
        };
        totalPagesLabel = new Label("?")
        {
            X = Pos.Right(pageLabel) + 2,
            Y = Pos.Top(prevPageBtn),
            Width = 5,
        };
        Button nextPageBtn = new Button("next")
        {
            X = Pos.Right(totalPagesLabel) + 2,
            Y = Pos.Top(prevPageBtn),
        };
        nextPageBtn.Clicked += ToNextPage;
        this.Add(prevPageBtn, pageLabel, totalPagesLabel, nextPageBtn);

        FrameView frameView = new FrameView("Goods")
        {
            X = 2,
            Y = 8,
            Width = Dim.Fill() - 4,
            Height = pageLength + 2,
        };
        frameView.Add(allGoodsView);

        this.Add(frameView);

        Button yourOrder = new Button(2, 3, "Basket");
        yourOrder.Clicked += seeCurrentOrder;
        this.Add(yourOrder);

    }
    private void seeCurrentOrder()
    {
        YourOrderDialog yourOrderDialog = new YourOrderDialog(this.order, this.orderRepository, this.goodRepository);
        Application.Run(yourOrderDialog);
        if (yourOrderDialog.confirm)
        {
            HomeWindow homeWindow = new HomeWindow();
            homeWindow.SetRepository(user, userRepository, goodRepository, orderRepository);
            Toplevel top = Application.Top;
            top.RemoveAll();
            top.Add(homeWindow);
            Application.Run();
        }
    }
    private void ToPrevPage()
    {
        if (page == 1)
        {
            return;
        }
        this.page -= 1;
        ShowCurrentPage();
    }
    private void ToNextPage()
    {
        if (page >= goodRepository.GetTotalPages())
        {
            return;
        }
        this.page += 1;
        ShowCurrentPage();
    }
    private void ShowCurrentPage()
    {
        this.pageLabel.Text = page.ToString();
        this.totalPagesLabel.Text = goodRepository.GetTotalPages().ToString();
        this.allGoodsView.SetSource(goodRepository.GetPage(page));
    }

    public void SetRepository(GoodRepository repository, OrderRepository orderRepository, UserRepository userRepository)
    {
        this.goodRepository = repository;
        this.orderRepository = orderRepository;
        this.userRepository = userRepository;
        ShowCurrentPage();
    }

    private void OpenGood(ListViewItemEventArgs args)
    {
        Good good = (Good)args.Value;
        OpenGoodDialog dialog = new OpenGoodDialog(this.order);

        dialog.SetGood(good);


        Application.Run(dialog);
        order = dialog.order;

    }
}
