using PokemonReviewApp.Properties.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface iPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
    }
}
