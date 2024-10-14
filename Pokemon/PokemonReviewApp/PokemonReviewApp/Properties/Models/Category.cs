namespace PokemonReviewApp.Properties.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
