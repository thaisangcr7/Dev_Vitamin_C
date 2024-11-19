using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // generation of the primary key id
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]

        public string Description { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; } // navigation property
        public int CityId { get; set; }

        public PointOfInterest(string name) // dependent class 
        {
            Name = name;
        }

    }
}
