﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMonCollision : MonoBehaviour
{
    public IndividualDataMon.DataMon DataMon;
    // Start is called before the first frame update
    void Awake()
    {
        DataMon = transform.parent.GetComponent<IndividualDataMon.DataMon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && DataMon.dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)
        {
            damage = collision.GetComponent<BulletInstance>().Damage;
            DataMon.dataMonCurrentAttributes.CurrentHealth -= damage;
            DataMon.dataMonCurrentAttributes.CurrentHealth = 
                Mathf.Clamp(DataMon.dataMonCurrentAttributes.CurrentHealth,0, DataMon.dataMonCurrentAttributes.CurrentHealth+1);
            collision.gameObject.SetActive(false);
            DataMon.dataMonAI.aggroSystem.SetDamageByEntity(GameManager.instance.Player,damage);
        }
    }
}
