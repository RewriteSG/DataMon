using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mount_", menuName = "Abilities/Mount/Add Mount", order = 0)]
public class Mount : AbilitiesScriptableObjects
{
    //public GameObject Wolf_HotbarPrefab;
    public GameObject HotbarPrefab;
    public string HotbarPrefabName;
    public Vector2 AttackPoint;

    //public GameObject HotBarInstance;
    //AbilityData abilityData;
    GameObject MountInstance, HotbarInstance;
    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        _AbilityType = AbilityType.Mount;
        base.Activate(dataMonData, dataMon, Player, toggle);


        SpriteRenderer[] spriteRenderers;

        GameObject _Mount = Instantiate(ItemObject, Player.Player.transform);
        _Mount.GetComponent<MountChanger>().currentTier = dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon);
        DataMonActiveAbility[] dataMonActiveAbilities = _Mount.GetComponentsInChildren<DataMonActiveAbility>();
        for (int i = 0; i < dataMonActiveAbilities.Length; i++)
        {
            dataMonActiveAbilities[i].dataMonsData = dataMonData;
            dataMonActiveAbilities[i].individualData = dataMon;

        }

        Player.DataMonMount = _Mount;
        spriteRenderers = _Mount.transform.GetComponentsInChildren<SpriteRenderer>();
        int sortOrder = 0;
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingOrder += 10;
            sortOrder = spriteRenderers[i].sortingOrder;
        }

        spriteRenderers = Player.Player.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingOrder += sortOrder + 20;
        }
        Player.Player.GetComponent<SpriteRenderer>().sortingOrder += 20;

        _Mount.SetActive(false);


        MountInstance = _Mount;
    }
    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        //base.Deactivate(dataMon, Player, toggle);

    }
    public override void RideDataMon(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        Player.DataMonHotBar = Instantiate(HotbarPrefab, Player.canvas.transform);
        DataMonActiveAbility[] dataMonActiveAbilities = Player.DataMonHotBar.GetComponentsInChildren<DataMonActiveAbility>();
        for (int i = 0; i < dataMonActiveAbilities.Length; i++)
        {
            dataMonActiveAbilities[i].dataMonsData = dataMonData;
            dataMonActiveAbilities[i].individualData = dataMon;

        }
        MountInstance.SetActive(true);
        Player.ridingDataMonAttackPoint = new GameObject("newAttackPoint");
        Player.ridingDataMonAttackPoint.transform.SetParent(Player.Player.transform);
        Player.ridingDataMonAttackPoint.transform.localPosition = AttackPoint;
    }
    public override void DismountDataMon(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        Debug.Log("daaaa"); 
        
        MountInstance.SetActive(false);

        Destroy(Player.DataMonHotBar);
        Destroy(Player.ridingDataMonAttackPoint);
    }
    public override AbilitiesScriptableObjects Serialize()
    {
        Mount toReturn = (Mount)base.Serialize();
        toReturn.HotbarPrefab = HotbarPrefab;
        toReturn.AttackPoint = AttackPoint;
        return toReturn;
    }
    public override AbilitiesScriptableObjects GetAbility()
    {
        return this;
    }
}