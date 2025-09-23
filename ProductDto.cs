public class ProductDto
{
    public int ProductID { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageURL { get; set; }
    public bool IsAvailable { get; set; }
    public string CategoryName { get; set; }
    public string? BrandName { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; } // ? Add this property to fix CS1061
}