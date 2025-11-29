using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalletAddressDisplay : MonoBehaviour
{
    public TextMeshProUGUI addressText; // Unity UI Text

    void Start()
    {
        addressText.text = "fbhcbhcbdhc";
        /*
        // ✅ Step 1: Get the full URL
        string url = Application.absoluteURL;
        Debug.Log("🌐 Full URL: " + url);

        // ✅ Step 2: Extract query param ?address=...
        string walletAddress = GetQueryParam(url, "address");
        Debug.Log("📦 Extracted Wallet Address: " + walletAddress);

        // ✅ Step 3: Show in UI
        if (!string.IsNullOrEmpty(walletAddress))
        {
            addressText.text = walletAddress;
        }
        else
        {
            addressText.text = "No wallet address found!";
        }*/
    }

    // 🔍 Query param parser
    string GetQueryParam(string url, string key)
    {
        if (!url.Contains("?")) return null;

        string queryString = url.Split('?')[1];
        string[] pairs = queryString.Split('&');

        foreach (string pair in pairs)
        {
            string[] kv = pair.Split('=');
            if (kv.Length == 2 && kv[0] == key)
            {
                return WWW.UnEscapeURL(kv[1]);
            }
        }

        return null;
    }
}
