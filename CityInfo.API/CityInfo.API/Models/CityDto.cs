namespace CityInfo.API.Models
{
    public class CityDto
    {
        

        // Add three properties into thsi class

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int NumberOfPointOfInterest
        { 
            get
            {
                return PointsOfInterest.Count;
            }
        }

        public ICollection<PointOfInterestDto> PointsOfInterest { get; set;} 
            = new List<PointOfInterestDto>();
      
    }
}
