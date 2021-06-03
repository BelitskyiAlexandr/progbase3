using System;
using System.Collections.Generic;
using Terminal.Gui;

public class OrdersDialog : Dialog
{
    private GoodRepository goodRepository;
    private OrderRepository orderRepository;
    private ListView allOrdersView;
    private int pageLength = 5;
    private int page = 1;
    private Label totalPagesLabel;
    private Label pageLabel;
    public OrdersDialog()
    {
        this.Title = "Orders";

        Button okBtn = new Button("Back");
        okBtn.Clicked += DialogConfirm;

        this.AddButton(okBtn);

        allOrdersView = new ListView(new List<User>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        allOrdersView.OpenSelectedItem += OpenOrder;

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
        frameView.Add(allOrdersView);

        this.Add(frameView);

    }
    private void OpenOrder(ListViewItemEventArgs args)
    {
        Order order = (Order)args.Value;
        order.goods = goodRepository.GetGoodsByOrderId(order.id);
        OpenOrderDialog openOrderDialog = new OpenOrderDialog(order);

        openOrderDialog.SetOrder(order);

        Application.Run(openOrderDialog);
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
        if (page >= (int)Math.Ceiling(this.orderRepository.GetAll().Count / (double)pageLength))
        {
            return;
        }
        this.page += 1;
        ShowCurrentPage();
    }
    private void ShowCurrentPage()
    {
        this.pageLabel.Text = page.ToString();
        this.totalPagesLabel.Text = ((int)Math.Ceiling(this.orderRepository.GetAll().Count / (double)pageLength)).ToString();
        this.allOrdersView.SetSource(GetPage(page));
    }

    private List<Order> GetPage(int page)
    {
        List<Order> orders = new List<Order>();

        List<Order> bdOrders = orderRepository.GetAll();
        for (int i = (page - 1) * 5; i < page * 5; i++)
        {
            if (i == bdOrders.Count)
            {
                break;
            }
            orders.Add(bdOrders[i]);
        }

        return orders;
    }


    public void SetRepository(GoodRepository goodRepository, OrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
        this.goodRepository = goodRepository;
        ShowCurrentPage();
    }


    private void DialogConfirm()
    {
        Application.RequestStop();
    }

}