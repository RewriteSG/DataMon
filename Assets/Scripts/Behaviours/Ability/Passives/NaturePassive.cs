using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passives_Nature", menuName = "Abilities/Passives/Add Nature Passive")]
public class NaturePassive : AbilitiesScriptableObjects
{
    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        base.Activate(dataMonData, dataMon, Player, toggle);

        // Increase Damage 
        Player.isHealedByNatureGuy = true;
        Player.PlayerRegenerationRatePerSecond += ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.AssaultRifle.WeaponModifiers.Damage += ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.HuntingRifle.WeaponModifiers.Damage += ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.Shotgun.WeaponModifiers.Damage += ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.Shotgun.WeaponModifiers.Damage += ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Instantiate(AbilityPrefab, Player.Player.transform);
        Debug.Log("Hhaa");
    }
    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        //base.Deactivate(dataMon, Player, toggle);
        Player.PlayerRegenerationRatePerSecond -= ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.Melee.WeaponModifiers.Damage -= ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.AssaultRifle.WeaponModifiers.Damage -= ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.HuntingRifle.WeaponModifiers.Damage -= ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        Player.player_progress.Shotgun.WeaponModifiers.Damage -= ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];

    }
    public override AbilitiesScriptableObjects GetAbility()
    {
        return this;
    }
    public override AbilitiesScriptableObjects Serialize()
    {
        return this;
    }

}