namespace TravelNest_APIGateway.Models;

public class Accommodation
{
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinPrice { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal Rate { get; set; }
        public int MaxGuests { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int CategoryId { get; set; }
        public int Cleanliness { get; set; }
        public int Location { get; set; }
        public int Luxury { get; set; }
        public int Family_friendly { get; set; }
        

        // Frontend šalje JSON string, npr. "[1,2,3]"
        public string FeatureIds { get; set; }

        public List<IFormFile> Images { get; set; } = new();
    
}