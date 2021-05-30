
public class Good
{
    public long id;
    public string name;
    public string description;
    public bool inStock;
    public double price;
    public Order[] orders;

    public Good(string name, double price, bool inStock, string description)
    {
        this.name = name;
        this.price = price;
        this.inStock = inStock;
        this.description = description;
    }

    public Good()
    {
        this.id = 0;
        this.name = "";
        this.description = "";
        this.price = 0;
        this.inStock = default;
    }

    public override string ToString()
    {
        return $"[{this.id}] - {this.name}   price: {this.price} \r\n\tin stock: {this.inStock}";
    }
}
