using IndividualDataMon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

[CreateAssetMenu(fileName = "Add DataMon", menuName = "DataMon/Add Ability", order = 1)]
public class AbilitiesScriptableObjects : ScriptableObject
{
    [Header("Evolution Passives Descriptions")]
    public AbilityDescription[] abilityDescriptions;
    public PassiveType passiveType;
    //public AttackScriptableObject AttackAbility;
    public float[] ModifiersByEvolution;


    public delegate void AffectEnemyStatDelegate(ref DataMon dataMon, float floatArg = 0);
    public delegate void AffectPlayerStatDelegate<T>(ref T Player,DataMon dataMon, bool toggle);
    public delegate void AffectStatDelegate(ref DataMon dataMon);

    public AffectStatDelegate affectStat;
    public AffectPlayerStatDelegate<GameManager> affectPlayerStat;
    public AffectEnemyStatDelegate affectEnemyStat;

    public enum PassiveType
    {
        Passive_Ant, Passive_Bee, Passive_Turt, Passive_Centaur, Passive_Plant, Passive_Skorp, Passive_Skorpike,ActiveBuff_Wolf, NotPassive
    }
    public void CallDelegates(ref DataMon dataMon, ref GameManager Player, bool toggle)
    {

        affectStat(ref dataMon);
        affectPlayerStat(ref Player, dataMon, toggle);
        //affectEnemyStat(ref dataMon, floatArg);
    }
    public void OwnPassives()
    {
        affectStat = EmptyDelegate;
        affectPlayerStat = EmptyDelegate;
        affectEnemyStat = EmptyDelegate;
        switch (passiveType)
        {
            case PassiveType.Passive_Ant:
                affectStat = Passive_Ant;
                break;
            case PassiveType.Passive_Bee:
                affectPlayerStat = Passive_Bee;
                break;
            case PassiveType.Passive_Turt:
                affectEnemyStat = Passive_Turt;
                break;
            case PassiveType.Passive_Centaur:
                affectPlayerStat = Passive_Centaur;
                break;
            case PassiveType.Passive_Plant:
                affectPlayerStat = Passive_Plant1;
                affectPlayerStat += Passive_Plant2;
                break;
            case PassiveType.Passive_Skorp:
                affectEnemyStat = Passive_Skorp;
                break;
            case PassiveType.Passive_Skorpike:
                affectEnemyStat = Passive_Skorpike;
                break;
            case PassiveType.ActiveBuff_Wolf:
                affectStat = ActiveBuff_Wolf;
                break;
        }
    }
    void EmptyDelegate(ref DataMon dataMon, float floatArg = 0)
    {
        return;
    }
    void EmptyDelegate(ref GameManager Player, DataMon dataMon, bool toggle)
    {
        return;

    }
    void EmptyDelegate(ref DataMon dataMon)
    {
        return;

    }

    //int count;
    float modifier;
    int currentEvo;
    private int GetDataMonEvolutionIndex(DataMon dataMon)
    {
        currentEvo = dataMon.tier;
        return currentEvo;
    }
    void SetAntAttribute(ref DataMon dataMon, int count)
    {
        int evolution = dataMon.tier;
        modifier = ModifiersByEvolution[evolution] * count;
        dataMon.SetAttributesByModifier(modifier);
    }
    void Passive_Ant(ref DataMon dataMon)
    {
        DataMonButton[] temp =  DataDex.instance.GetDataMonFromTeam(dataMon.dataMonData);
        //Debug.Log("Before ? HP " + dataMon.CurrentAttributes.CurrentHealth);
        for (int i = 0; i < temp.Length; i++)
        {
            SetAntAttribute(ref temp[i].dataMon, temp.Length);
        }
        //Debug.Log("working? " + modifier);
    }
    void Passive_Bee(ref GameManager GM, DataMon dataMon, bool toggle)
    {
        GetDataMonEvolutionIndex(dataMon);
        if (toggle)
            GM.ChanceForDoubleDrop += ModifiersByEvolution[currentEvo];
        else
            GM.ChanceForDoubleDrop -= ModifiersByEvolution[currentEvo];

    }
    void Passive_Turt(ref DataMon dataMon, float reflectDamage )
    {
        GetDataMonEvolutionIndex(dataMon);
        dataMon.CurrentAttributes.CurrentHealth -= reflectDamage * ModifiersByEvolution[currentEvo];
    }
    void Passive_Centaur(ref GameManager Player, DataMon dataMon, bool toggle)
    {
        GetDataMonEvolutionIndex(dataMon);
        DataMonButton[] temp = DataDex.instance.GetDataMonFromTeam(dataMon.dataMonData);
        bool isHighestEvo = temp.Length == 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (currentEvo >= temp[i].dataMon.dataMonData.DataMons.GetDataMonIndexInDataArray(temp[i].dataMon.dataMon))
                isHighestEvo = true;
        }
        if (isHighestEvo)
            return;

        if (toggle)
            Player.MaxShieldHealth = ModifiersByEvolution[currentEvo];

        if (temp.Length == 1)
            Player.isShielded = toggle;
    }

    
    void Passive_Plant1(ref GameManager Player, DataMon dataMon, bool toggle)
    {
        GetDataMonEvolutionIndex(dataMon);
        if (toggle)
        {
            Player.PlayerRegenerationRatePerSecond += ModifiersByEvolution[currentEvo];
        }
        else
        {
            Player.PlayerRegenerationRatePerSecond -= ModifiersByEvolution[currentEvo];
        }
    }
    void Passive_Plant2(ref GameManager Player, DataMon dataMon, bool toggle)
    {

        GetDataMonEvolutionIndex(dataMon);
        if (toggle)
        {
            Player.AllDamageModifier += ModifiersByEvolution[currentEvo];
        }
        else
        {
            Player.AllDamageModifier -= ModifiersByEvolution[currentEvo];

        }
    }
    void Passive_Skorp(ref DataMon dataMon, float modifer)
    {
        dataMon.CurrentAttributes.CurrentMoveSpeed = dataMon.baseAttributes.BaseMoveSpeed * modifer;
    }

    void Passive_Skorpike(ref DataMon dataMon, float modifer)
    {
        dataMon.CurrentAttributes.CurrentMoveSpeed = dataMon.baseAttributes.BaseMoveSpeed * modifer;
        if(Random.Range(0,100)<= 25)
        {
            dataMon.CurrentAttributes.CurrentMoveSpeed = 0;
        }
    }
    void ActiveBuff_Wolf(ref DataMon dataMon)
    {
        GetDataMonEvolutionIndex(dataMon);
        dataMon.DataMonStartBuff(10, ModifiersByEvolution[currentEvo]);
    }

    void Passive_Dragon(ref DataMon dataMon)
    {
        GetDataMonEvolutionIndex(dataMon);
        //count = DataDex.instance.GetDataMonCountFromTeam(dataMon.dataMonData);
        modifier = ModifiersByEvolution[currentEvo];
        dataMon.SetAttributesByModifier(modifier);
    }
}
[System.Serializable]
public class AbilityDescription
{
    public string Name;
    public string Description;
}
    

