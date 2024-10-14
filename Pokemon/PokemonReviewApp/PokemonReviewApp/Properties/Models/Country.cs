namespace PokemonReviewApp.Properties.Models
{
    public class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }
        
        public ICollection<Owner> Owners { get; set; }
    }
}
