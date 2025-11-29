using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSound;
    public AudioSource clickSound;
    public AudioSource moneyChangeSound;
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource drawSound;
    public AudioSource cardDealingSound;
    public AudioSource cardSelectSound;
    public AudioSource slapSound;

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private bool isMusicMuted = false;
    private bool isSfxMuted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        sfxSources.Add(clickSound);
        sfxSources.Add(moneyChangeSound);
        sfxSources.Add(winSound);
        sfxSources.Add(loseSound);
        sfxSources.Add(drawSound);
        sfxSources.Add(cardDealingSound);
        sfxSources.Add(cardSelectSound);
        sfxSources.Add(slapSound);
    }   
    
    // Music control
    public void MuteMusic(bool mute)
    {
        isMusicMuted = mute;
        if (musicSound != null)
            musicSound.mute = mute;
    }

    public void MuteSFX(bool mute)
    {
        isSfxMuted = mute;
        foreach (var src in sfxSources)
        {
            src.mute = mute;
        }
    }

    public bool IsMusicMuted => isMusicMuted;
    public bool IsSfxMuted => isSfxMuted;
}
