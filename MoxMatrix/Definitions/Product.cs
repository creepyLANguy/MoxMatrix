namespace MoxMatrix;

public partial class Form1
{
  public record Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Price { get; set; }
    public string PriceRead { get; set; }
    public string Link { get; set; }
    public int Stock { get; set; }
    public bool Is_Foil { get; set; }
    public DateTime LastScraped { get; set; }
    public string Image { get; set; }
    public int Retailer_Id { get; set; }
    public string Retailer_Name { get; set; }
    public string Currency { get; set; }
  }
}