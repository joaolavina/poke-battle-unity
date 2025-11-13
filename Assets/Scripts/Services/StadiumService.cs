using System;
using System.Threading.Tasks;

public class StadiumService: Singleton<StadiumService>
{
    private ApiService _apiService => ApiService.GetInstance();

    public async Task<string> EnterStadium(int idUser)
    {
        var result = await _apiService.PostJson<string>($"users/{idUser}/enter-stadium");
        return result;
    }
}
