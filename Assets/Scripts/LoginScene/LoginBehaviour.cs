using System;
using UnityEngine;
using UnityEngine.UIElements;


public class LoginBehaviour : MonoBehaviour
{
    [SerializeField] private TextField _usernameField, _passwordField;
    [SerializeField] private String menuSceneName;
    private Loading _loading;
    private Snackbar _snackbar;
    private SceneLoader _sceneLoader;
    private UserService _userService;

    private void Awake()
    {
        _loading = Loading.GetInstance();
        _snackbar = Snackbar.GetInstance();
        _userService = UserService.GetInstance();
        _sceneLoader = SceneLoader.GetInstance();
    }

    public async void HandleLogin()
    {
        try
        {
            _loading.Show();
            // await _userService.Login(_usernameField.text, _passwordField.text);

            Debug.Log("Login successful");
            _sceneLoader.LoadSceneByName(menuSceneName);
        }
        catch (System.Exception)
        {
            _snackbar.ShowSnackbar();
        }
        finally
        {
            _loading.Hide();
        }
    }
}
