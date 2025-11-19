using System;
using System.Threading.Tasks;
using UnityEngine;

public class StadiumService: Singleton<StadiumService>
{
    public GameObject Stadium { get => _stadium; set => _stadium = value; }
    private ApiService _apiService => ApiService.GetInstance();
    private GameObject _stadium;

    public async Task<string> EnterStadium(int idUser, GameObject stadium)
    {
        var result = await _apiService.PostJson<string>($"stadium/{idUser}/enter/{stadium.name}");
        Stadium = stadium;
        return result;
    }

    public async Task<string> Attack(int idUser, int battleId)
    {
        var result = await _apiService.PostJson<string>($"users/{idUser}/attack?battleId={battleId}");
        return result;
    }
    
    public async Task<string> Flee(int idUser, int battleId)
    {
        var result = await _apiService.PostJson<string>($"users/{idUser}/flee?battleId={battleId}");
        return result;
    }

    public async Task<string> SwitchPokemon(int idUser, int battleId, int pokemonIndex)
    {
        var result = await _apiService.PostJson<string>($"users/{idUser}/switch-pokemon?battleId={battleId}&pokemonIndex={pokemonIndex}");
        return result;
    }
}
