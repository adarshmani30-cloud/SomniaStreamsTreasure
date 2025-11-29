using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SomniaPublisher_RPS : MonoBehaviour
{
    public string walletAddress;
    private string baseUrl = "https://on-chain-data-two.vercel.app";

    private void Start()
    {
        // Load wallet from PlayerPrefs
        if (PlayerPrefs.HasKey("walletAddress"))
        {
            walletAddress = PlayerPrefs.GetString("walletAddress");
            Debug.Log("Loaded Wallet Address from PlayerPrefs: " + walletAddress);
        }
        else
        {
            Debug.LogWarning("⚠ No walletAddress found in PlayerPrefs!");
        }
    }

    public void PublishScore(int score)
    {
        StartCoroutine(Publish(score));
    }

    IEnumerator Publish(int score)
    {
        string url = baseUrl + "/api/publish";
        string json = "{\"player\":\"" + walletAddress + "\",\"score\":" + score + "}";

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log("✔ RPS Score Published → " + score);
        else
            Debug.LogError("❌ Publish error: " + req.error);
    }
}
