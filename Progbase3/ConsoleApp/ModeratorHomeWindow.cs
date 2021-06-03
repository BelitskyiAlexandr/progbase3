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
    public void SetXml(XmlProcess xmlProcess)
    {
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

    static Dialog dialog;
    static Label fileLabel;
    static TextField substringInput;
    public void ClickExport()
    {
        dialog = new Dialog("Export");
        Button deleteBtn = new Button(3, 2, "Open file");
        deleteBtn.Clicked += SelectFile;

        fileLabel = new Label("default")
        {
            X = Pos.Right(deleteBtn) + 2,
            Y = Pos.Top(deleteBtn),
            Width = Dim.Fill(),
        };

        dialog.Add(deleteBtn, fileLabel);


        Label substringLbl = new Label(2, 4, "Enter substring: ");
        substringInput = new TextField()
        {
            X = Pos.Right(substringLbl),
            Y = Pos.Top(substringLbl),
            Width = 15,
        };
        dialog.Add(substringLbl, substringInput);

        Button exportBtn = new Button(2, 7, "Export");
        exportBtn.Clicked += ExportProcess;
        dialog.Add(exportBtn);


        Application.Run(dialog);

    }
    public void ExportProcess()
    {
        string filePath = fileLabel.Text.ToString();
        string substring = substringInput.Text.ToString();
        if (filePath == "default" || !filePath.EndsWith(".xml"))
        {
            MessageBox.ErrorQuery("Filepath", "Choose Xml file", "Back");
        }
        else if (substring == "")
        {
            MessageBox.ErrorQuery("Substring", "Enter substring", "Back");
        }
        else
        {
            xmlProcess.XmlExport(goodRepository.GetExportGoods(substring), filePath);
            MessageBox.Query("Export", "Exported successfully", "Back");
            Application.RequestStop();
        }
    }
    static void SelectFile()
    {
        OpenDialog dialog = new OpenDialog("Open directory", "Open?");
        dialog.CanChooseDirectories = false;
        dialog.CanChooseFiles = true;
        Application.Run(dialog);

        if (!dialog.Canceled)
        {
            NStack.ustring filePath = dialog.FilePath;
            fileLabel.Text = filePath;
        }
        else
        {
            fileLabel.Text = "not selected.";
        }


    }
    public void ClickImport()
    {
        dialog = new Dialog("Import");
        Button deleteBtn = new Button(3, 2, "Open file");
        deleteBtn.Clicked += SelectFile;

        fileLabel = new Label("default")
        {
            X = Pos.Right(deleteBtn) + 2,
            Y = Pos.Top(deleteBtn),
            Width = Dim.Fill(),
        };

        dialog.Add(deleteBtn, fileLabel);


        Button importBtn = new Button(2, 7, "Import");
        importBtn.Clicked += ImportProcess;
        dialog.Add(importBtn);

        Application.Run(dialog);
    }

    public void ImportProcess()
    {
        string filePath = fileLabel.Text.ToString();
        if (filePath == "default" || !filePath.EndsWith(".xml"))
        {
            MessageBox.ErrorQuery("Filepath", "Choose Xml file", "Back");
        }
        else
        {
            xmlProcess.XmlImport(filePath, goodRepository);
            MessageBox.Query("Import", "Imported successfully", "Back");
            Application.RequestStop();
        }
    }
}