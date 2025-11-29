using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform leaderboardContainer;
    public GameObject leaderboardEntryPrefab;

    [Header("Settings")]
    public int leaderboardLimit = 20;

    private string leaderboardURL = "https://on-chain-data-two.vercel.app/api/data";

    private Coroutine loadCoroutine;

    private void Start()
    {
        RefreshLeaderboard();
    }

    // 🔥 PUBLIC FUNCTION — call this from button / game end / anywhere
    public void RefreshLeaderboard()
    {
        Debug.Log("🔄 Refreshing leaderboard...");

        // stop previous coroutine safely
        if (loadCoroutine != null)
            StopCoroutine(loadCoroutine);

        // clear UI before reload
        foreach (Transform child in leaderboardContainer)
            Destroy(child.gameObject);

        loadCoroutine = StartCoroutine(LoadLeaderboard());
    }

    private IEnumerator LoadLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(leaderboardURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Failed to load leaderboard: " + request.error);
            yield break;
        }

        Debug.Log("📥 RAW API RESPONSE: " + request.downloadHandler.text);

        LeaderboardAPIResponse response =
            JsonUtility.FromJson<LeaderboardAPIResponse>(request.downloadHandler.text);

        if (response == null || response.leaderboard == null || response.leaderboard.Length == 0)
        {
            Debug.Log("📭 No leaderboard data.");
            yield break;
        }

        PopulateLeaderboard(response.leaderboard);
    }

    private void PopulateLeaderboard(LeaderboardEntry[] entries)
    {
        foreach (Transform child in leaderboardContainer)
            Destroy(child.gameObject);

        foreach (var entry in entries)
        {
            GameObject row = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            texts[0].text = entry.rank.ToString();
            texts[1].text = entry.player;
            texts[2].text = entry.score;
        }
    }
}

[System.Serializable]
public class LeaderboardAPIResponse
{
    public int totalPlayers;
    public LeaderboardEntry[] leaderboard;
}

[System.Serializable]
public class LeaderboardEntry
{
    public int rank;
    public string player;
    public string score;
}
