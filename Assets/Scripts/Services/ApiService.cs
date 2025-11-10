
using System.Threading.Tasks;
using UnityEngine.Networking;

public class ApiService : Singleton<ApiService>
{
    public async Task<string> Get(string url)
    {
        using var request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
            throw new System.Exception(request.error);

        return request.downloadHandler.text;
    }
}
