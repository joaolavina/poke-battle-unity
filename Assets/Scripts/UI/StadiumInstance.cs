using UnityEngine;
using UnityEngine.SceneManagement;

public class StadiumInstance : MonoBehaviour
{
    [SerializeField] private GameObject _teamPanel;
    [SerializeField] private GameObject _stadium;

    private StadiumService _stadiumService => StadiumService.GetInstance();
    private UserService _userService => UserService.GetInstance();
    private UIManager _uiManager => UIManager.GetInstance();
    private Loading _loading => Loading.GetInstance();
    private Snackbar _snackbar => Snackbar.GetInstance();
    public async void EnterStadium()
    {
        try
        {
            _loading.Show();
            // string result = await _stadiumService.EnterStadium(_userService.CurrentUser.id, _stadium);
            // Debug.Log($"StadiumInstance: Resultado ao entrar no estádio: {result}");

            _stadiumService.Stadium = _stadium;
            _uiManager.SetUIElement("StadiumPanel", false);
            SceneManager.LoadScene("Battle");
        } catch (System.Exception e)
        {
            _snackbar.ShowSnackbar("Erro ao entrar no estádio");
            Debug.LogError($"StadiumInstance: Erro ao entrar no estádio: {e.Message}", this);
        } finally
        {
            _loading.Hide();
        }

    }
}
