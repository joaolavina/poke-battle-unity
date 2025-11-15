using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _pokemonNameInput;
    [SerializeField] private GameObject[] _pokemonsSlots;
    [SerializeField] private GameObject _fightButton;

    private PokemonService _pokemonService => PokemonService.GetInstance();
    private UserService _userService => UserService.GetInstance();
    private UIManager _uiManager => UIManager.GetInstance();
    private Loading _loading => Loading.GetInstance();
    private Snackbar _snackbar => Snackbar.GetInstance();
    private SpriteService _spriteService => SpriteService.GetInstance();

    private void OnEnable()
    {
        _ = LoadTeam();
    }

    private async Task LoadTeam()
    {
        try
        {
            _loading.Show();
            Pokemon[] team = await _pokemonService.GetTeam();

            for (int i = 0; i < _pokemonsSlots.Length; i++)
            {
                GameObject slot = _pokemonsSlots[i];

                if (i < team.Length)
                {
                    slot.SetActive(true);
                    Pokemon pokemon = team[i];
                    TMP_Text nameText = slot.transform.Find("PokemonName").GetComponent<TMP_Text>();
                    nameText.text = pokemon.name;
                    Slider slider = slot.transform.Find("HP_BAR").GetComponent<Slider>();
                    slider.maxValue = pokemon.hp;
                    slider.value = pokemon.currentHp;
                    TMP_Text hpText = slot.transform.Find("HP_BAR/HP_TEXT").GetComponent<TMP_Text>();
                    hpText.text = $"{pokemon.currentHp} / {pokemon.hp}";

                    Image img = slot.transform.Find("PokemonImage").GetComponent<Image>();
                    Sprite sprite = await _spriteService.DownloadSpriteAsync(pokemon.frontSprite);

                    if (sprite != null)
                        img.sprite = sprite;
                }
                else
                    slot.SetActive(false);
            }

            bool canFight = team.Length == 6;
            SetFightButton(canFight);
        }
        catch (System.Exception e)
        {
            _snackbar.ShowSnackbar("Erro ao carregar time");
            Debug.LogError($"TeamMenu: Erro ao carregar time: {e.Message}", this);
        }
        finally
        {
            _loading.Hide();
        }
    }

    public async void AddPokemon()
    {
        string pokemonName = _pokemonNameInput.text;
        if (!string.IsNullOrEmpty(pokemonName))
        {
            try
            {
                _loading.Show();
                Pokemon pokemon = await _pokemonService.GetPokemon(pokemonName);
                await _pokemonService.AddPokemonToTeam(_userService.CurrentUser.id, pokemon.id);
                await LoadTeam();
                _pokemonNameInput.text = "";
            }
            catch (System.Exception e)
            {
                _snackbar.ShowSnackbar("Erro ao adicionar Pokémon");
                Debug.LogError($"TeamMenu: Erro ao adicionar Pokémon: {e.Message}", this);
            }
            finally
            {
                _loading.Hide();
            }

        }
    }

    private void SetFightButton(bool enabled) => _fightButton.SetActive(enabled);

    public void OnFight()
    {
        _uiManager.SetUIElement("TeamPanel", false);
        _uiManager.SetUIElement("StadiumPanel", true);
    }
}
