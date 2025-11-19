using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ApiService : Singleton<ApiService>
{
    private string baseUrl = "http://localhost:8081/";

    public async Task<string> Post(string endpoint, string json = null)
    {
        using var request = new UnityWebRequest(baseUrl + endpoint, "POST");

        // sempre setamos downloadHandler para poder ler a resposta
        request.downloadHandler = new DownloadHandlerBuffer();

        if (!string.IsNullOrEmpty(json))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
        }

        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
            throw new System.Exception($"Erro HTTP: {request.error}");

        return request.downloadHandler.text;
    }

    public async Task<string> Get(string endpoint)
    {
        using var request = UnityWebRequest.Get(baseUrl + endpoint);
        request.downloadHandler = new DownloadHandlerBuffer();

        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
            throw new System.Exception($"Erro HTTP GET: {request.error}");

        return request.downloadHandler.text;
    }

    public async Task<string> Delete(string endpoint)
    {
        using var request = UnityWebRequest.Delete(baseUrl + endpoint);
        request.downloadHandler = new DownloadHandlerBuffer();

        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
            throw new System.Exception($"Erro HTTP DELETE: {request.error}");

        return request.downloadHandler.text;
    }

    public async Task<T> GetJson<T>(string endpoint)
    {
        string response = await Get(endpoint);
        if (string.IsNullOrWhiteSpace(response)) return default;
        if (typeof(T) == typeof(string)) return (T)(object)response;
        return JsonUtility.FromJson<T>(response); // cuidado com arrays/primitivos
    }

    public async Task<T> PostJson<T>(string endpoint, object body = null)
    {
        // Serializa só quando tiver body
        string json = null;
        if (body != null)
        {
            // Recomendado: body deve ser uma classe [Serializable] com campos públicos
            json = JsonUtility.ToJson(body);
            Debug.Log($"[ApiService] POST {endpoint} body: {json}");
        }
        else
        {
            Debug.Log($"[ApiService] POST {endpoint} no body");
        }

        string response = await Post(endpoint, json);

        // Se a resposta for vazia (204 No Content), retorna default(T)
        if (string.IsNullOrWhiteSpace(response))
            return default;

        // Se T for string — o backend retornou uma string simples, devolve-a direta
        if (typeof(T) == typeof(string))
            return (T)(object)response;

        // Caso contrário, tenta desserializar JSON para T
        try
        {
            return JsonUtility.FromJson<T>(response);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ApiService] Falha ao desserializar resposta para {typeof(T)}. Response: {response}. Ex: {ex}");
            throw;
        }
    }

    public async Task<T> DeleteJson<T>(string endpoint)
    {
        string response = await Delete(endpoint);

        if (string.IsNullOrWhiteSpace(response))
            return default;

        if (typeof(T) == typeof(string))
            return (T)(object)response;

        try
        {
            return JsonUtility.FromJson<T>(response);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ApiService] Falha ao desserializar DELETE resposta para {typeof(T)}. Response: {response}. Ex: {ex}");
            throw;
        }
    }
}
