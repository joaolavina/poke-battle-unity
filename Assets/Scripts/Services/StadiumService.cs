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
        var result = await _apiService.PostJson<string>($"users/{idUser}/enter-stadium");
        Stadium = stadium;
        return result;
    }
}
