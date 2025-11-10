using UnityEngine;

public class UserService: Singleton<UserService>
{
    private ApiService _apiService => ApiService.GetInstance();

    public async void Login(string username, string password)
    {
        await _apiService.Get("https://example.com/login");
    }
}
