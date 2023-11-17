namespace MonolithServer.Models;

public class Store
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string StoreDescription { get; set; }
    public string StoreBannerImgUrl { get; set; }
    public string LogoImgUrl { get; set; }
    public string ReturnAddress { get; set; }
    public string ContactNumber { get; set; }
    public string ContactEmail { get; set; }
    public string AboutUsDescription { get; set; }
}