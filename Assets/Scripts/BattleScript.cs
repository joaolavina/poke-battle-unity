using System.Threading.Tasks;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    private StadiumService _stadiumService => StadiumService.GetInstance();
    private UserService _userService => UserService.GetInstance();
    private Loading _loading => Loading.GetInstance();
    private Snackbar _snackbar => Snackbar.GetInstance();
    private GameObject _stadiumInstance;
    private User _currentUser => _userService.CurrentUser;

    private int _battleId;


    void Awake()
    {
        _stadiumInstance = Instantiate(_stadiumService.Stadium);
    }

    public async Task Attack()
    {
        try
        {
            _loading.Show();
            await _stadiumService.Attack(_currentUser.id, _battleId);
        }
        catch (System.Exception e)
        {
            _snackbar.ShowSnackbar("Erro ao realizar ataque");
            Debug.LogError($"BattleScript: Erro ao realizar ataque: {e.Message}", this);
        }
        finally
        {
            _loading.Hide();
        }
    }

    public async Task Flee()
    {
        try
        {
            _loading.Show();
            await _stadiumService.Flee(_currentUser.id, _battleId);
        }
        catch (System.Exception e)
        {
            _snackbar.ShowSnackbar("Erro ao fugir da batalha");
            Debug.LogError($"BattleScript: Erro ao fugir da batalha: {e.Message}", this);
        }
        finally
        {
            _loading.Hide();
        }
    }

    public async Task SwitchPokemon(int pokemonIndex)
    {
        try
        {
            _loading.Show();
            await _stadiumService.SwitchPokemon(_currentUser.id, _battleId, pokemonIndex);
        }
        catch (System.Exception e)
        {
            _snackbar.ShowSnackbar("Erro ao trocar de Pokémon");
            Debug.LogError($"BattleScript: Erro ao trocar de Pokémon: {e.Message}", this);
        }
        finally
        {
            _loading.Hide();
        }
    }
}
