using System;
using System.Threading.Tasks;
using UnityEngine;

public class StadiumService: Singleton<StadiumService>
{
    public GameObject Stadium { get => _stadium; set => _stadium = value; }
    private ApiService _apiService => ApiService.GetInstance();
    private GameObject _stadium;

    public async Task<EnterStadiumResponse> EnterStadium(int idUser, GameObject stadium)
    {
        Debug.Log($"StadiumService: Entrando no estádio {stadium.name} para o usuário {idUser}");
        var result = await _apiService.PostJson<EnterStadiumResponse>($"stadium/{idUser}/enter/{stadium.name}");
        Stadium = stadium;
        return result;
    }

    public async Task<string> Attack(int idUser, string battleId)
    {
        var result = await _apiService.PostJson<string>($"battle/{idUser}/attack?battleId={battleId}");
        return result;
    }
    
    public async Task<string> Flee(int idUser, string battleId)
    {
        var result = await _apiService.PostJson<string>($"users/{idUser}/flee?battleId={battleId}");
        return result;
    }

    public async Task<string> SwitchPokemon(int idUser, string battleId, int pokemonIndex)
    {
        var result = await _apiService.PostJson<string>($"users/{idUser}/switch-pokemon?battleId={battleId}&pokemonIndex={pokemonIndex}");
        return result;
    }

    public async Task<StadiumResponse> GetAvailableStadiums()
    {
        StadiumResponse stadiumResponse = await _apiService.GetJson<StadiumResponse>("stadium/available");
        return stadiumResponse;
    }

    public async Task <StadiumStatusDTO> GetStadiumStatus()
    {
        StadiumStatusDTO stadiumStatus = await _apiService.GetJson<StadiumStatusDTO>($"{_stadium.name}/status");
        return stadiumStatus;
    }
}
