using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this as T)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this as T)
            _instance = null;
    }

    // Helpers
    public static bool HasInstance() => _instance != null;

    /// <summary>
    /// Tenta retornar a instância registrada; se não existir, tenta encontrar um na cena.
    /// NÃO cria objetos automaticamente. Retorna null se não encontrar nada.
    /// </summary>
    public static T GetInstance()
    {
        if (_instance != null) return _instance;

        _instance = FindObjectOfType<T>(true); // inclui objetos inativos
        return _instance;
    }
}
