using TMPro;
using UnityEngine;


public class LoginBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameField, _passwordField;
    [SerializeField] private GameObject _loginPanel, _stadiumPanel;

    private Loading _loading => Loading.GetInstance();
    private Snackbar _snackbar => Snackbar.GetInstance();
    private UIManager _uiManager => UIManager.GetInstance();
    private UserService _userService => UserService.GetInstance();

    private void Awake()
    {
        _uiManager.RegisterElement("LoginPanel", _loginPanel);
        _uiManager.RegisterElement("StadiumPanel", _stadiumPanel);
    }

    public async void HandleLogin()
    {
        try
        {
            _loading.Show();
            await _userService.Login(_usernameField.text, _passwordField.text);

            _uiManager.SetUIElement("LoginPanel", false);
            _uiManager.SetUIElement("StadiumPanel", true);
        }
        catch (System.Exception e)
        {
            _snackbar.ShowSnackbar("Erro ao logar");
            Debug.LogError($"LoginBehaviour: Erro ao logar: {e.Message}", this);
        }
        finally
        {
            _loading.Hide();
        }
    }
}
