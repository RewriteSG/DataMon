using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadManager : MonoBehaviour
{
    public static ResourceLoadManager instance;
    public AbilitiesScriptableObjects[] AbilitiesScriptableObjects;
    public AttackScriptableObject AttackScriptableObject;
    public DataMonsData[] dataMonsData;
    public GameManager gameManager;
    private void Awake()
    {
        instance = this;
        LoadAllResources();
    }
    // Start is called before the first frame update
    public void LoadAllResources()
    {

        gameManager.player_progress.AssaultRifle.prefabName = gameManager.player_progress.AssaultRifle.ItemPrefab.name;
        gameManager.player_progress.DataBall.prefabName = gameManager.player_progress.DataBall.ItemPrefab.name;
        gameManager.player_progress.HuntingRifle.prefabName = gameManager.player_progress.HuntingRifle.ItemPrefab.name;
        gameManager.player_progress.Shotgun.prefabName = gameManager.player_progress.Shotgun.ItemPrefab.name;
        gameManager.player_progress.Melee.prefabName = gameManager.player_progress.Melee.ItemPrefab.name;


        gameManager.assaultRifle.ModelName = gameManager.assaultRifle.Model.name;
        gameManager.DataBallLauncher.ModelName = gameManager.DataBallLauncher.Model.name;
        gameManager.shotgun.ModelName = gameManager.shotgun.Model.name;
        gameManager.huntingRifle.ModelName = gameManager.huntingRifle.Model.name;

        for (int i = 0; i < AttackScriptableObject.AllAttacks.Length; i++)
        {
            AttackScriptableObject.AllAttacks[i].AttackPrefabName = AttackScriptableObject.AllAttacks[i].AttackPrefabName;
        }
        for (int i = 0; i < dataMonsData.Length; i++)
        {
            if(dataMonsData[i].Ability!=null)
            dataMonsData[i].AbilityName = dataMonsData[i].Ability.name;
            for (int x = 0; x < dataMonsData[i].DataMons.Length; x++)
            {
                dataMonsData[i].DataMons[x].DataMonPrefabName = dataMonsData[i].DataMons[x].DataMonPrefab.name;
                dataMonsData[i].DataMons[x].UIspriteName = dataMonsData[i].DataMons[x].UIsprite.name;
            }

        }
        for (int i = 0; i < AbilitiesScriptableObjects.Length; i++)
        {
            if(AbilitiesScriptableObjects[i].ItemPlaceHolder != null)
            AbilitiesScriptableObjects[i].ItemPlaceHolderPrefabName = AbilitiesScriptableObjects[i].ItemPlaceHolder.name;
            AbilitiesScriptableObjects[i].ItemImageName = new string[AbilitiesScriptableObjects[i].ItemImage.Length];
            for (int x = 0; x < AbilitiesScriptableObjects[i].ItemImage.Length; x++)
            {

                if (AbilitiesScriptableObjects[i].ItemImage[x] != null)
                    AbilitiesScriptableObjects[i].ItemImageName[x] = AbilitiesScriptableObjects[i].ItemImage[x].name;
            }

            if (AbilitiesScriptableObjects[i].ItemObject != null)
                AbilitiesScriptableObjects[i].ItemObjectName = AbilitiesScriptableObjects[i].ItemObject.name;
            if(AbilitiesScriptableObjects[i].AbilityPrefab !=null)
            AbilitiesScriptableObjects[i].AbilityPrefabName = AbilitiesScriptableObjects[i].AbilityPrefab.name;
            if(AbilitiesScriptableObjects[i] is Mount)
            {
                Mount mount = (Mount)AbilitiesScriptableObjects[i];
                if(mount.HotbarPrefab != null)
                mount.HotbarPrefabName = mount.HotbarPrefab.name;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
