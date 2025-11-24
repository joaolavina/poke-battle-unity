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

    public async Task<User> Login(string name)
    {
        var body = new LoginRequest { name = name };

        User user = await _apiService.PostJson<User>("users", body);
        CurrentUser = user;

        return user;
    }

    public async Task<User> GetUserByName(string name)
    {
        User user = await _apiService.GetJson<User>($"users/name/{name}");
        return user;
    }    
}
