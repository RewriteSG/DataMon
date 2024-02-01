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
    public float AttackRange = 1;
    public GameObject DataMonPrefab;
    public GameObject[] DataMonAttackProjectiles;
    public DataMonAttributes BaseAttributes;
    public DataMonBehaviourState MonBehaviourState;
    public DataMonIndividualData()
    {

    }
    /// <summary>
    /// Use this to only copy the data
    /// </summary>
    public DataMonIndividualData(DataMonIndividualData toCopy)
    {
        DataMonName = toCopy.DataMonName;
        AttackRange = toCopy.AttackRange;
        DataMonPrefab = toCopy.DataMonPrefab;
        DataMonAttackProjectiles = toCopy.DataMonAttackProjectiles;
        BaseAttributes = toCopy.BaseAttributes;
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