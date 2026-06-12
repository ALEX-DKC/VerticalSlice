using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Player Sounds")]
    public AudioClip playerHitSound;
    public AudioClip playerDeathSound;
    public AudioClip playerPistolShootSound;
    public AudioClip playerRifleShootSound;

    [Header("Guard Sounds")]
    public AudioClip guardHitSound;
    public AudioClip guardDeathSound;
    public AudioClip enemyRifleShootSound;

    [Header("Boss Sounds")]
    public AudioClip bossHitSound;
    public AudioClip bossDeathSound;
    public AudioClip bossPunchSound;

    [Header("Assassination Sounds")]
    public AudioClip assassinateSound;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundManager: AudioClip is missing.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("SoundManager: AudioSource is missing.");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlaySoundDelayed(AudioClip clip, float delay)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundManager: AudioClip is missing.");
            return;
        }

        if (sfxSource == null)
        {
            Debug.LogWarning("SoundManager: AudioSource is missing.");
            return;
        }

        sfxSource.clip = clip;
        sfxSource.PlayDelayed(delay);
    }

    public void PlayPlayerHit()
    {
        PlaySound(playerHitSound);
    }

    public void PlayPlayerDeath()
    {
        PlaySound(playerDeathSound);
    }

    public void PlayPlayerPistolShoot()
    {
        PlaySound(playerPistolShootSound);
    }

    public void PlayPlayerRifleShoot()
    {
        PlaySound(playerRifleShootSound);
    }

    public void PlayGuardHit()
    {
        PlaySound(guardHitSound);
    }

    public void PlayGuardDeath()
    {
        PlaySound(guardDeathSound);
    }

    public void PlayEnemyRifleShoot()
    {
        PlaySound(enemyRifleShootSound);
    }

    public void PlayBossHit()
    {
        PlaySound(bossHitSound);
    }

    public void PlayBossDeath()
    {
        PlaySound(bossDeathSound);
    }

    public void PlayBossPunch()
    {
        PlaySound(bossPunchSound);
    }

    public void PlayAssassinate()
    {
        PlaySound(assassinateSound);
    }

    public void PlayAssassinateDelayed(float delay)
    {
        PlaySoundDelayed(assassinateSound, delay);
    }
}