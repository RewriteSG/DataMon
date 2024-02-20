using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScriptableObject : ScriptableObject
{
    public Attack[] AllAttacks = new Attack[] { };
    public GameObject GetAttackByRandom()
    {
        return AllAttacks[Random.Range(0, AllAttacks.Length)].AttackPrefab;
    }
}
[System.Serializable]
public class Attack
{
    public GameObject AttackPrefab;
    public string AttackName;
    public string AttackDescription;
    public float AttackDamageModifier;
}