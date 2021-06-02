
using System.Collections.Generic;
using Terminal.Gui;

public class OrderWindow : Window
{
    private ListView allActivitiesView;
    private GoodRepository goodRepository;
    private int pageLength = 5;
    private int page = 1;

    private Label totalPagesLabel;
    private Label pageLabel;

    public OrderWindow()
    {
        this.Title = "Goods list";

        Rect frame = new Rect(4, 8, 40, 20);
        allActivitiesView = new ListView(new List<Good>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        allActivitiesView.OpenSelectedItem += OpenActivity;


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

        FrameView frameView = new FrameView("Activities")
        {
            X = 2,
            Y = 8,
            Width = Dim.Fill() - 4,
            Height = pageLength + 2,
        };
        frameView.Add(allActivitiesView);

        this.Add(frameView);
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
        this.allActivitiesView.SetSource(goodRepository.GetPage(page));
    }

    public void SetRepository(GoodRepository repository)
    {
        this.goodRepository = repository;
        ShowCurrentPage();
    }

    private void OpenActivity(ListViewItemEventArgs args)
    {
        Good activity = (Good)args.Value;
        OpenGoodDialog dialog = new OpenGoodDialog();

        dialog.SetGood(activity);

        Application.Run(dialog);

        //allActivitiesView.SetSource(goodRepository.GetPage(page));

    }
}
