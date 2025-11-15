using UnityEngine;

public class BattleScript : MonoBehaviour
{
    private StadiumService _stadiumService => StadiumService.GetInstance();
    private GameObject _stadiumInstance;

    void Awake()
    {
        _stadiumInstance = Instantiate(_stadiumService.Stadium);
    }

    
}
