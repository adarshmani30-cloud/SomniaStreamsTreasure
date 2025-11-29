using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
namespace DD.Web3
{
    public class ClaimButtonHandler : MonoBehaviour
    {
        public Button[] claimButtons;

        [SerializeField] private TextMeshPro scoreText; // Assign your score Text here

        [SerializeField] private int claimAmount = 0;

        private void Start()
        {
            foreach (var claimButton in claimButtons)
            {
                claimButton.onClick.AddListener(OnClaimButtonClicked);
            }
        }

        private void OnClaimButtonClicked()
        {
            if (scoreText != null)
            {
                string numericText = Regex.Replace(scoreText.text, @"[^\d]", ""); // Remove non-digit characters

                if (int.TryParse(numericText, out claimAmount))
                {
                    Debug.Log("Claim Amount (extracted) = " + claimAmount);
                }
                else
                {
                    Debug.LogWarning("Score Text is not a valid number after stripping symbols!");
                    return;
                }
            }
            else
            {
                Debug.LogWarning("Score Text is null!");
                return;
            }

            if (BlockchainManager.Instance != null)
            {
                HandleClaimFlow();
            }
            else
            {
                Debug.Log("Blockchain or Wallet is not being used !");
            }
        }
        private void HandleClaimFlow()
        {
            BlockchainManager.Instance.connectionManager.ClaimDropERC20(claimAmount, (result) =>
            {
                if (result)
                {
                    OnTransactionSuccessful();
                }
                else
                {
                    OnTransactionFailed();
                }
            });
        }

        private void OnTransactionSuccessful()
        {
            Debug.Log("Transaction successful.");
            BlockchainManager.Instance.connectionManager.ShowLoadingScreen(false);
        }

        private void OnTransactionFailed()
        {
            Debug.Log("Transaction failed.");
            BlockchainManager.Instance.connectionManager.ShowLoadingScreen(false);
        }

        private void OnDestroy()
        {
            foreach (var claimButton in claimButtons)
            {
                claimButton.onClick.RemoveAllListeners();
            }
        }
    }


}

