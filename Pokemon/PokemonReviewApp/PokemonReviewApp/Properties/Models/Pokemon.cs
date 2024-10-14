using System.Security.Cryptography.X509Certificates;

namespace PokemonReviewApp.Properties.Models
{
    public class Pokemon
    {
        public int ID { get; set; }
        public string Name {  get; set; }
        public DateTime Birthdate { get; set; }

        public ICollection<Review> Reviews { get; set; }
        
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
