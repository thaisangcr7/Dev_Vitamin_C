namespace CityInfo.API.Models
{
    /// <summary>
    /// A City without points of Interest
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        /// <summary>
        /// the id of the city
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// the name of the city
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of the city
        /// </summary>
        public string? Description { get; set; }


    }
}
