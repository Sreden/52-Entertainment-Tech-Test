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

            StartCoroutine(LoadLogoImage(data.thumbnail.source));
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
