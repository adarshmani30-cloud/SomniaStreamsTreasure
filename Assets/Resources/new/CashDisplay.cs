using UnityEngine;
using TMPro;

public class CashDisplay : MonoBehaviour
{
    public TextMeshProUGUI cashText;

    void Start()
    {
        int cash = 0;

        // SaveGame() ne json ke andar cash store kiya tha
        if (PlayerPrefs.HasKey(PrefKeys.InvestmentSave))
        {
            string json = PlayerPrefs.GetString(PrefKeys.InvestmentSave);
            InvestmentManagerSaveData data = JsonUtility.FromJson<InvestmentManagerSaveData>(json);
            cash = data.cash;
        }

        cashText.text = $"${cash}";
    }

    [System.Serializable]
    private class InvestmentManagerSaveData
    {
        public int cash;
        public System.Collections.Generic.List<string> boughtAssets;
        public System.Collections.Generic.List<string> automatedAssets;
        public long lastSaveTime;
    }
}
