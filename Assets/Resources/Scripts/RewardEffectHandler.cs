using UnityEngine;

public class RewardEffectHandler : Collectible
{
    public override void Collect(string owner)
    {
        base.Collect(owner); // Hides the object

        ScoreManager.Instance.ApplyReward(owner, rewardType, value);
    }
}
