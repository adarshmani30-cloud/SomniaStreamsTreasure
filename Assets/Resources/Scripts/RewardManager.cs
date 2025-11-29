using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("Rewards")]
    public List<GameObject> rewardPrefabs;

    [Header("Spawn Points")]
    public Transform initialSpawnGroup;
    public GameObject finalRewardGroupPrefab;

    public List<GameObject> spawnPointGroups;

    private List<Collectible> spawnedRewards = new();
    private HashSet<Collectible> usedRewards = new();
    private List<Collectible> currentSelectionPool = new();

    private string currentTurn = "";
    private string rpsWinner = "";
    private string rpsLoser = "";
    private bool playerCanCollect = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void HandlePlayerChoice(bool playerWon, bool enemyWon)
    {
        Debug.Log("Spawning rewards...");

        currentTurn = playerWon ? "Player" : enemyWon ? "Enemy" : "Player";
        rpsWinner = currentTurn;
        rpsLoser = (currentTurn == "Player") ? "Enemy" : "Player";

        // Spawn rewards and only call TryNextTurn() after they finish moving
        SpawnRewardsWithCallback(TryNextTurn);
    }

    void TryNextTurn()
    {
        if (spawnedRewards.Count == usedRewards.Count)
        {
            ScoreManager.Instance.EvaluateGameResult();
            return;
        }

        UpdateSelectableRewards();

        if (currentTurn == "Player")
        {
            playerCanCollect = true;
            UIManager.Instance.Turntext.GetComponent<TextMeshProUGUI>().text = "Player Turn";
            Debug.Log("Player's turn to collect.");
        }
        else if (currentTurn == "Enemy")
        {
            playerCanCollect = false;
            UIManager.Instance.Turntext.GetComponent<TextMeshProUGUI>().text = "Enemy Turn";
            Debug.Log("Enemy's turn to collect.");
            Invoke(nameof(EnemySelectReward), 1.5f);
        }
    }

    void SpawnRewardsWithCallback(System.Action onComplete)
    {
        ClearPreviousRewards();

        if (spawnPointGroups.Count == 0 || rewardPrefabs.Count == 0 || initialSpawnGroup == null)
        {
            Debug.LogWarning("Missing spawn points, rewards, or initialSpawnGroup.");
            return;
        }

        GameObject selectedGroup = Instantiate(
            spawnPointGroups[Random.Range(0, spawnPointGroups.Count)]
        );
        selectedGroup.transform.position = finalRewardGroupPrefab.transform.position;

        List<Transform> finalTargets = new();
        foreach (Transform child in selectedGroup.transform)
            finalTargets.Add(child);

        List<Transform> initialPoints = new();
        foreach (Transform child in initialSpawnGroup)
            initialPoints.Add(child);

        int spawnCount = Mathf.Min(initialPoints.Count, finalTargets.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject rewardPrefab = rewardPrefabs[Random.Range(0, rewardPrefabs.Count)];
            Transform initialPoint = initialPoints[i];

            GameObject rewardInstance = Instantiate(
                rewardPrefab,
                initialPoint.position,
                initialPoint.rotation,
                initialPoint
            );

            var collectible = rewardInstance.GetComponent<Collectible>();
            if (collectible != null)
                spawnedRewards.Add(collectible);
        }

        // now pass the onComplete callback to animation
        StartCoroutine(AnimateRewardsIntoPlace(finalTargets, onComplete));
    }



    IEnumerator AnimateRewardsIntoPlace(List<Transform> targetPoints, System.Action onComplete)
    {
        for (int i = 0; i < spawnedRewards.Count && i < targetPoints.Count; i++)
        {
            var reward = spawnedRewards[i];
            var target = targetPoints[i];

            // Animate position
            reward.transform.DOMove(target.position, 0.75f).SetEase(Ease.OutBack);

            // Animate rotation to match target
            reward.transform.DORotate(target.rotation.eulerAngles, 0.75f).SetEase(Ease.OutBack);

            // Play SFX
            SoundManager.Instance.cardDealingSound.Play();

            yield return new WaitForSeconds(0.3f); // sequence delay
        }

        yield return new WaitForSeconds(0.5f); // final delay after last move
        onComplete?.Invoke();
    }

    void UpdateSelectableRewards()
    {
        // Disable interaction on all uncollected rewards
        foreach (var reward in spawnedRewards)
        {
            if (!usedRewards.Contains(reward))
            {
                reward.SetInteractable(false);
                reward.SetGlow(false);
            }
        }

        // Get uncollected + unused rewards
        var unclaimed = spawnedRewards.Where(r => !usedRewards.Contains(r)).ToList();

        // If 3 or fewer remain, all are selectable
        int countToEnable = Mathf.Min(3, unclaimed.Count);
        currentSelectionPool = new();

        // Randomly pick 3 from the unclaimed list
        for (int i = 0; i < countToEnable; i++)
        {
            int index = Random.Range(0, unclaimed.Count);
            var chosen = unclaimed[index];
            chosen.SetInteractable(true);
            chosen.SetGlow(true);
            currentSelectionPool.Add(chosen);
            unclaimed.RemoveAt(index);
        }
    }

    public bool CanPlayerCollect(Collectible reward)
    {
        return playerCanCollect && !usedRewards.Contains(reward);
    }

    public void PlayerSelects(Collectible reward)
    {
        if (!playerCanCollect) return;
        if (!reward.isSelectable) return;
        if (!currentSelectionPool.Contains(reward)) return;           // Not in active selection
        if (usedRewards.Contains(reward)) return;                     // Already collected
        if (!reward.GetComponent<Collider>().enabled) return;         // Collider is off

        reward.Collect("Player");
        SoundManager.Instance.cardSelectSound.Play();
        usedRewards.Add(reward);

        currentTurn = "Enemy";
        TryNextTurn();
    }

    void EnemySelectReward()
    {
        var available = spawnedRewards.Where(r => !usedRewards.Contains(r)).ToList();

        if (available.Count == 0) return;

        var chosen = available[Random.Range(0, available.Count)];
        Debug.Log($"Enemy is collecting: {chosen.rewardName}");
        chosen.Collect("Enemy");
        SoundManager.Instance.cardSelectSound.Play();
        usedRewards.Add(chosen);

        currentTurn = "Player";
        TryNextTurn();
    }

    void ClearPreviousRewards()
    {
        foreach (var reward in spawnedRewards)
        {
            if (reward != null)
                Destroy(reward.gameObject);
        }

        spawnedRewards.Clear();
        usedRewards.Clear();
        currentSelectionPool.Clear();
    }
}
