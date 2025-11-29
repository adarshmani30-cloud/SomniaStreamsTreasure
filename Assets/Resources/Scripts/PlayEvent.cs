using UnityEngine;

public class PlayEvent : MonoBehaviour
{
    public void PlaySlapSOund()
    {
        SoundManager.Instance.slapSound.Play();
    }
}
