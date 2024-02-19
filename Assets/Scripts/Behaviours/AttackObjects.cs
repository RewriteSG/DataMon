using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjects : MonoBehaviour
{
    public float DmgBasedOfStat = 0.1f;
    public GameObject AttacksByEntity;
    public void OwnAttack(GameObject byDataMon)
    {
        AttacksByEntity = byDataMon;
    }
    public void AttackFinished()
    {
        gameObject.SetActive(false);
        if (AttacksByEntity == null)
            Destroy(gameObject);
    }
}
