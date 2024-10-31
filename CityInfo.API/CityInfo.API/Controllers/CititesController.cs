using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    public class CititesController : ControllerBase
    // Let have the CititesController class derives from the ControllerBase
    {
        // json result class: this result a jsonifies version of whatever we passed into the cosntuctor of jsonresult
        public JsonResult GetCities()
        {
            // because we dont have the model class yet,so we gona create objects
            // id and a name

            return new JsonResult(
                
                new List<object> 
                {
                    new {id = 1, Name = "New York City" },
                    new {id = 2, Name = "Antwerp" }
                }
               
                );

        }

    }
}
