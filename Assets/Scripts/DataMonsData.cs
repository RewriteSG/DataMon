using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Add DataMon", menuName = "DataMon/Add", order = 1)]
public class DataMonsData : ScriptableObject
{
    
    public string DataMonName;
    [Header("Put DataMons from tier 1 to tier 2, and so on..")]
    public DataMonIndividualData[] _DataMon;
    public DataMonBehaviourState MonBehaviourState;
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
    public GameObject DataMonPrefab;
    public GameObject[] DataMonAttackProjectiles;
    public DataMonAttributes BaseAttributes;
}
[System.Serializable]
public class DataMonAttributes
{
    public float BaseHealth;
    public float BaseAttack;
    public float BaseProductionSpeed;
    public float BaseMoveSpeed;
}