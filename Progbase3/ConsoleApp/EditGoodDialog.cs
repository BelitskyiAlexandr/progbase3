using System;
using Terminal.Gui;

public class EditGoodGialog :Dialog
{
    protected Good good;

    private TextField titleInput;
    private TextView descriptionInput;
    private CheckBox inStockValue;
    private TextField priceInput;
    public EditGoodGialog()
    {
        int rightColumnX = 20;

        Label titleLbl = new Label(2, 2, "Title: ");
        titleInput = new TextField("")
        {
            X = rightColumnX,
            Y = Pos.Top(titleLbl),
            Width = 40,
        };
        this.Add(titleLbl, titleInput);

        Label priceLbl = new Label(2, 4, "Price: "); //price
        priceInput = new TextField()
        {
            X = rightColumnX,
            Y = Pos.Top(priceLbl),
            Width = 40,
        };
        this.Add(priceLbl, priceInput);

        Label inStockLbl = new Label(2, 6, "In stock: ");
        inStockValue = new CheckBox("")
        {
            X = rightColumnX,
            Y = Pos.Top(inStockLbl),
            Width = 40,
        };
        this.Add(inStockLbl, inStockValue);

        Label descriptionLbl = new Label(2, 8, "Description: "); //desc
        descriptionInput = new TextView()
        {
            X = rightColumnX,
            Y = 8,
            Width = 40,
            Height = 3,
        };
        this.Add(descriptionLbl, descriptionInput);

        Button ok = new Button("Ok");
        ok.Clicked += DialogConfirm;
        this.AddButton(ok);
    }
      private void DialogConfirm()
    {
        Application.RequestStop();
    }

    public void SetGood(Good good)
    {
        this.good = good;
        this.titleInput.Text = good.name;
        this.descriptionInput.Text = good.description;
        this.inStockValue.Checked = Convert.ToBoolean(good.inStock.ToString()) ;
        this.priceInput.Text = good.price.ToString();
    }

    public Good GetGood()
    {
        if (!double.TryParse(priceInput.Text.ToString(), out double result))
        {
            MessageBox.ErrorQuery("Error", $"Price must be real number but have {priceInput.Text.ToString()}", "OK");
            return null;
        }
        return new Good()
        {
            name = titleInput.Text.ToString(),
            inStock = inStockValue.Checked,
            description = descriptionInput.Text.ToString(),
            price = result,
        };
    }
}