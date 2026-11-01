namespace TravelNest_APIGateway.Models;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AccommodationId { get; set; }
    public int? RoomId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
}