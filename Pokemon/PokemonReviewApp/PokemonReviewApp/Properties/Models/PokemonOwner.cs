namespace PokemonReviewApp.Properties.Models
{
    public class PokemonOwner
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }

    }
}
