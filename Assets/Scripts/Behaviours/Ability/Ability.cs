using IndividualDataMon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ability : AbilitiesScriptableObjects
{

}


//[CreateAssetMenu(fileName = "Ability Name", menuName = "Abilities/Add Scorpion Ability")]
//public class Ability_Scorpion : AbilitiesScriptableObjects
//{
//    //public KeyCode Input1, Input2;
//    //public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    //{
//    //    base.Activate(dataMonData, dataMon, Player, toggle);
//    //}
//    public override GameObject UseAbility(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle, Transform Gunpoint = default)
//    {
//        //base.StartAbility(dataMon, Player, toggle);
//        AttackObjects attack = Instantiate(AbilityPrefab, Gunpoint.position, Gunpoint.rotation).GetComponent<AttackObjects>();
//        attack.AttacksByEntityGameObject = Player.Player.gameObject;
//        attack.gameObject.tag = "AllyAttack";
//        attack.Damage = attack.DmgBasedOfStat * dataMon.BaseAttributes.BaseAttack;

//        return attack.gameObject;
//    }
//    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.Deactivate(dataMon, Player, toggle);

//    }
//    public override void RideDataMon(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.RideDataMon(dataMonData, dataMon, Player, toggle);
//    }
//    public override void DismountDataMon(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.DismountDataMon(dataMonData, dataMon, Player, toggle);
//    }
//    public override AbilitiesScriptableObjects Serialize()
//    {
//        return this;
//    }

//    public override AbilitiesScriptableObjects GetAbility()
//    {
//        return this;
//    }

//}
//[CreateAssetMenu(fileName = "Turtle_Ability", menuName = "Abilities/Active/Add Turtle Ability")]
//public class Ability_Turtle : AbilitiesScriptableObjects
//{
//    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.Activate(dataMonData, dataMon, Player, toggle);

//    }
//    //public KeyCode Input1, Input2;
//    public override void UseAbility(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle, Transform Gunpoint = default)
//    {
//        //base.StartAbility(dataMon, Player, toggle);

//        //base.StartAbility(dataMon, Player, toggle);
//        //Vector3 objPos = Camera.main.WorldToScreenPoint(Player.Player.transform.position);
//        //Vector3 mousePos = Input.mousePosition;
//        ////mousePos.y = Camera.main.transform.position.y - transform.position.y;

//        //mousePos -= objPos;
//        //mousePos = mousePos.normalized;
//        //print("mos" + mousePos);
//        //float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
        

//        //if (!attack.isDashAttack)
//        //    attack.transform.position = Player.ridingDataMonAttackPoint.transform.position;
//        //attack.transform.localScale = (modelScale / 0.7054937403162723f);


//    }
//    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.Deactivate(dataMon, Player, toggle);

//    }
//    public override AbilitiesScriptableObjects Serialize()
//    {
//        return this;
//    }

//    public override AbilitiesScriptableObjects GetAbility()
//    {
//        return this;
//    }

//}
//[CreateAssetMenu(fileName = "Ability Name", menuName = "Abilities/Add Bee Ability")]
//public class Ability_Bee : AbilitiesScriptableObjects
//{
//    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.StartAbility(dataMon, Player, toggle);

//    }
//    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.Deactivate(dataMon, Player, toggle);
//    }
//    public override AbilitiesScriptableObjects Serialize()
//    {
//        return this;
//    }
//    public override AbilitiesScriptableObjects GetAbility()
//    {
//        return this;
//    }

////}
//[CreateAssetMenu(fileName = "Ability Name", menuName = "Abilities/Add Drake Ability")]
//public class Ability_Drake : AbilitiesScriptableObjects
//{
//    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.StartAbility(dataMon, Player, toggle);


//    }
//    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.Deactivate(dataMon, Player, toggle);

//    }
//    public override AbilitiesScriptableObjects Serialize()
//    {
//        return this;
//    }
//    public override AbilitiesScriptableObjects GetAbility()
//    {
//        return this;
//    }

//}
//[CreateAssetMenu(fileName = "Ability Name", menuName = "Abilities/Add Nature Ability")]
//public class Ability_Nature : AbilitiesScriptableObjects
//{
//    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.StartAbility(dataMon, Player, toggle);


//    }
//    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
//    {
//        //base.Deactivate(dataMon, Player, toggle);

//    }
//    public override AbilitiesScriptableObjects Serialize()
//    {
//        return this;
//    }

//    public override AbilitiesScriptableObjects GetAbility()
//    {
//        return this;
//    }
//}

[CreateAssetMenu(fileName = "Ability Name", menuName = "Abilities/Add Centaur Ability")]
public class Ability_Centaur : AbilitiesScriptableObjects
{
    public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        //base.StartAbility(dataMon, Player, toggle);

    }
    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        //base.Deactivate(dataMon, Player, toggle);

    }

    public override AbilitiesScriptableObjects Serialize()
    {
        return this;
    }
    public override AbilitiesScriptableObjects GetAbility()
    {
        return this;
    }
}