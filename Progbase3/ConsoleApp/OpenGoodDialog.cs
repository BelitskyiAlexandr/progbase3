using System;
using Terminal.Gui;

public class OpenGoodDialog : Dialog
{
    public bool deleted;
    public bool updated;

    protected Good good;

    private TextField titleInput;
    private TextView descriptionInput;
    private TextField inStockValueLbl;
    private TextField priceInput;

    public OpenGoodDialog()
    {
        this.Title = "Good";

        Button okBtn = new Button("Back");
        okBtn.Clicked += DialogConfirm;

        this.AddButton(okBtn);

        int rightColumnX = 20;

        Label titleLbl = new Label(2, 2, "Title: ");
        titleInput = new TextField("")
        {
            X = rightColumnX,
            Y = Pos.Top(titleLbl),
            Width = 40,
            ReadOnly = true,
        };
        this.Add(titleLbl, titleInput);

        Label priceLbl = new Label(2, 4, "Price: "); //price
        priceInput = new TextField()
        {
            X = rightColumnX,
            Y = Pos.Top(priceLbl),
            Width = 40,
            ReadOnly = true,
        };
        this.Add(priceLbl, priceInput);

        Label inStockLbl = new Label(2, 6, "In stock: ");
        inStockValueLbl = new TextField() 
        {
            X = rightColumnX,
            Y = Pos.Top(inStockLbl),
            Width = 40,
            ReadOnly = true,
        };
        this.Add(inStockLbl, inStockValueLbl);

        Label descriptionLbl = new Label(2, 8, "Description: "); //desc
        descriptionInput = new TextView()
        {
            X = rightColumnX,
            Y = 8,
            Width = 40,
            Height = 3,
            ReadOnly = true,
        };
        this.Add(descriptionLbl, descriptionInput);
    }


    public Good GetGood()
    {
        return this.good;
    }

    public void SetGood(Good good)
    {
        this.good = good;
        this.titleInput.Text = good.name;
        this.descriptionInput.Text = good.description;        
        this.inStockValueLbl.Text = good.inStock.ToString();  
        this.priceInput.Text = good.price.ToString();
    }


    private void DialogConfirm()
    {
        Application.RequestStop();
    }

}