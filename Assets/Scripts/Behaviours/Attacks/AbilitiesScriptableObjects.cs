using IndividualDataMon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

//[CreateAssetMenu(fileName = "Add DataMon", menuName = "DataMon/Add Ability", order = 1)]
public class AbilitiesScriptableObjects : ScriptableObject
{
    [Header("Evolution Passives Descriptions")]
    public AbilityDescription[] abilityDescriptions = new AbilityDescription[3];

    public bool isAbilitySemiAuto;

    public AbilityType _AbilityType;
    public float[] ModifiersByEvolution = new float[3];
    public float[] AbilityCooldown = new float[3];
    public float ActiveTime;
    //public string AttackName;
    //public string AttackDescription;
    public GameObject ItemImage;
    public GameObject ItemObject;

    public GameObject AbilityPrefab;



    //public delegate void AbilityDelegate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle);
    //public delegate void AffectPlayerStatDelegate<T>(ref T Player,DataMon dataMon, bool toggle);
    //public delegate void AffectStatDelegate(ref DataMon dataMon);

    //public AffectStatDelegate affectStat;
    //public AffectPlayerStatDelegate<GameManager> affectPlayerStat;
    //public AffectEnemyStatDelegate affectEnemyStat;
    

    public enum AbilityType
    {
        AffectEnemy, Passive, ActiveAbility, Mount
        //Passive_Ant, Passive_Bee, Passive_Turt, Passive_Centaur, Passive_Plant, Passive_Skorp, Passive_Skorpike,ActiveBuff_Wolf
    }
    public virtual void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {

        ItemInHotBar newItem = new ItemInHotBar(isAbilitySemiAuto ? ItemHolding.DataMonSemiAuto : ItemHolding.DataMonAuto, ItemImage);
        DataMonItemAbility temp = newItem.ItemImage.gameObject.AddComponent<DataMonItemAbility>();
        temp.SetValue(dataMonData, dataMon, Player, toggle, GetAbility());
        newItem.SelectedItem = temp.ItemSelected;
        newItem.UnselectItem = temp.ItemUnselected;
        newItem.useItem = temp.UseItem;
        Debug.Log("activated");

        HotBarController.ItemsInHotbar.Add(newItem);

        //affectStat(ref dataMon);
        //affectPlayerStat(ref Player, dataMon, toggle);
        ////affectEnemyStat(ref dataMon, floatArg);
    }
    public virtual void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {

    }
    
    public virtual GameObject UseAbility(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle, Transform gunPoint = default)
    {
        return null;
    }
    public virtual void RideDataMon(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {

    }
    public virtual void DismountDataMon(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {

    }
    public virtual AbilitiesScriptableObjects GetAbility()
    {
        return this;
    }
    //public void AddToHotbar(ItemInHotBar newItem , DataMonItemAbility temp)
    //{

    //}
    public virtual AbilitiesScriptableObjects Serialize()
    {
        AbilitiesScriptableObjects toReturn = CreateInstance<AbilitiesScriptableObjects>();
        toReturn.abilityDescriptions = abilityDescriptions;
        toReturn.AbilityCooldown = AbilityCooldown;
        toReturn.ActiveTime = ActiveTime;
        toReturn.ItemObject = ItemObject;

        toReturn.AbilityPrefab = AbilityPrefab;
        toReturn.ItemImage = ItemImage;
        toReturn._AbilityType = _AbilityType;
        return this;
    }

    //public void OwnPassives()
    //{
    //    affectStat = EmptyDelegate;
    //    affectPlayerStat = EmptyDelegate;
    //    affectEnemyStat = EmptyDelegate;
    //    switch (passiveType)
    //    {
    //        case PassiveType.Passive_Ant:
    //            affectStat = Passive_Ant;
    //            break;
    //        case PassiveType.Passive_Bee:
    //            affectPlayerStat = Passive_Bee;
    //            break;
    //        case PassiveType.Passive_Turt:
    //            affectEnemyStat = Passive_Turt;
    //            break;
    //        case PassiveType.Passive_Centaur:
    //            affectPlayerStat = Passive_Centaur;
    //            break;
    //        case PassiveType.Passive_Plant:
    //            affectPlayerStat = Passive_Plant1;
    //            affectPlayerStat += Passive_Plant2;
    //            break;
    //        case PassiveType.Passive_Skorp:
    //            affectEnemyStat = Passive_Skorp;
    //            break;
    //        case PassiveType.Passive_Skorpike:
    //            affectEnemyStat = Passive_Skorpike;
    //            break;
    //        case PassiveType.ActiveBuff_Wolf:
    //            affectStat = ActiveBuff_Wolf;
    //            break;
    //    }
    //}
    //void EmptyDelegate(ref DataMon dataMon, float floatArg = 0)
    //{
    //    return;
    //}
    //void EmptyDelegate(ref GameManager Player, DataMon dataMon, bool toggle)
    //{
    //    return;

    //}
    //void EmptyDelegate(ref DataMon dataMon)
    //{
    //    return;

    //}

    ////int count;
    //float modifier;
    //int currentEvo;
    //private int GetDataMonEvolutionIndex(DataMon dataMon)
    //{
    //    currentEvo = dataMon.tier;
    //    return currentEvo;
    //}
    //void SetAntAttribute(ref DataMon dataMon, int count)
    //{
    //    int evolution = dataMon.tier;
    //    modifier = ModifiersByEvolution[evolution] * count-1;
    //    dataMon.SetAttributesByModifier(modifier);
    //}
    void Passive_Ant(ref DataMon dataMon)
    {
        //DataMonButton[] temp =  DataDex.instance.GetDataMonFromTeam(dataMon.dataMonData);
        ////Debug.Log("Before ? HP " + dataMon.CurrentAttributes.CurrentHealth);
        //for (int i = 0; i < temp.Length; i++)
        //{
        //    SetAntAttribute(ref temp[i].dataMon, temp.Length);
        //}
        //Debug.Log("working? " + modifier);
    }
    void Passive_Bee(ref GameManager GM, DataMon dataMon, bool toggle)
    {

    }
    void Passive_Turt(ref DataMon dataMon, float reflectDamage )
    {

        dataMon.CurrentAttributes.CurrentHealth -= reflectDamage * ModifiersByEvolution[dataMon.tier];
    }
    //void Passive_Centaur(ref GameManager Player, DataMon dataMon, bool toggle)
    //{
    //            //GetDataMonEvolutionIndex(dataMon);
    //    DataMonButton[] temp = DataDex.instance.GetDataMonFromTeam(dataMon.dataMonData);
    //    bool isHighestEvo = temp.Length == 0;
    //    for (int i = 0; i < temp.Length; i++)
    //    {
    //        if (dataMon.tier >= temp[i].dataMon.dataMonData.DataMons.GetDataMonIndexInDataArray(temp[i].dataMon.dataMon))
    //            isHighestEvo = true;
    //    }
    //    if (isHighestEvo)
    //        return;

    //    if (toggle)
    //        Player.MaxShieldHealth = ModifiersByEvolution[dataMon.tier];

    //    if (temp.Length == 1)
    //        Player.isShielded = toggle;
    //}

    
    void Passive_Plant1(ref GameManager Player, DataMon dataMon, bool toggle)
    {
                //GetDataMonEvolutionIndex(dataMon);
        if (toggle)
        {
            Player.PlayerRegenerationRatePerSecond += ModifiersByEvolution[dataMon.tier];
        }
        else
        {
            Player.PlayerRegenerationRatePerSecond -= ModifiersByEvolution[dataMon.tier];
        }
    }
    void Passive_Plant2(ref GameManager Player, DataMon dataMon, bool toggle)
    {

                //GetDataMonEvolutionIndex(dataMon);
        if (toggle)
        {
            Player.AllDamageModifier += ModifiersByEvolution[dataMon.tier];
        }
        else
        {
            Player.AllDamageModifier -= ModifiersByEvolution[dataMon.tier];

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
                //GetDataMonEvolutionIndex(dataMon);
        //dataMon.DataMonStartBuff(10, ModifiersByEvolution[dataMon.tier]);
    }

    //void Passive_Dragon(ref DataMon dataMon)
    //{
    //            //GetDataMonEvolutionIndex(dataMon);
    //    //count = DataDex.instance.GetDataMonCountFromTeam(dataMon.dataMonData);
    //    modifier = ModifiersByEvolution[dataMon.tier];
    //    dataMon.SetAttributesByModifier(modifier);
    //}
}
[System.Serializable]
public class AbilityDescription
{
    public string Name;
    public string Description;
}
    

