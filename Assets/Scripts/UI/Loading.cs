using UnityEngine;

public class Loading : SingletonMonoBehaviour<Loading>
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
