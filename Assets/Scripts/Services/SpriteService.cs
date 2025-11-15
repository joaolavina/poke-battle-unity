using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SpriteService : Singleton<SpriteService>
{
    private Dictionary<string, Sprite> _cache = new Dictionary<string, Sprite>();

    public async Task<Sprite> DownloadSpriteAsync(string url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        // 1) Cache
        if (_cache.TryGetValue(url, out Sprite cached))
            return cached;

        using var req = UnityWebRequestTexture.GetTexture(url);
        var op = req.SendWebRequest();

        var tcs = new TaskCompletionSource<bool>();
        op.completed += _ => tcs.TrySetResult(true);

        await tcs.Task;

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Download failed: {req.error} ({url})");
            return null;
        }

        Texture2D tex = DownloadHandlerTexture.GetContent(req);
        if (tex == null) return null;

        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), 100f);

        // 2) Guarda no cache
        _cache[url] = sprite;

        return sprite;
    }

}
