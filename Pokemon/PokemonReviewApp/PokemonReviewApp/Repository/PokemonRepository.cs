using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Properties.Data;
using PokemonReviewApp.Properties.Models;

namespace PokemonReviewApp.Repository 
{
    public class PokemonRepository : iPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            this._context = context;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.ID).ToList();
        }
    }
}
