using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using DD.Web3;

public class InvestmentManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI yieldRateText;
    public Transform assetPanel;
    public GameObject assetCardPrefab;

    [Header("Game Data")]
    public List<Asset> assetOptions;
    private List<AssetUI> assetUIs = new List<AssetUI>();
    private List<Asset> playerPortfolio = new List<Asset>();
    public int availableCash;

    [Header("Yield Tracking")]
    public float yieldRate;

    public SomniaManager_Shop somnia;

    private long lastSaveTime;

    void Start()
    {
        Debug.Log("🔥 InvestmentManager Start RUNNING");

        if (somnia.cash > 0)
        {
            OnSomniaCashLoaded(somnia.cash);
        }
        else
        {
            somnia.OnCashLoaded = OnSomniaCashLoaded;
        }
    }


    private void OnSomniaCashLoaded(int loadedCash)
    {
        Debug.Log("⚡ Callback Received Cash = " + loadedCash);
        availableCash = loadedCash;
        UpdateUI();

        // Create Asset UI after cash loads
        foreach (var asset in assetOptions)
        {
            GameObject card = Instantiate(assetCardPrefab, assetPanel);
            AssetUI ui = card.GetComponent<AssetUI>();
            ui.Setup(asset, this);
            assetUIs.Add(ui);
        }

        LoadGame();
        UpdateUI();
    }

    public void OnBackButton()
    {
        SaveGame();
        somnia.cash = availableCash;
        SomniaManager_Shop.Instance.PublishFinalCash();
        SceneManager.LoadScene("RPS");
    }

    public bool BuyAsset(Asset asset)
    {
        if (availableCash >= asset.price && !playerPortfolio.Contains(asset))
        {
            availableCash -= asset.price;
            playerPortfolio.Add(asset);
            RecalculateYieldRate();
            UpdateUI();
            SaveGame();
            return true;
        }
        return false;
    }

    public void CollectYield(Asset asset, int gain)
    {
        availableCash += gain;
        UpdateUI();
        SaveGame();
    }

    public bool SpendMoney(int amount)
    {
        if (availableCash >= amount)
        {
            availableCash -= amount;
            UpdateUI();
            SaveGame();
            return true;
        }
        return false;
    }

    void UpdateUI()
    {
        cashText.text = "$" + availableCash;
        Debug.Log("Updating UI → Text Object = " + cashText.name);

    }

    private void RecalculateYieldRate()
    {
        yieldRate = 0f;
        foreach (var asset in playerPortfolio)
            yieldRate += asset.price * (asset.yieldPercent / 100f);

        yieldRateText.text = $"±${yieldRate}/s";
    }

    // ---------------- SAVE / LOAD ----------------
    [System.Serializable]
    private class SaveData
    {
        public int cash;
        public List<string> boughtAssets = new List<string>();
        public List<string> automatedAssets = new List<string>();
        public long lastSaveTime;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.cash = availableCash;

        foreach (var ui in assetUIs)
        {
            if (ui.IsBought) data.boughtAssets.Add(ui.AssetName);
            if (ui.IsAutomated) data.automatedAssets.Add(ui.AssetName);
        }

        data.lastSaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(PrefKeys.InvestmentSave, json);
        PlayerPrefs.Save();

        lastSaveTime = data.lastSaveTime;
    }

    private void LoadGame()
    {
        if (!PlayerPrefs.HasKey(PrefKeys.InvestmentSave)) return;

        string json = PlayerPrefs.GetString(PrefKeys.InvestmentSave);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        availableCash = data.cash;
        lastSaveTime = data.lastSaveTime;

        foreach (var ui in assetUIs)
        {
            if (data.boughtAssets.Contains(ui.AssetName))
            {
                ui.RestoreBought();
                playerPortfolio.Add(ui.GetAsset());
            }

            if (data.automatedAssets.Contains(ui.AssetName))
                ui.RestoreAutomated();
        }

        RecalculateYieldRate();

        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long elapsed = now - lastSaveTime;

        if (elapsed > 0 && yieldRate > 0)
        {
            float offlineEarnings = yieldRate * elapsed;
            availableCash += Mathf.RoundToInt(offlineEarnings);
        }

        lastSaveTime = now;
        SaveGame();

        UpdateUI();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
