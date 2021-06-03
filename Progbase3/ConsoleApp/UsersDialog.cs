using System;
using System.Collections.Generic;
using Terminal.Gui;

public class UsersDialog : Dialog
{
    private UserRepository userRepository;
    private ListView allUsersView;
    private int pageLength = 5;
    private int page = 1;
    private Label totalPagesLabel;
    private Label pageLabel;
    public UsersDialog()
    {
        this.Title = "Users";

        Button okBtn = new Button("Back");
        okBtn.Clicked += DialogConfirm;

        this.AddButton(okBtn);

        allUsersView = new ListView(new List<User>())
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
        frameView.Add(allUsersView);

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
        if (page >= (int)Math.Ceiling(this.userRepository.AllRecords().Count / (double)pageLength))
        {
            return;
        }
        this.page += 1;
        ShowCurrentPage();
    }
    private void ShowCurrentPage()
    {
        this.pageLabel.Text = page.ToString();
        this.totalPagesLabel.Text = ((int)Math.Ceiling(this.userRepository.AllRecords().Count / (double)pageLength)).ToString();
        this.allUsersView.SetSource(GetPage(page));
    }

    private List<User> GetPage(int page)
    {
        List<User> users = new List<User>();

        List<User> bdUsers = userRepository.AllRecords();
        for (int i = (page - 1) * 5; i < page * 5; i++)
        {
            if (i == bdUsers.Count)
            {
                break;
            }
            users.Add(bdUsers[i]);
        }

        return users;
    }


    public void SetRepository(UserRepository userRepository)
    {
        this.userRepository = userRepository;
        ShowCurrentPage();
    }


    private void DialogConfirm()
    {
        Application.RequestStop();
    }

}