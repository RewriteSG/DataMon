using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioManager instance;
    public AudioClip[] Explosions;
    public AudioClip DataMonHurt, turtleSpin, ShieldFull, PlayerDeath, Dash, Healing;
    private void Awake()
    {
        instance = this;
    }
}
