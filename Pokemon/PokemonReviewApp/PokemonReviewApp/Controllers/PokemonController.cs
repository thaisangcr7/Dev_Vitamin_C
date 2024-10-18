
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Properties.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly iPokemonRepository _pokemonRepository;

        public PokemonController(iPokemonRepository pokemonRepository)
        {
            this._pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]

        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();
            return Ok(pokemons);

        }

        private IActionResult Ok(ICollection<Pokemon> pokemons)
        {
            throw new NotImplementedException();
        }
    }
}
