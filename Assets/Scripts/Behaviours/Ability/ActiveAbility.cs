using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Active_", menuName = "Abilities/Active/Add Active Weapon")]
public class ActiveAbility : AbilitiesScriptableObjects

{

    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        //_AbilityType = AbilityType.ActiveAbility;

        base.Activate(dataMonData, dataMon, Player, toggle);

    }
    public override GameObject UseAbility(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle, Transform Gunpoint = default)
    {
        //base.StartAbility(dataMon, Player, toggle);
        AttackObjects attack = Instantiate(AbilityPrefab, Gunpoint.position, Gunpoint.rotation).GetComponent<AttackObjects>();
        attack.AttacksByEntityGameObject = Player.Player.gameObject;
        attack.gameObject.tag = "AllyAttack";
        attack.Damage = attack.DmgBasedOfStat * dataMon.BaseAttributes.BaseAttack;
        return attack.gameObject;
    }
    //public override void UseAbility(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle, Transform Gunpoint = default)
    //{
    //    //base.StartAbility(dataMon, Player, toggle);

    //}
    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        //base.Deactivate(dataMon, Player, toggle);

    }
    public override AbilitiesScriptableObjects Serialize()
    {
        ActiveAbility toReturn = (ActiveAbility)base.Serialize();

        return toReturn;
    }
    public override AbilitiesScriptableObjects GetAbility()
    {
        return this;
    }

}
