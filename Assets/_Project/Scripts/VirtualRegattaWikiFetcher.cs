using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Note : this class could be refactored to retrieve any wiki page description and thumbnail
/// </summary>
public class VirtualRegattaWikiFetcher : MonoBehaviour
{
    private const string WikipediaEndpoint = "https://en.wikipedia.org/api/rest_v1/page/summary/Virtual_Regatta";

    public event Action<string> OnDescriptionLoaded;
    public event Action<Texture2D> OnTextureDownloadedOrLoaded;
    public void FetchData()
    {
        StartCoroutine(LoadWikipediaData());
    }

    private IEnumerator LoadWikipediaData()
    {
        UnityWebRequest request = UnityWebRequest.Get(WikipediaEndpoint);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON data
            var data = JsonUtility.FromJson<WikipediaSummary>(request.downloadHandler.text);
            OnDescriptionLoaded(data.extract);

            // TODO: 
            //StartCoroutine(LoadLogoImage());
        }
        else
        {
            Debug.LogError("Failed to fetch Wikipedia data: " + request.error);
            OnDescriptionLoaded("Failed to load data.");
        }
    }

    private IEnumerator LoadLogoImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            OnTextureDownloadedOrLoaded?.Invoke(texture);
        }
        else
        {
            Debug.LogError("Failed to fetch logo image: " + request.error);
        }
    }
}


/// <summary>
/// Return Example :
/// 
/// "thumbnail": {
///    "source": "https://upload.wikimedia.org/wikipedia/commons/thumb/7/75/Logo_Virtual_Regatta.svg/320px-Logo_Virtual_Regatta.svg.png",
///        "width": 320,
///        "height": 149
/// },
/// "extract": "Virtual Regatta is an online web browser sailing race simulator, though the development of a mobile app version of the game has seen a significant number of users shift to this platform in recent years."
/// 
/// 
/// </summary>
[System.Serializable]
public class WikipediaSummary
{
    public Thumbnail thumbnail;
    public string extract;
}

[System.Serializable]
public class Thumbnail
{
    public string source;
    public int width;
    public int height;
}
