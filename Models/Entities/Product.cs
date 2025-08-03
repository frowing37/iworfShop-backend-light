namespace iworfShop_backend_light.Models.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Sku { get; set; }
    public string BaseSku { get; set; }
    public string Name { get; set; }
    public Brand Brand { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public string Gender { get; set; }
    public string Color { get; set; }
    public string ColorCode { get; set; }
    public string Size { get; set; }
    public decimal Price { get; set; }
    public decimal Oldprice { get; set; }
    public List<string> ImageUrls { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public List<string> Tags { get; set; }
    public Dictionary<string, string> CustomValues { get; set; }
}