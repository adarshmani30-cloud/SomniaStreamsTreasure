using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class SomniaManager_Shop : MonoBehaviour
{
    public static SomniaManager_Shop Instance;

    [Header("Wallet + UI")]
    public string walletAddress;
    public int cash;
    public TextMeshProUGUI cashText;

    [Header("Events")]
    public System.Action<int> OnCashLoaded;

    private string baseUrl = "https://on-chain-data-two.vercel.app";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // ✅ Load wallet from PlayerPrefs first
        if (PlayerPrefs.HasKey("walletAddress"))
        {
            walletAddress = PlayerPrefs.GetString("walletAddress");
            Debug.Log("Loaded Wallet Address from PlayerPrefs: " + walletAddress);
        }
        else
        {
            Debug.LogWarning("⚠ No walletAddress found in PlayerPrefs!");
        }

        // Now fetch cash for this saved wallet
        StartCoroutine(FetchCash());
    }

    IEnumerator FetchCash()
    {
        string url = baseUrl + "/api/data?player=" + walletAddress;

        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Fetch error: " + req.error);
            yield break;
        }

        LeaderboardAPIResponse response =
            JsonUtility.FromJson<LeaderboardAPIResponse>(req.downloadHandler.text);

        cash = 0;

        if (response != null && response.leaderboard != null)
        {
            foreach (var entry in response.leaderboard)
            {
                if (entry.player.ToLower().Trim() == walletAddress.ToLower().Trim())
                {
                    int.TryParse(entry.score, out cash);
                    break;
                }
            }
        }

        Debug.Log("💰 Cash Loaded = " + cash);

        UpdateUI();

        OnCashLoaded?.Invoke(cash);
    }

    public void UpdateUI()
    {
        if (cashText != null)
            cashText.text = "$" + cash;
    }

    public void PublishFinalCash()
    {
        StartCoroutine(PublishCash());
    }

    IEnumerator PublishCash()
    {
        string url = baseUrl + "/api/publish";
        string json = "{\"player\":\"" + walletAddress + "\",\"score\":" + cash + "}";

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            Debug.Log("✔ Final Cash Published → " + cash);
        else
            Debug.LogError("❌ Publish error: " + req.error);
    }
}
