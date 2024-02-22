using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMonItemAbility : MonoBehaviour
{
    public AbilitiesScriptableObjects ability;
    public DataMonsData _dataMonData;
    public DataMonIndividualData _dataMon;
    public GameManager GM; bool toggle;
    [SerializeField]GameObject weaponInstance;
    public void SetValue(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager _GM, bool toggie, AbilitiesScriptableObjects _ability)
    {
        _dataMonData = dataMonData;
        _dataMon = dataMon;
        GM = _GM;
        toggle = toggie;
        ability = _ability;
    }
    public void ItemSelected()
    {
        
        switch(ability._AbilityType)
        {
            case AbilitiesScriptableObjects.AbilityType.Mount:
                ability.RideDataMon(_dataMonData, _dataMon, GM, toggle);
                break;
            case AbilitiesScriptableObjects.AbilityType.ActiveAbility:

                GM.playerShootScript.ShowWeaponModel(ItemHolding.None);
                weaponInstance = Instantiate(ability.ItemObject, GM.Player.transform);

                break;

        }
    }
    public void ItemUnselected()
    {

        switch (ability._AbilityType)
        {
            case AbilitiesScriptableObjects.AbilityType.Mount:
                ability.DismountDataMon(_dataMonData, _dataMon, GM, toggle);
                break;
            case AbilitiesScriptableObjects.AbilityType.ActiveAbility:
                Destroy(weaponInstance);
                break;
        }
    }
    public void UseItem()
    {
        //switch (ability._AbilityType)
        //{
        //    case AbilitiesScriptableObjects.AbilityType.Mount:
        //        //ability.UseAbility(_dataMonData, _dataMon, GM, toggle);
        //        break;
        //    case AbilitiesScriptableObjects.AbilityType.ActiveAbility:
        //        ability.UseAbility(_dataMonData, _dataMon, GM, toggle);
        //        break;
        //}
    }
}
