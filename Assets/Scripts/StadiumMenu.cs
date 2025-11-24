using UnityEngine;

public class StadiumMenu : MonoBehaviour
{

    private StadiumService _stadiumService => StadiumService.GetInstance();
    private Loading _loading => Loading.GetInstance();
    private Snackbar _snackbar => Snackbar.GetInstance();

    void OnEnable()
    {
        FetchStadiums();
    }

    private async void FetchStadiums()
    {
        try
        {
            _loading.Show();
            StadiumResponse response = await _stadiumService.GetAvailableStadiums();
            GameObject content = transform.Find("Scroll View/Viewport/Content").gameObject;
            
            foreach (Transform child in content.transform)
                child.gameObject.SetActive(false);
    

            if (response != null && response.success && response.count > 0)
            {
                foreach (var stadium in response.stadiums)
                {
                    content.transform.Find(stadium.name).gameObject.SetActive(true);
                }
            }
        }
        catch (System.Exception e)
        {
            _snackbar.ShowSnackbar("Erro ao carregar estádios");
            Debug.LogError($"StadiumMenu: Erro ao carregar estádios: {e.Message}", this);
        }
        finally
        {
            _loading.Hide();
        }

    }
}
