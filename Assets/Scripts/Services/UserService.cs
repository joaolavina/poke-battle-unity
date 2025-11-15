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

        User user = await _apiService.PostJson<User>("users", body);
        CurrentUser = user;

        return user;
    }


}
