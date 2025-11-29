using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static Collectible;
using DG.Tweening;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Visual Money")]
    public Transform playerMoneyObject;
    public Transform enemyMoneyObject;


    [Header("Slap")]
    public GameObject playerWinVC;
    public GameObject enemyWinVC;
    public Animator playerAnimator;
    public Animator enemyAnimator;
    private float scoreScaleFactor = 0.01f;

    public TextMeshPro playerScoreText;
    public TextMeshProUGUI playerScoreText2;
    public TextMeshPro enemyScoreText;

    public int playerScore { get; private set; }
    public int enemyScore { get; private set; }

    [SerializeField] private List<int> initialScoreOptions = new List<int> { 20, 50, 100, 150, 200 };

    


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void InitializeScores()
    {
        int startScore = initialScoreOptions[Random.Range(0, initialScoreOptions.Count)];
        playerScore = startScore;
        enemyScore = startScore;

        UpdateUI();

        // Animation
        float scale = playerScore * scoreScaleFactor;

        playerMoneyObject.localScale = Vector3.zero;
        enemyMoneyObject.localScale = Vector3.zero;

        playerMoneyObject.DOScale(new Vector3(1, scale, 1), 1f).SetEase(Ease.OutBack);
        enemyMoneyObject.DOScale(new Vector3(1, scale, 1f), 1f).SetEase(Ease.OutBack);

        Debug.Log($"Initial Score: Player = {playerScore}, Enemy = {enemyScore}");
    }


    private void SavePlayerScore()
    {
        PlayerPrefs.SetInt(PrefKeys.PlayerScore, playerScore);
        PlayerPrefs.SetInt(PrefKeys.ScoreAdded, 0); // reset flag
        PlayerPrefs.Save();
        Debug.Log($"Player score saved: {playerScore}");
    }

    private void OnApplicationQuit()
    {
        SavePlayerScore();
    }

    public void ApplyReward(string owner, RewardType type, int value)
    {
        int newScore;

        if (owner == "Player")
            newScore = playerScore;
        else
            newScore = enemyScore;

        switch (type)
        {
            case RewardType.Multiplier:
                newScore *= value;
                break;

            case RewardType.Additive:
                newScore += value;
                break;

            case RewardType.Subtractor:
                newScore -= value;
                break;

            case RewardType.Divider:
                newScore = value == 0 ? newScore : Mathf.Max(1, newScore / value);
                break;
        }

        // Assign back to the correct player
        if (owner == "Player")
        {
            playerScore = newScore;
            SavePlayerScore(); // Player score save karega
        }
        else
            enemyScore = newScore;

        Debug.Log($"{owner} score updated to {newScore} with {type}({value})");

        // Animation
        Transform target = owner == "Player" ? playerMoneyObject : enemyMoneyObject;

        float newScale = Mathf.Clamp(newScore * scoreScaleFactor, 0, 20f);

        if (newScale > 0)
        {
            target.DOScale(new Vector3(1, newScale, 1), 0.75f).SetEase(Ease.OutElastic);
        }
        else
        {
            target.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }

        UpdateUI(); // Optional: refresh any text display
    }


    public void ResetScore()
    {
        playerScore = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (playerScoreText) playerScoreText.text = $"{playerScore} $";
        if (playerScoreText2) playerScoreText2.text = $"{playerScore} $";
        if (enemyScoreText) enemyScoreText.text = $"{enemyScore} $";
    }

    public void EvaluateGameResult()
    {
        int playerFinal = playerScore;
        int enemyFinal = enemyScore;

        if (playerFinal > enemyFinal)
        {
            Debug.Log($"Game Over! Player wins with score {playerFinal} vs {enemyFinal}");
            StartCoroutine(PlayFinalVictory("Player"));
        }
        else if (enemyFinal > playerFinal)
        {
            Debug.Log($"Game Over! Enemy wins with score {enemyFinal} vs {playerFinal}");
            StartCoroutine(PlayFinalVictory("Enemy"));
        }
        else
        {
            Debug.Log($"Game Over! It's a draw. Both scored {playerFinal}");
            GameManager.Instance.EndGame();
        }
    }


    private IEnumerator PlayFinalVictory(string winner)
    {
        UIManager.Instance.Turntext.SetActive(false);

        if (winner == "Player")
        {
            playerWinVC.SetActive(true);
        }

        else if (winner == "Enemy")
        {
            enemyWinVC.SetActive(true);
        }

        yield return new WaitForSeconds(1);

        if (winner == "Player")
        {
            playerWinVC.SetActive(true);
            // SoundManager.Instance.slapSound.Play();      // playing in animation event
            playerAnimator.SetTrigger("Slap");
        }

        else if (winner == "Enemy")
        {
            enemyWinVC.SetActive(true);
            // SoundManager.Instance.slapSound.Play();      // playing in animation event
            enemyAnimator.SetTrigger("Slap");
        }
        
        yield return new WaitForSeconds(3f); // Adjust to slap animation length

        playerWinVC.SetActive(false);
        enemyWinVC.SetActive(false);
        yield return new WaitForSeconds(1);

        bool playerWon = (winner == "Player");
        UIManager.Instance.ShowResult(playerWon);

        GameManager.Instance.EndGame();

    }

}
