namespace Store.Domain.Entities;

public class Product : Entity
{
    public Product(string title, decimal price, bool ative = true)
    {
        Title = title;
        Price = price;
        Ative = ative;
    }

    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public bool Ative { get; private set; }
}