using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MountAbility_", menuName = "Abilities/Mount/Add Mount Ability")]
public class MountAbility : AbilitiesScriptableObjects
{
    //public Vector2 AttackPointOffset;
    public float ChargeTime;
    public float FuelUseThreshold = 1;

    public Vector2 modelScale = Vector2.one;
    [Header("If it is Fuel, Use Cooldown as number for Amount of fuel and Use ChargeTime for fuel Regen")]
    public TypeOfAbility typeOfAbility;
    

    public enum TypeOfAbility
    {
        Active, Fuel, Charging
    }
    public override GameObject UseAbility(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle, Transform Gunpoint = default)
    {
        //base.StartAbility(dataMon, Player, toggle);
        Vector3 objPos = Camera.main.WorldToScreenPoint(Player.Player.transform.position);
        Vector3 mousePos = Input.mousePosition;
        //mousePos.y = Camera.main.transform.position.y - transform.position.y;

        mousePos -= objPos;
        mousePos = mousePos.normalized;
        //print("mos" + mousePos);
        float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
        AttackObjects attack = Instantiate(AbilityPrefab, Player.Player.transform.position, Quaternion.Euler(new Vector3(0, 0, -angle))).GetComponent<AttackObjects>();
        attack.AttacksByEntityGameObject = Player.Player.gameObject;

        if (!attack.isDashAttack)
            attack.transform.position = Player.ridingDataMonAttackPoint.transform.position;
        attack.transform.localScale = (modelScale / 0.7054937403162723f);
        Debug.Log(attack.name);
        attack.Damage = dataMon.BaseAttributes.BaseAttack * ModifiersByEvolution[dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon)];
        attack.gameObject.tag = "AllyAttack";
        attack.Damage = attack.DmgBasedOfStat * dataMon.BaseAttributes.BaseAttack;
        return attack.gameObject;
    }
    public override void Deactivate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    {
        //base.Deactivate(dataMon, Player, toggle);
        
    }
    //public override void RideDataMon(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
    //{
    //    GameObject Mount = Instantiate(WolfMount, Player.Player.transform);
    //    SpriteRenderer[] spriteRenderers = Player.Player.GetComponentsInChildren<SpriteRenderer>();
    //    for (int i = 0; i < spriteRenderers.Length; i++)
    //    {
    //        spriteRenderers[i].sortingLayerID += 40;
    //    }

    //    spriteRenderers = Mount.transform.GetComponentsInChildren<SpriteRenderer>();

    //    for (int i = 0; i < spriteRenderers.Length; i++)
    //    {
    //        spriteRenderers[i].sortingLayerID = 2;
    //    }

    //}
    public override AbilitiesScriptableObjects Serialize()
    {
        MountAbility toReturn = (MountAbility)base.Serialize();
        toReturn.modelScale = modelScale;

        return toReturn;
    }
    public override AbilitiesScriptableObjects GetAbility()
    {
        return this;
    }

}
