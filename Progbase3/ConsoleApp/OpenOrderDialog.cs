using System;
using System.Collections.Generic;
using Terminal.Gui;

public class OpenOrderDialog : Dialog
{
    private Order order;
    private ListView allGoodsView;
    private DateField createInput;
    private TextField priceInput;
    private int pageLength = 5;
    private int page = 1;
    private Label totalPagesLabel;
    private Label pageLabel;
    public OpenOrderDialog(Order order)
    {
        this.order = order;
        this.Title = "Order";

        Button okBtn = new Button("Back");
        okBtn.Clicked += DialogConfirm;

        this.AddButton(okBtn);

        int rightColumnX = 20;

        Label createLbl = new Label(2, 2, "Created at: ");
        createInput = new DateField()
        {
            X = rightColumnX,
            Y = Pos.Top(createLbl),
            Width = 40,
            ReadOnly = true,
        };
        this.Add(createLbl, createInput);

        Label priceLbl = new Label(2, 4, "Price: "); //price
        priceInput = new TextField()
        {
            X = rightColumnX,
            Y = Pos.Top(priceLbl),
            Width = 40,
            ReadOnly = true,
        };
        this.Add(priceLbl, priceInput);

        allGoodsView = new ListView(new List<Good>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };

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


    public void SetOrder(Order order)
    {
        this.order = order;
        this.createInput.Text = order.createdAt.ToShortDateString();
        this.priceInput.Text = order.amount.ToString("####.00");
    }


    private void DialogConfirm()
    {
        Application.RequestStop();
    }

}