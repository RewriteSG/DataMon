using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMonCollision : MonoBehaviour
{
    public IndividualDataMon.DataMon DataMon;
    // Start is called before the first frame update
    void Awake()
    {
        DataMon = transform.GetComponentInParent<IndividualDataMon.DataMon>();
        //if (GetComponent<Animator>() != null)
        //    GetComponent<Animator>().;

        if (GetComponent<BoxCollider2D>() !=null)
            GetComponent<BoxCollider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float damage;
    BulletInstance bullet;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            bullet = collision.GetComponent<BulletInstance>();
            damage = bullet.Damage;
            DataMon.CurrentAttributes.CurrentHealth -= damage;
            DataMon.CurrentAttributes.CurrentHealth = 
                Mathf.Clamp(DataMon.CurrentAttributes.CurrentHealth,0, DataMon.CurrentAttributes.CurrentHealth+1);
            if(!bullet.IsDrivenByAnimation)
            collision.gameObject.SetActive(false);
            if (bullet.IsFromPlayer)
                DataMon.dataMonAI.aggroSystem.SetDamageByEntity(GameManager.instance.Player, damage);
            AudioManager.instance.PlayAudioClip(AudioManager.instance.DataMonHit);
        }
        if (collision.gameObject.CompareTag("AllyAttack"))
        {

            damage = collision.GetComponent<AttackObjects>().Damage;

            DataMon.CurrentAttributes.CurrentHealth -= damage;

            DataMon.CurrentAttributes.CurrentHealth =
                Mathf.Clamp(DataMon.CurrentAttributes.CurrentHealth, 0, DataMon.CurrentAttributes.CurrentHealth + 1);

            AudioManager.instance.PlayAudioClip(AudioManager.instance.DataMonHit);
            //= bullet.Damage;
        }
    }
}
