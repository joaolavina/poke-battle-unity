using System;
using UnityEngine;

public class PokemonService : Singleton<PokemonService>
{
    private ApiService _apiService => ApiService.GetInstance();

    public void GetTeam()
    {
        
    }
}
