using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class VirtualRegattaWikiFetcher : MonoBehaviour
{
    private const string WIKIPEDIA_ENDPOINT = "https://en.wikipedia.org/api/rest_v1/page/summary/Virtual_Regatta";
    private const string CACHED_IMAGE_NAME = "VirtualRegattaLogo.png";
    private const string CACHED_TIMESTAMP_TXT = "ImageTimestamp.txt";

    public float maxCacheDurationInSeconds = 24 * 60 * 60; // 24 hours

    public event Action<string> OnDescriptionLoaded;
    public event Action<Texture2D> OnTextureDownloadedOrLoaded;

    public void FetchData()
    {
        StartCoroutine(LoadWikipediaData());
    }

    private IEnumerator LoadWikipediaData()
    {
        UnityWebRequest request = UnityWebRequest.Get(WIKIPEDIA_ENDPOINT);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON data
            var data = JsonUtility.FromJson<WikipediaSummary>(request.downloadHandler.text);
            OnDescriptionLoaded?.Invoke(data.extract);

            StartCoroutine(LoadLogoImage(data.thumbnail.source));
        }
        else
        {
            Debug.LogError("Failed to fetch Wikipedia data: " + request.error);
            OnDescriptionLoaded?.Invoke("Failed to load data.");
        }
    }

    private IEnumerator LoadLogoImage(string url)
    {
        string cachedImagePath = Path.Combine(Application.persistentDataPath, CACHED_IMAGE_NAME);
        string cachedTimestampPath = Path.Combine(Application.persistentDataPath, CACHED_TIMESTAMP_TXT);

        bool shouldReload = true;

        // Check cache validity
        if (File.Exists(cachedImagePath) && File.Exists(cachedTimestampPath))
        {
            string cachedTimestamp = File.ReadAllText(cachedTimestampPath);

            if (DateTime.TryParse(cachedTimestamp, out DateTime savedTime))
            {
                double elapsedSeconds = (DateTime.UtcNow - savedTime).TotalSeconds;

                if (elapsedSeconds <= maxCacheDurationInSeconds)
                {
                    // Cache is valid, load image
                    byte[] fileData = File.ReadAllBytes(cachedImagePath);
                    Texture2D cachedTexture = new Texture2D(2, 2);
                    cachedTexture.LoadImage(fileData);

                    OnTextureDownloadedOrLoaded?.Invoke(cachedTexture);
                    Debug.Log("Loaded cached image.");
                    shouldReload = false;
                }
            }
        }

        if (shouldReload)
        {
            // Download the image if no valid cache is found
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                OnTextureDownloadedOrLoaded?.Invoke(texture);

                // Save the image locally
                byte[] imageData = texture.EncodeToPNG();
                File.WriteAllBytes(cachedImagePath, imageData);
                File.WriteAllText(cachedTimestampPath, DateTime.UtcNow.ToString("o")); // ISO 8601 format

                Debug.Log("Image downloaded and cached.");
            }
            else
            {
                Debug.LogError("Failed to fetch logo image: " + request.error);
            }
        }
    }
}
