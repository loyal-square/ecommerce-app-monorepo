namespace MonolithServer.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DeliveryAddressId { get; set; }
    public int ContactInfoId { get; set; }
}