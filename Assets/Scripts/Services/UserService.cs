using System.Threading.Tasks;
using UnityEngine;

public class UserService: Singleton<UserService>
{
    public User CurrentUser { get => _currentUser; set => _currentUser = value; }

    private ApiService _apiService => ApiService.GetInstance();
    private User _currentUser;

    public UserService()
    {
        _currentUser = null;
    }

    public async Task<User> Login(string name, string password)
    {
        var body = new LoginRequest { name = name, password = password };
        Debug.Log($"UserService: Login chamado com username: {name}, password: {password}");
        Debug.Log(_apiService == null ? "API é nula" : "API OK");

        User user = await _apiService.PostJson<User>("users", body);
        CurrentUser = user;

        return user;
    }


}
