using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public delegate void GameEvent();
    public event GameEvent OnGameStart, OnRoundEnd, OnGameOver;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartGameButton()
    {
        ScoreManager.Instance.ResetScore();
        GameManager.Instance.StartGame();
    }

    public void StartGame()
    {
        ScoreManager.Instance.InitializeScores();
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        UIManager.Instance.Turntext.GetComponent<TextMeshProUGUI>().text = "";
        Debug.Log("EndGame triggered.");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("RPS");
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("Phase-2");
    }
}
