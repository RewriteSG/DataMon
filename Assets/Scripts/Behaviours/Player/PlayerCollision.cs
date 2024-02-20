using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerHealth playerhealth;
    // Start is called before the first frame update
    void Start()
    {
        playerhealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    AttackObjects attack;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            attack = collision.gameObject.GetComponent<AttackObjects>();
            print(attack.DmgBasedOfStat);
            playerhealth.TakeDamage(attack.DmgBasedOfStat);
        }

        if (collision.CompareTag("Databytes"))
        {
            GameManager.instance.Databytes += 1;
            Destroy(collision.gameObject);
        }
    }
}
