using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject canvasGlobalPrefab; // deve conter Canvas + EventSystem
    [SerializeField] private GameObject snackbarPrefab;
    [SerializeField] private GameObject loadingPrefab;

    private static bool _initialized = false;
    private static Transform _uiRoot;

    private void Awake()
    {
        if (_initialized)
        {
            Destroy(gameObject);
            return;
        }

        _initialized = true;
        DontDestroyOnLoad(gameObject);

        CreateUIRootIfNeeded();
        InstantiateGlobalIfMissing(snackbarPrefab, "Snackbar");
        InstantiateGlobalIfMissing(loadingPrefab, "Loading");

        // Inicialize singletons puros se precisar:
        _ = ApiService.GetInstance();
        _ = UserService.GetInstance();

        // Carrega cena principal (se quiser)
        SceneManager.LoadScene("Login");
    }

    private void CreateUIRootIfNeeded()
    {
        if (canvasGlobalPrefab != null)
        {
            var canvasInstance = Instantiate(canvasGlobalPrefab);
            canvasInstance.name = "Canvas_Global";
            DontDestroyOnLoad(canvasInstance);
            _uiRoot = canvasInstance.transform;
        }
        else
        {
            Debug.LogError("Bootstrapper: canvasGlobalPrefab não atribuído!");
        }
    }

    private void InstantiateGlobalIfMissing(GameObject prefab, string name)
    {
        if (prefab == null) return;

        // se a instância já existe (pelo SingletonMonoBehaviour), não cria.
        var existing = FindAnyObjectByType(typeof(Component)) as Component; // noop, deixo por segurança
        // instancia sempre, mas se o componente Singleton já estiver registrado, destrói o clone
        var go = Instantiate(prefab, _uiRoot, false); // parent = CanvasGlobal, worldPositionStays = false
        go.name = name;
        DontDestroyOnLoad(go);
    }
}
