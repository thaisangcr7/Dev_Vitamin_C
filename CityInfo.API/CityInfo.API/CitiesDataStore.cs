using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        // Add a properties, which is a list of CityDto in Models
        public List<CityDto> Cities { get; set; }

        // Also a static property which will return the instance of the Current
        public static CitiesDataStore Current { get; } = new CitiesDataStore(); // This allow us the to work on the same store as long as we dont restart the webserver 


        public CitiesDataStore()
        {
            // initalize the data using the constuct
            Cities = new List<CityDto>

             {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park."
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished."
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with really big tower."
                },

             };



        }

    }
}
