using System.Net.WebSockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleScript : MonoBehaviour
{
    [SerializeField] private GameObject _pokemonPlayer, _pokemonOpponent, _pokemonPlayerHealthBar, _pokemonOpponentHealthBar, _panelBtn, _waitingPlayersPanel;
    [SerializeField] private TextMeshProUGUI _battleLogText;

    private StadiumService _stadiumService => StadiumService.GetInstance();
    private UserService _userService => UserService.GetInstance();
    private Loading _loading => Loading.GetInstance();
    private Snackbar _snackbar => Snackbar.GetInstance();
    private SpriteService _spriteService => SpriteService.GetInstance();
    private BattleWebSocket _battleWebSocket => BattleWebSocket.GetInstance();
    private User _opponentUser;

    private string _battleId;


    void Awake()
    {
        Instantiate(_stadiumService.Stadium);
        RenderPokemonSprite("player", _userService.CurrentUser.team[0]);

    }

    void OnEnable()
    {
        _battleWebSocket.BattleStart += BattleStart;
        _battleWebSocket.PlayerAction += ChooseCommand;
        // _battleWebSocket.TurnResult += BattleStart;
        // _battleWebSocket.BattleEnd += ChooseCommand;
    }

    void OnDisable()
    {
        _battleWebSocket.BattleStart -= BattleStart;
        _battleWebSocket.PlayerAction -= ChooseCommand;
        // _battleWebSocket.TurnResult -= BattleStart;
        // _battleWebSocket.BattleEnd -= ChooseCommand;
    }

    private async void BattleStart(BattleMessageDTO dto)
    {
        _waitingPlayersPanel.SetActive(false);

        if (dto.opponentName == _userService.CurrentUser.name)
        {
            _opponentUser = await _userService.GetUserByName(dto.user.name);
            LogBattleEvent("Esperando a vez do oponente.");
        }
        else
        {
            _opponentUser = await _userService.GetUserByName(dto.opponentName);
            SetButtons(true);
        }

        _battleId = dto.battleId;
        _pokemonOpponent.SetActive(true);
        _pokemonOpponentHealthBar.SetActive(true);
        RenderPokemonSprite("opponent", _opponentUser.team[0]);
    }

    private void ChooseCommand(BattleMessageDTO dto)
    {
        LogBattleEvent("Seu turno. Escolha uma acao");
        SetButtons(true);
    }

    public async void Attack()
    {
        try
        {
            _loading.Show();
            await _stadiumService.Attack(_userService.CurrentUser.id, _battleId);
            SetButtons(false);
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
            await _stadiumService.Flee(_userService.CurrentUser.id, _battleId);
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
            await _stadiumService.SwitchPokemon(_userService.CurrentUser.id, _battleId, pokemonIndex);
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

    private async void RenderPokemonSprite(string user, Pokemon pokemon)
    {
        try
        {
            _loading.Show();
            UpdatePokemonStats(user, pokemon);
            Sprite pokemonSprite;

            if (user == "player")
            {
                pokemonSprite = await _spriteService.DownloadSpriteAsync(pokemon.backSprite);
                if (pokemonSprite != null)
                    _pokemonPlayer.GetComponent<SpriteRenderer>().sprite = pokemonSprite;
            }
            else
            {
                pokemonSprite = await _spriteService.DownloadSpriteAsync(pokemon.frontSprite);
                if (pokemonSprite != null)
                    _pokemonOpponent.GetComponent<SpriteRenderer>().sprite = pokemonSprite;
            }

            pokemonSprite.texture.filterMode = FilterMode.Point;
            return;

        }
        catch (System.Exception)
        {
            _snackbar.ShowSnackbar("Erro ao carregar sprite do Pokémon");
        }
        finally
        {
            _loading.Hide();
        }

    }

    private void UpdatePokemonStats(string user, Pokemon pokemon)
    {
        if (user == "player")
        {
            _pokemonPlayerHealthBar.transform.Find("PokeName").GetComponent<TextMeshProUGUI>().text = pokemon.name;
        }
        else
        {
            _pokemonOpponentHealthBar.transform.Find("PokeName").GetComponent<TextMeshProUGUI>().text = pokemon.name;
        }

        UpdatePokemonHealthBar(user, pokemon);
    }

    private void UpdatePokemonHealthBar(string user, Pokemon pokemon)
    {
        Slider slider;
        TextMeshProUGUI hpText;

        if (user == "player")
        {
            slider = _pokemonPlayerHealthBar.transform.Find("HP_BAR").GetComponent<Slider>();
            hpText = _pokemonPlayerHealthBar.transform.Find("HP_BAR/HP_TEXT").GetComponent<TextMeshProUGUI>();

        }
        else
        {
            slider = _pokemonOpponentHealthBar.transform.Find("HP_BAR").GetComponent<Slider>();
            hpText = _pokemonOpponentHealthBar.transform.Find("HP_BAR/HP_TEXT").GetComponent<TextMeshProUGUI>();
        }

        slider.maxValue = pokemon.hp;
        slider.value = pokemon.currentHp;
        hpText.text = $"{pokemon.currentHp} / {pokemon.hp}";
    }

    private void LogBattleEvent(string message)
    {
        _battleLogText.text = message;
    }

    private void SetButtons(bool state)
    {
        foreach (Transform btn in _panelBtn.transform)
        {
            btn.gameObject.GetComponent<Button>().interactable = state;
        }
    }
}
