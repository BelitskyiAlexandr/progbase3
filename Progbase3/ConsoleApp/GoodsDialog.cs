using System;
using System.Collections.Generic;
using Terminal.Gui;

public class GoodsDialog : Dialog
{
    private GoodRepository goodRepository;
    private ListView allGoodsView;
    private int pageLength = 5;
    private int page = 1;
    private Label totalPagesLabel;
    private Label pageLabel;
    public GoodsDialog()
    {
        this.Title = "Goods";

        Button okBtn = new Button("Back");
        okBtn.Clicked += DialogConfirm;

        this.AddButton(okBtn);

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

    }
    private void OpenGood(ListViewItemEventArgs args)
    {
        Good good = (Good)args.Value;
        OpenModerGoodDialog openOrderDialog = new OpenModerGoodDialog();

        openOrderDialog.SetGood(good);

        Application.Run(openOrderDialog);

        if (openOrderDialog.deleted)
        {
            bool result = goodRepository.Delete(good);
            if (result)
            {
                int pages = goodRepository.GetTotalPages();
                if (page > pages && page > 1)
                {
                    page -= 1;
                    this.ShowCurrentPage();
                }

                allGoodsView.SetSource(goodRepository.GetPage(page));
            }
            else
            {
                MessageBox.ErrorQuery("Delete activity", "Can not delete activity", "OK");
            }
        }
        if (openOrderDialog.updated)
        {
            bool result = goodRepository.Update(good.id, openOrderDialog.good);
            if (result)
            {
                allGoodsView.SetSource(goodRepository.GetPage(page));
            }
            else
            {
                MessageBox.ErrorQuery("Update activity", "Can not update activity", "OK");
            }
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
        if (page >= (int)Math.Ceiling(this.goodRepository.GetAll().Count / (double)pageLength))
        {
            return;
        }
        this.page += 1;
        ShowCurrentPage();
    }
    private void ShowCurrentPage()
    {
        this.pageLabel.Text = page.ToString();
        this.totalPagesLabel.Text = ((int)Math.Ceiling(this.goodRepository.GetAll().Count / (double)pageLength)).ToString();
        this.allGoodsView.SetSource(GetPage(page));
    }

    private List<Good> GetPage(int page)
    {
        List<Good> goods = new List<Good>();

        List<Good> bdGoods = goodRepository.GetAll();
        for (int i = (page - 1) * 5; i < page * 5; i++)
        {
            if (i == bdGoods.Count)
            {
                break;
            }
            goods.Add(bdGoods[i]);
        }

        return goods;
    }


    public void SetRepository(GoodRepository goodRepository)
    {
        this.goodRepository = goodRepository;
        ShowCurrentPage();
    }


    private void DialogConfirm()
    {
        Application.RequestStop();
    }
}