using UnityEngine;

public class StadiumInstance : MonoBehaviour
{
    [SerializeField] private GameObject _teamPanel;

    private StadiumService _stadiumService => StadiumService.GetInstance();
    private UserService _userService => UserService.GetInstance();
    private UIManager _uiManager => UIManager.GetInstance();
    private Loading _loading => Loading.GetInstance();
    private Snackbar _snackbar => Snackbar.GetInstance();

    private void Awake()
    {
        _uiManager.RegisterElement("TeamPanel", _teamPanel);
    }

    public async void EnterStadium()
    {
        try
        {
            _loading.Show();
            string result = await _stadiumService.EnterStadium(_userService.CurrentUser.id);
            Debug.Log($"StadiumInstance: Resultado ao entrar no estádio: {result}");
            _uiManager.SetUIElement("StadiumPanel", false);
            _uiManager.SetUIElement("TeamPanel", true);
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
