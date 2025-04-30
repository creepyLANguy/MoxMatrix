namespace MoxMatrix;

public partial class Form1
{
  public class Card
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime LastScraped { get; set; }
    public bool IsScraping { get; set; }
  }
}