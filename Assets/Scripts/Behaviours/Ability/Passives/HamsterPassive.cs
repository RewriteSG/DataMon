using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "Passive_", menuName = "Abilities/Passives/Add Hamster Ability")]
public class HamsterPassive : AbilitiesScriptableObjects
{
   
        public override void Activate(DataMonsData dataMonData, DataMonIndividualData dataMon, GameManager Player, bool toggle)
        {
            base.Activate(dataMonData, dataMon, Player, toggle);
            Player.isHamsterPassive = true;

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
