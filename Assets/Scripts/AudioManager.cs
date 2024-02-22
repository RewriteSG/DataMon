using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] Explosions;
    public AudioClip DataMonHit , PlayerHurt;
    public AudioClip  turtleSpin, ShieldFull, PlayerDeath, Dash, Healing, Stomp;
    public AudioSource audioSrc;
    private void Awake()
    {
        instance = this;
    }
    public void PlayAudioClip(AudioClip audioClip)
    {
        audioSrc.PlayOneShot(audioClip);
    }
}
