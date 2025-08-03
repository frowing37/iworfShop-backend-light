namespace iworfShop_backend_light.Models.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ParentName { get; set; }
}