using System;
using System.Collections.Generic;
using Terminal.Gui;

public class YourOrderDialog : Dialog
{
    public bool confirm;
    private OrderRepository orderRepository;
    private GoodRepository goodRepository;
    private Order order;
    private ListView allGoodsView;
    private int pageLength = 5;
    private int page = 1;
    private Label totalPagesLabel;
    private Label pageLabel;

    public YourOrderDialog(Order order, OrderRepository orderRepository, GoodRepository goodRepository)
    {
        this.goodRepository = goodRepository;
        this.orderRepository = orderRepository;
        this.order = order;
        this.Title = "Basket";

        Rect frame = new Rect(4, 8, 40, 20);
        allGoodsView = new ListView(new List<Good>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        allGoodsView.OpenSelectedItem += OpenGood;

        Button prevPageBtn = new Button(2, 6, "prev");
        prevPageBtn.Clicked += ToPrevPage;
        pageLabel = new Label("0")
        {
            X = Pos.Right(prevPageBtn) + 2,
            Y = Pos.Top(prevPageBtn),
            Width = 5,
        };
        totalPagesLabel = new Label("1")
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
        ShowCurrentPage();

        Button Back = new Button(2, 2, "Continue shopping");
        Back.Clicked += BackToOrder;

        this.Add(Back);

        Label amountLbl = new Label(43, 6, "amount:");
        TextField amountInput = new TextField(' ' + order.SetAmount().ToString("###0.00"))
        {
            X = Pos.Right(amountLbl) + 2,
            Y = Pos.Top(amountLbl),
            Width = 8,
            ReadOnly = true,
        };
        this.Add(amountLbl, amountInput);

        Button confirmOrder = new Button("Confirm");
        confirmOrder.Clicked += ConfirmOrder;
        this.AddButton(confirmOrder);
    }

    private void OpenGood(ListViewItemEventArgs args)
    {
        Good good = (Good)args.Value;
        OpenGoodBasketDialog dialog = new OpenGoodBasketDialog(this.order, orderRepository);

        dialog.SetGood(good);


        Application.Run(dialog);
        order = dialog.order;

    }
    private void ConfirmOrder()
    {
        int index = MessageBox.Query("Confirmation", "Finished shopping", "No", "Yes");
        if (index == 1)
        {
            confirm = true;
            if (this.order.SetAmount() != 0)
            {
                this.order.id = this.orderRepository.Insert(this.order);
                this.orderRepository.AddGoodsOrders(this.order);

                MessageBox.Query("Thanks", "Your order is processed", "Back");
            }
            Application.RequestStop();
        }

    }

    private void BackToOrder()
    {
        Application.RequestStop();
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
        if (page >= (int)Math.Ceiling(this.order.goods.Length / (double)pageLength))
        {
            return;
        }
        this.page += 1;
        ShowCurrentPage();
    }
    private void ShowCurrentPage()
    {
        this.pageLabel.Text = page.ToString();
        this.totalPagesLabel.Text = ((int)Math.Ceiling(this.order.goods.Length / (double)pageLength)).ToString();
        this.allGoodsView.SetSource(GetPage(page));
    }

    private List<Good> GetPage(int page)
    {
        List<Good> goods = new List<Good>();

        for (int i = (page - 1) * 5; i < page * 5; i++)
        {
            if (order.goods.Length == i)
            {
                break;
            }
            goods.Add(order.goods[i]);
        }

        return goods;
    }
}