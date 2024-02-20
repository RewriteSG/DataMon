using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMonCollision : MonoBehaviour
{
    public IndividualDataMon.DataMon DataMon;
    // Start is called before the first frame update
    void Awake()
    {
        DataMon = transform.parent.GetComponent<IndividualDataMon.DataMon>();
        if (GetComponent<Animator>() != null)
            Destroy(GetComponent<Animator>());

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
        if (collision.gameObject.CompareTag("Bullet") && DataMon.dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)
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
        }
        if(collision.gameObject.CompareTag("EnemyAttack") && DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion)
        {
            if(DataMon.dataMonAI.AI_state == AI_State.Produce)
            {
                DataMon.dataMonAI.CancelProduction();
            }
        }
    }
}
