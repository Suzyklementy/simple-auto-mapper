namespace Test.Objects;

public class Clothes
{
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string Category { get; set; }
    public string Size { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }

    public IEnumerable<string> ImagesUrl { get; set; }
}