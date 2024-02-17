﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Add DataMon", menuName = "DataMon/Add", order = 1)]
public class DataMonsData : ScriptableObject
{

    [HideInInspector] public List<int> EvolutionCosts = new List<int>();
    [Header("Put DataMons from tier 1 to tier 2, and so on..")]
    public DataMonIndividualData[] DataMons;
    public DataMonRole MonRole;
}

public enum DataMonRole
{
    Production, Attack, Support
}
public enum DataMonBehaviourState
{
    isHostile, isCompanion, isNeutral

}
[System.Serializable]
public class DataMonIndividualData
{
    public string DataMonName;
    public GameObject DataMonPrefab;
    public GameObject[] DataMonAbility;
    public DataMonAttributes BaseAttributes;
    public DataMonBehaviourState MonBehaviourState;
    public DataMonIndividualData()
    {
         
    }
    /// <summary>
    /// Use this to only copy the data
    /// </summary>
    public static DataMonIndividualData CloneDataMonClass(DataMonIndividualData toCopy)
    {
        DataMonIndividualData temp = new DataMonIndividualData();
        temp.DataMonName = toCopy.DataMonName;
        temp.DataMonPrefab = toCopy.DataMonPrefab;
        temp.DataMonAbility = toCopy.DataMonAbility;
        temp.BaseAttributes = DataMonInstancedAttributes.ConvertToDataMonAttributes(new DataMonInstancedAttributes(toCopy.BaseAttributes));
        temp.MonBehaviourState = toCopy.MonBehaviourState;
        return temp;
    }
}
[System.Serializable]
public class DataMonAttributes
{
    public float BaseHealth;
    public float BaseAttack;
    public float BaseProductionSpeed;
    public float BaseMoveSpeed = 200;
    public float BaseAttackRange = 3;
    public float BaseCaptureChance;
    
}
[System.Serializable]
public class DataMonInstancedAttributes
{
    public float CurrentHealth;
    public float CurrentAttack;
    public float CurrentProductionSpeed;
    public float CurrentMoveSpeed;
    public float CurrentAttackRange = 1;
    public float CurrentCaptureChance;
    public DataMonInstancedAttributes() { }

    public DataMonInstancedAttributes(DataMonAttributes getAttribute)
    {
        CurrentHealth = getAttribute.BaseHealth;
        CurrentAttack = getAttribute.BaseAttack;
        CurrentProductionSpeed = getAttribute.BaseProductionSpeed;
        CurrentMoveSpeed = getAttribute.BaseMoveSpeed;
        CurrentAttackRange = getAttribute.BaseAttackRange;
        CurrentCaptureChance = getAttribute.BaseCaptureChance;
    }
    public DataMonInstancedAttributes(DataMonInstancedAttributes getAttribute)
    {
        CurrentHealth = getAttribute.CurrentHealth;
        CurrentAttack = getAttribute.CurrentAttack;
        CurrentProductionSpeed = getAttribute.CurrentProductionSpeed;
        CurrentMoveSpeed = getAttribute.CurrentMoveSpeed;
        CurrentAttackRange = getAttribute.CurrentAttackRange;
        CurrentCaptureChance = getAttribute.CurrentCaptureChance;
    }
    public static DataMonAttributes ConvertToDataMonAttributes(DataMonInstancedAttributes instancedAttributes)
    {
        DataMonAttributes temp = new DataMonAttributes();
        temp.BaseHealth = instancedAttributes.CurrentHealth;
        temp.BaseAttack = instancedAttributes.CurrentAttack;
        temp.BaseProductionSpeed = instancedAttributes.CurrentProductionSpeed;
        temp.BaseMoveSpeed = instancedAttributes.CurrentMoveSpeed;
        temp.BaseAttackRange = instancedAttributes.CurrentAttackRange;
        temp.BaseCaptureChance = instancedAttributes.CurrentCaptureChance;
        return temp;
    }
    public void ResetAttributes(DataMonAttributes toReset)
    {
        CurrentHealth = toReset.BaseHealth;
        CurrentAttack = toReset.BaseAttack;
        CurrentProductionSpeed = toReset.BaseProductionSpeed;
        CurrentMoveSpeed = toReset.BaseMoveSpeed;
        CurrentAttackRange = toReset.BaseAttackRange;
        CurrentCaptureChance = toReset.BaseCaptureChance;
    }
    public void SetAttributes(DataMonInstancedAttributes instanceAttributes)
    {
        CurrentHealth = instanceAttributes.CurrentHealth;
        CurrentAttack = instanceAttributes.CurrentAttack;
        CurrentProductionSpeed = instanceAttributes.CurrentProductionSpeed;
        CurrentMoveSpeed = instanceAttributes.CurrentMoveSpeed;
        CurrentAttackRange = instanceAttributes.CurrentAttackRange;
        CurrentCaptureChance = instanceAttributes.CurrentCaptureChance;
    }
}