using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public string rewardName;
    public bool isCollected = false;
    public bool isSelectable = false;

    public RewardType rewardType;
    public int value;

    public enum RewardType
    {
        Multiplier,
        Additive,
        Subtractor,
        Divider
    }

    public virtual void ResetState()
    {
        isCollected = false;
        gameObject.SetActive(true);
    }

    public virtual void Collect(string owner)
    {
        if (isCollected)
        {
            return;
        }

        isCollected = true;
        Debug.Log($"{owner} collected {rewardName}");

        gameObject.SetActive(false);
    }

    public void SetInteractable(bool canInteract)
    {
        isSelectable = canInteract;

        var col = GetComponent<Collider>();
        if (col != null)
            col.enabled = canInteract;

        SetGlow(canInteract);
    }

    public void SetGlow(bool isActive)
    {
        Transform glow = transform.Find("Glow");
        if (glow != null)
            glow.gameObject.SetActive(isActive);
    }

    private void OnMouseDown()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (!isCollected && RewardManager.Instance.CanPlayerCollect(this))
        {
            RewardManager.Instance.PlayerSelects(this);
        }
#endif
    }
}
