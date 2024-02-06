using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Add DataMon", menuName = "DataMon/Add", order = 1)]
public class DataMonsData : ScriptableObject
{
    
    [Header("Put DataMons from tier 1 to tier 2, and so on..")]
    public DataMonIndividualData[] _DataMon;
    public DataMonRole MonRole;
}

public enum DataMonRole
{
    Production, Attack, Defense, Healer
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
    public GameObject[] DataMonAttackProjectiles;
    public DataMonAttributes BaseAttributes;
    public DataMonBehaviourState MonBehaviourState;
    //public DataMonsData derivedData;
    public DataMonIndividualData()
    {

    }
    /// <summary>
    /// Use this to only copy the data
    /// </summary>
    public DataMonIndividualData(DataMonIndividualData toCopy)
    {
        DataMonName = toCopy.DataMonName;
        DataMonPrefab = toCopy.DataMonPrefab;
        DataMonAttackProjectiles = toCopy.DataMonAttackProjectiles;
        BaseAttributes = DataMonInstancedAttributes.ConvertToDataMonAttributes(new DataMonInstancedAttributes(toCopy.BaseAttributes));
        MonBehaviourState = toCopy.MonBehaviourState;
    }
}
[System.Serializable]
public class DataMonAttributes
{
    public float BaseHealth;
    public float BaseAttack;
    public float BaseProductionSpeed;
    public float BaseMoveSpeed;
    public float BaseAttackRange = 1;
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
}
    public static class DataMonsDataExtensions
{
    public static DataMonIndividualData GetDataMonInDataArray<T>(this T[] array, GameObject dataMon) where T : DataMonIndividualData    
    {
        DataMonIndividualData toReturn = null;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].DataMonPrefab == dataMon)
            {
                toReturn = array[i];
                break;
            }
        }
        return toReturn;
    }
}