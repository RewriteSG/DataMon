using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScriptableObject : ScriptableObject
{
    public GameObject[] AllAttacks = new GameObject[] { };
    public GameObject GetAttackByRandom()
    {
        return AllAttacks[Random.Range(0, AllAttacks.Length)];
    }
    
}
