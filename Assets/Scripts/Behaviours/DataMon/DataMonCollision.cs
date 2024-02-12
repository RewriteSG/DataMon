using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMonCollision : MonoBehaviour
{
    [HideInInspector]public DataMon.IndividualDataMon.DataMon _datamon;
    // Start is called before the first frame update
    void Start()
    {
        _datamon = transform.parent.GetComponent<DataMon.IndividualDataMon.DataMon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            damage = float.Parse(collision.gameObject.name);
            _datamon.dataMonCurrentAttributes.CurrentHealth -= damage;
            _datamon.dataMonCurrentAttributes.CurrentHealth = 
                Mathf.Clamp(_datamon.dataMonCurrentAttributes.CurrentHealth,0, _datamon.dataMonCurrentAttributes.CurrentHealth+1);
        }
    }
}
