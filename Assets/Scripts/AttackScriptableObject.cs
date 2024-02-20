using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Add DataMon", menuName = "DataMon/Add Attack Data", order = 1)]
public class AttackScriptableObject : ScriptableObject
{
    public Attack[] AllAttacks = new Attack[] { };
    public Attack RandomizeAttack()
    {
        return Attack.InstanceAttack(AllAttacks[Random.Range(0, AllAttacks.Length)]);

    }
    public Attack GetAttackByName(string attackName)
    {
        for (int i = 0; i < AllAttacks.Length; i++)
        {
            if (AllAttacks[i].AttackName == attackName)
                return AllAttacks[i];
        }
        return null;
    }

}
[System.Serializable]
public class Attack
{
    public GameObject AttackPrefab;
    public GameObject _gameObject;
    [HideInInspector] public AttackObjects attackObject;
    [HideInInspector]public float CurrentCD;
    public bool isAvailable;
    public float AttackCooldown;
    public string AttackName;
    public string AttackDescription;
    public float AttackDelayDataMonAfterFiring = 0.8f;
    public Attack() { }
    public void CreateInstance(Transform Parent, IndividualDataMon.DataMon byDataMon)
    {
        _gameObject = Object.Instantiate(AttackPrefab, Parent);
        attackObject = _gameObject.GetComponent<AttackObjects>();
        attackObject.AttacksByEntity = byDataMon; 
        _gameObject.transform.position = Vector3.up * 9999;
        _gameObject.SetActive(false);
        CurrentCD = AttackCooldown;
    }
    public void AttackCooldownUpdate()
    {
        if (_gameObject == null)
            return;
        isAvailable = CurrentCD >= AttackCooldown && !_gameObject.activeSelf;
        if (!isAvailable)
            CurrentCD += Time.deltaTime;
    }



    public static Attack InstanceAttack(Attack attack)
    {
        Attack temp = new Attack();
        temp.AttackPrefab = attack.AttackPrefab;
        temp.AttackName = attack.AttackName;
        temp.AttackCooldown = attack.AttackCooldown;
        temp.AttackDescription = attack.AttackDescription;
        temp.AttackDelayDataMonAfterFiring = attack.AttackDelayDataMonAfterFiring;
        return temp;
    }
    public static Attack[] InstanceAttack(Attack[] attack)
    {
        Attack temp;
        List<Attack> list = new List<Attack>();
        for (int i = 0; i < attack.Length; i++)
        {
            temp = InstanceAttack(attack[i]);
            list.Add(temp);
        }
        return list.ToArray();
    }
    public static bool ListHasAttack(List<Attack> attacks, Attack attack)
    {
        for (int i = 0; i < attacks.Count; i++)
        {
            if(attacks[i].AttackName == attack.AttackName)
            {
                return true;
            }
        }
        return false;
    }
}