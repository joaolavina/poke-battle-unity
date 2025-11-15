using System;
using System.Threading.Tasks;
using UnityEngine;

public class PokemonService : Singleton<PokemonService>
{
    public Pokemon[] CurrentTeam { get => _currentTeam; set => _currentTeam = value; }
    private ApiService _apiService => ApiService.GetInstance();
    private UserService _userService => UserService.GetInstance();
    private Pokemon[] _currentTeam;


    public async Task<Pokemon[]> GetTeam()
    {
        User user = await _apiService.GetJson<User>($"users/{_userService.CurrentUser.id}");
        _currentTeam = user.team;
        return _currentTeam;
    }

    public async Task<Pokemon> GetPokemon(string name)
    {
        Pokemon pokemon = await _apiService.GetJson<Pokemon>($"pokemon/{name}");
        return pokemon;
    }

    public async Task<User> AddPokemonToTeam(int userId, int pokemonId)
    {
        var body = new AddPokemonRequest { pokemonId = pokemonId };

        User user = await _apiService.PostJson<User>($"users/{userId}/team", body);
        return user;
    }

    
}
 