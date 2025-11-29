using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Buttons and Panels")]
    public Button startButton;
    public Button tryAgainButton;
    public Button ClaimButton;
    public GameObject rpsPanel;
    public GameObject winText;
    public GameObject loseText;
    public GameObject drawText;
    public GameObject Turntext;
    public GameObject rockButton;
    public GameObject paperButton;
    public GameObject scissorButton;

    public GameObject rpsRoundWin;
    public GameObject rpsRoundLose;
    public SomniaPublisher_RPS somniaPublisher;
    public int roundScore = 0; // each win = +score

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        // Wait until GameManager is initialized
        yield return new WaitUntil(() => GameManager.Instance != null);

        // Subscribe to events
        GameManager.Instance.OnGameStart += ShowRPSPanel;
        GameManager.Instance.OnRoundEnd += ShowRPSPanel;

        // Hook up start button
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogWarning("Start Button is not assigned in UIManager.");
        }

        startButton.transform.localScale = Vector3.zero;
        startButton.transform.DOScale(Vector3.one, 1f);
        tryAgainButton.transform.localScale = Vector3.zero;
        tryAgainButton.gameObject.SetActive(false);
        ClaimButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= ShowRPSPanel;
            GameManager.Instance.OnRoundEnd -= ShowRPSPanel;
        }

        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked");
        GameManager.Instance.StartGame();
    }

    public void ShowRPSPanel()
    {
        if (rpsPanel != null)
        {
            rockButton.transform.localScale = Vector3.zero;
            paperButton.transform.localScale = Vector3.zero;
            scissorButton.transform.localScale = Vector3.zero;
            rpsPanel.SetActive(true);

            rockButton.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.5f);
            paperButton.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.8f);
            scissorButton.transform.DOScale(Vector3.one, 0.5f).SetDelay(1.1f);

            Debug.Log("RPSPanel shown");
        }
    }

    public void HideRPSPanel()
    {
        if (rpsPanel != null)
        {
            rpsPanel.SetActive(false);
        }
    }

    public void ShowResult(bool win)
    {
        GameObject resultText = win ? winText : loseText;

        if (resultText != null)
        {
            resultText.SetActive(true);
            resultText.transform.localScale = Vector3.zero;

            resultText.transform.DOScale(Vector3.one, 1f);
            resultText.transform
     .DOScale(Vector3.zero, 0.5f)
     .SetDelay(5f)
     .OnComplete(() =>
     {
         resultText.SetActive(false);

         string rawScore = ScoreManager.Instance.playerScoreText.text;
         rawScore = rawScore.Replace("$", "").Replace(" ", "");
         int score = int.Parse(rawScore);

         somniaPublisher.PublishScore(score);

         tryAgainButton.gameObject.SetActive(true);
         ClaimButton.gameObject.SetActive(true);
         tryAgainButton.transform.DOScale(Vector3.one, 0.5f);
     });

        }
    }

    public void ShowDrawMessage()
    {
        if (drawText != null)
        {
            drawText.SetActive(true);
            drawText.transform.localScale = Vector3.zero;

            drawText.transform.DOScale(Vector3.one, 1f);
            drawText.transform
                .DOScale(Vector3.zero, 0.5f)
                .SetDelay(2f)
                .OnComplete(() =>
                {
                    drawText.SetActive(false);
                    ShowRPSPanel(); // Try again after delay
                });
        }
    }

    public void ShowRPSRoundResult(string result)
    {
        if(result == "win")
        {
            rpsRoundWin.transform.DOScale(Vector3.one, 1f).SetDelay(3f).OnComplete(()=>
            {
                rpsRoundWin.transform.DOScale(Vector3.zero, 0.3f);
            });
        }
        else if(result == "lose")
        {
            rpsRoundLose.transform.DOScale(Vector3.one, 1f).SetDelay(3f).OnComplete(()=>
            {
                rpsRoundLose.transform.DOScale(Vector3.zero, 0.3f);
            });
        }
    }
}
