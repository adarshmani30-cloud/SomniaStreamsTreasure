using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;  // TOP pe hona chahiye
namespace DD.Web3
{
    public class AssetUI : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI assetNameText;
        public TextMeshProUGUI yieldText;
        public Image assetIcon;
        public Button automateButton;
        public Button buyButton;
        public Button collectButton;
        public TextMeshProUGUI collectText;

        private Asset assetData;
        private InvestmentManager manager;
        private int yieldValue;
        private bool isAutomated;
        private float timer;
        private bool onCooldown;
        private bool isBought;
        [SerializeField] private int claimAmount;  // Add this field
        public bool IsBought => isBought;
        public bool IsAutomated => isAutomated;
        public string AssetName => assetData.assetName;
        public Asset GetAsset() => assetData;


        // ------------------ CLAIM + BUY FLOW ------------------ //

        private void StartClaimFlowThenBuy()
        {
            Debug.Log("👉 BUY clicked → Extracting amount from Buy button");

            // Step 1 → Extract claim amount from Buy button text
            claimAmount = ExtractClaimAmountFromBuyButton();

            if (claimAmount <= 0)
            {
                Debug.LogWarning("❌ Invalid claim amount. BUY cancelled.");
                return;
            }

            Debug.Log("💰 Claim Amount = " + claimAmount);

            // Step 2 → Call blockchain claim
            if (BlockchainManager.Instance != null)
            {
                BlockchainManager.Instance.connectionManager.ClaimDropERC20(claimAmount, (result) =>
                {
                    if (result)
                    {
                        Debug.Log("✅ Claim successful → Now buying asset...");
                        BuyAfterClaim();
                    }
                    else
                    {
                        Debug.Log("❌ Claim failed → Buy cancelled.");
                    }

                    BlockchainManager.Instance.connectionManager.ShowLoadingScreen(false);
                });
            }
            else
            {
                Debug.Log("⚠ BlockchainManager not found, cannot claim!");
            }
        }


        // Extract number from BUY button text e.g. "Buy $50" → 50
        private int ExtractClaimAmountFromBuyButton()
        {
            string raw = buyButton.GetComponentInChildren<TextMeshProUGUI>().text;
            // Remove all non-digits
            string numeric = Regex.Replace(raw, @"[^\d]", "");

            if (int.TryParse(numeric, out int value))
            {
                return value;
            }

            Debug.LogWarning("⚠ Could not extract number from: " + raw);
            return 0;
        }


        // Step 3 → After claim success → Buy the asset normally
        private void BuyAfterClaim()
        {
            if (manager.BuyAsset(assetData))
            {
                isBought = true;
                buyButton.gameObject.SetActive(false);
                collectButton.gameObject.SetActive(true);

                Debug.Log("🔥 Asset bought successfully after claim flow");
            }
            else
            {
                Debug.Log("❌ BUY FAILED — maybe not enough cash?");
            }
        }
        public void RestoreBought()
        {
            isBought = true;
            buyButton.gameObject.SetActive(false);
            collectButton.gameObject.SetActive(true);
        }

        public void RestoreAutomated()
        {
            isAutomated = true;
            automateButton.interactable = false;
            automateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Automated";
            collectButton.interactable = false;

            if (!onCooldown)
                StartCoroutine(ManualCooldownRoutine());
        }



        public void Setup(Asset asset, InvestmentManager mgr)
        {
            assetData = asset;
            manager = mgr;

            assetNameText.text = asset.assetName;
            yieldText.text = asset.isVolatile ? "Yield: ±" + asset.yieldPercent + "%"
                                              : "Yield: " + asset.yieldPercent + "%";

            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy $" + asset.price;
            if (asset.icon != null)
                assetIcon.sprite = asset.icon;

            buyButton.gameObject.SetActive(true);
            collectButton.gameObject.SetActive(false);
            automateButton.interactable = false;
            automateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Auto Collect $" + asset.priceToAutomate;
            // Pre-roll yield for first collection
            yieldValue = GenerateYield();
            collectText.text = "Collect $" + yieldValue;

            buyButton.onClick.AddListener(() =>
            {
                StartClaimFlowThenBuy();
            });


            collectButton.onClick.AddListener(Collect);
            automateButton.onClick.AddListener(StartAutomation);
        }

        private void Update()
        {
            if (isBought && !isAutomated)
            {
                automateButton.interactable = manager.availableCash >= assetData.priceToAutomate;
            }
            else
            {
                automateButton.interactable = false;
            }
        }

        private int GenerateYield()
        {
            if (assetData.isVolatile)
            {
                float randomYield = Random.Range(-assetData.yieldPercent, assetData.yieldPercent);
                float factor = 1f + (randomYield / 100f);
                return Mathf.RoundToInt(assetData.price * factor) - assetData.price;
            }
            else
            {
                float factor = 1f + (assetData.yieldPercent / 100f);
                return Mathf.RoundToInt(assetData.price * factor) - assetData.price;
            }
        }

        private void Collect()
        {
            if (!isAutomated && !onCooldown)
            {
                manager.CollectYield(assetData, yieldValue);
                StartCoroutine(ManualCooldownRoutine());
            }
        }

        IEnumerator ManualCooldownRoutine()
        {
            onCooldown = true;
            collectButton.interactable = false;

            timer = assetData.cooldown;
            while (timer > 0)
            {
                collectText.text = FormatTime(timer);
                timer -= Time.deltaTime;
                yield return null;
            }

            yieldValue = GenerateYield();

            if (isAutomated)
            {
                manager.CollectYield(assetData, yieldValue);
                StartCoroutine(ManualCooldownRoutine());
            }
            else
            {
                collectText.text = "Collect $" + yieldValue;
                collectButton.interactable = true;
                onCooldown = false;
            }
        }

        private void StartAutomation()
        {
            if (!isAutomated && manager.SpendMoney(assetData.priceToAutomate))
            {
                isAutomated = true;
                automateButton.interactable = false;
                automateButton.GetComponentInChildren<TextMeshProUGUI>().text = "Automated";
                collectButton.interactable = false;

                if (!onCooldown)
                {
                    StartCoroutine(ManualCooldownRoutine());
                }
            }
        }

        private string FormatTime(float time)
        {
            int totalSeconds = Mathf.CeilToInt(time);

            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            if (hours > 0)
            {
                if (minutes > 0)
                    return $"{hours}h {minutes}m";
                else
                    return $"{hours}h";
            }
            else if (minutes > 0)
            {
                if (seconds > 0)
                    return $"{minutes}m {seconds}s";
                else
                    return $"{minutes}m";
            }
            else
            {
                return $"{seconds}s";
            }
        }
    }
}