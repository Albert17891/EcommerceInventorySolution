namespace EcommerceInventory.Domain.Entities;
public class Product
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = null!;
    public int Stock { get; private set; }
    public decimal Price { get; private set; }

    // Constructor for EF
    protected Product() { }

    public Product(string name, int stock, decimal price)
    {
        Name = name;
        Stock = stock;
        Price = price;
    }

    public bool TryPurchase(int quantity)
    {
        if (Stock > quantity)
        {
            Stock -= quantity;

            return true;
        }

        return false;
    }

    public void Restock(int quantity)
    {
        Stock += quantity;
    }
}
