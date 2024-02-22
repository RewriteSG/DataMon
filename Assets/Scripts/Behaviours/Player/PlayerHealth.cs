using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100;
    public float currentHealth;
    public HealthBarScript healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);
        }
        if (currentHealth <= 0)
            GameManager.LoseGame = true;
    }
    public void TakeDamage(float damage)
    {

        if (damage > 0)
        {
            GameManager.instance.timeShieldToRegen = GameManager.TimeOutOfCombat;
            AudioManager.instance.PlayAudioClip(AudioManager.instance.PlayerHurt);
        }

        if ((GameManager.instance.isShielded && GameManager.instance.CurrentShieldHealth < 0) || !GameManager.instance.isShielded)
            currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        healthBar.SetHealth(currentHealth);
    }
}
