using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
[DefaultExecutionOrder(-1)]
public class SaveLoadManager : MonoBehaviour
{
    
    public static SaveLoadManager instance;
    public string FileDirectory;
    public string[] AllSavesFile;
    public const string SaveFile = "GameProgress";
    public static string SavesFilePath;
    public bool DoLoadWorld = false;
    public static SavedData savedData;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; FileDirectory = Application.persistentDataPath;

        SavesFilePath = FileDirectory + "/" + SaveFile;
        print(SavesFilePath);
#if UNITY_EDITOR
        if (!System.IO.Directory.Exists(SavesFilePath))
        {
            System.IO.Directory.CreateDirectory(SavesFilePath);
            //UnityEditor.AssetDatabase.CreateFolder("Assets", Saves);

        }
        else
        {
            print("Does it Exist?");
            AllSavesFile = System.IO.Directory.GetFiles(FileDirectory);
        }
#endif
        if (!DoLoadWorld)
        {

            return;
        }
        if (DeserializeSaveData(out SavedData loadedSave))
            savedData = loadedSave;
        else
            savedData = null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public static DataMonHolder[] LoadDataMonsInBank()
    {
        if (!IsDataUnkown())
            return savedData.DataMons.DataMons;
        else

            return new DataMonHolder[] { };
    }
    public static int LoadDataBytes()
    {
        if (!IsDataUnkown())
            return savedData.playerProgress.DataBytes;
        else

            return 0;
    }
    public static PlayerProgress LoadPlayerProgress(Item melee)
    {
        if (!IsDataUnkown())
        {
            PlayerProgress loadedPlayerProgress = new PlayerProgress();
            loadedPlayerProgress.AssaultRifle = savedData.playerProgress.savedProgress.AssaultRifle;
            loadedPlayerProgress.DataBall = savedData.playerProgress.savedProgress.DataBall;
            loadedPlayerProgress.Shotgun = savedData.playerProgress.savedProgress.Shotgun;
            loadedPlayerProgress.HuntingRifle = savedData.playerProgress.savedProgress.HuntingRifle;
            loadedPlayerProgress.Melee = melee;
            return loadedPlayerProgress;
        }
        else
            return new PlayerProgress();
    }
    public static WeaponType[] LoadWeaponTypes()
    {
        List<WeaponType> list = new List<WeaponType>();
        list.Clear();
        if (!IsDataUnkown())
        {

            list.Add(savedData.DataballLauncher);
            list.Add(savedData.HuntingRifle);
            list.Add(savedData.Shotgun);
            list.Add(savedData.AssaultRifle);
            return list.ToArray();
        }
        else
            return list.ToArray();

    }
    public static DataMonHolder[] LoadDataTeamFromSave()
    {
        if(!IsDataUnkown())
            return savedData.DataMons.DataMonsInTeam;
        else

        return new DataMonHolder[] { };
    }
    public static bool IsDataUnkown()
    {
        return savedData == null;
    }
    private static bool DeserializeSaveData(out SavedData DeserializedData)
    {
        DeserializedData = null;
        try
        {

            DeserializedData = (SavedData)JsonUtility.FromJson(System.IO.File.ReadAllText(SavesFilePath + ".json"), typeof(SavedData));

            DeserializedData.playerProgress.savedProgress.AssaultRifle.ItemPrefab = 
                Resources.Load<GameObject>(DeserializedData.playerProgress.savedProgress.AssaultRifle.prefabName);

            DeserializedData.playerProgress.savedProgress.Shotgun.ItemPrefab =
                Resources.Load<GameObject>(DeserializedData.playerProgress.savedProgress.AssaultRifle.prefabName);

            DeserializedData.playerProgress.savedProgress.HuntingRifle.ItemPrefab =
                Resources.Load<GameObject>(DeserializedData.playerProgress.savedProgress.HuntingRifle.prefabName);

            DeserializedData.playerProgress.savedProgress.DataBall.ItemPrefab =
                Resources.Load<GameObject>(DeserializedData.playerProgress.savedProgress.DataBall.prefabName);

            for (int i = 0; i < DeserializedData.DataMons.DataMons.Length; i++)
            {
                DeserializedData.DataMons.DataMons[i].abilities =
                Resources.Load<AbilitiesScriptableObjects>("ScriptableObjects/" + DeserializedData.DataMons.DataMons[i].abilitiesName);

                DeserializedData.DataMons.DataMons[i].dataMonData =
                Resources.Load<DataMonsData>("ScriptableObjects/"+DeserializedData.DataMons.DataMons[i].dataMonDataName);

                DeserializedData.DataMons.DataMons[i].dataMon.UIsprite =
                Resources.Load<Sprite>("UI/" + DeserializedData.DataMons.DataMons[i].dataMon.UIspriteName);

                for (int x = 0; x < DeserializedData.DataMons.DataMons[i].dataMonAttacks.Length; x++)
                {
                    DeserializedData.DataMons.DataMons[i].dataMonAttacks[x].AttackPrefab =
                Resources.Load<GameObject>("Attacks/" + DeserializedData.DataMons.DataMons[i].dataMonAttacks[x].AttackName);
                }
                DeserializedData.DataMons.DataMons[i].dataMon.DataMonPrefab =
                Resources.Load<GameObject>("DataMons/" + DeserializedData.DataMons.DataMons[i].dataMon.DataMonPrefabName);
                //for (int x = 0; x < ; x++)
                //{
                //    DeserializedData.DataMons.DataMons[i].dataMonAttacks[x].AttackPrefab =
                //Resources.Load<GameObject>("Attacks/" + DeserializedData.DataMons.DataMons[i].dataMonAttacks[x].AttackName);
                //}
            }
            for (int i = 0; i < DeserializedData.DataMons.DataMonsInTeam.Length; i++)
            {
                DeserializedData.DataMons.DataMonsInTeam[i].abilities =
                Resources.Load<AbilitiesScriptableObjects>("ScriptableObjects/" + DeserializedData.DataMons.DataMons[i].abilitiesName);



                DeserializedData.DataMons.DataMonsInTeam[i].dataMonData =
                Resources.Load<DataMonsData>("ScriptableObjects/" + DeserializedData.DataMons.DataMons[i].dataMonDataName);

                DeserializedData.DataMons.DataMonsInTeam[i].dataMon.UIsprite =
                Resources.Load<Sprite>("UI/" + DeserializedData.DataMons.DataMonsInTeam[i].dataMon.UIspriteName);

                for (int x = 0; x < DeserializedData.DataMons.DataMons[i].dataMonAttacks.Length; x++)
                {
                    DeserializedData.DataMons.DataMonsInTeam[i].dataMonAttacks[x].AttackPrefab =
                Resources.Load<GameObject>("Attacks/" + DeserializedData.DataMons.DataMonsInTeam[i].dataMonAttacks[x].AttackPrefabName);
                }
                DeserializedData.DataMons.DataMonsInTeam[i].dataMon.DataMonPrefab =
                Resources.Load<GameObject>("DataMons/" + DeserializedData.DataMons.DataMonsInTeam[i].dataMon.DataMonPrefabName);


                //for (int x = 0; x < ; x++)
                //{
                //    DeserializedData.DataMons.DataMons[i].dataMonAttacks[x].AttackPrefab =
                //Resources.Load<GameObject>("Attacks/" + DeserializedData.DataMons.DataMons[i].dataMonAttacks[x].AttackName);
                //}
            }

            DeserializedData.DataballLauncher.Model =
                Resources.Load<GameObject>(DeserializedData.DataballLauncher.ModelName);

            DeserializedData.AssaultRifle.Model =
                Resources.Load<GameObject>(DeserializedData.AssaultRifle.ModelName);

            DeserializedData.Shotgun.Model =
                Resources.Load<GameObject>(DeserializedData.Shotgun.ModelName);

            DeserializedData.HuntingRifle.Model =
                Resources.Load<GameObject>(DeserializedData.HuntingRifle.ModelName);



        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.Log("oof");
            DeserializedData = null;
        }

        return DeserializedData != null;
        
    }

    public static void EndExploration()
    {
        ResourceLoadManager.instance.LoadAllResources();
        for (int i = 0; i < GameManager.instance.DataMonAbilities.Count; i++)
        {
            GameManager.instance.DataMonAbilities[i].Deactivate(GameManager.instance.DataTeam[i].dataMonData, GameManager.instance.DataTeam[i].dataMon
                , GameManager.instance, true);
        }

        SavedData savedData = new SavedData(GameManager.instance.Databytes, GameManager.instance.Player,
            GameManager.instance.player_progress, GameManager.instance.DataBallLauncher, GameManager.instance.huntingRifle,
            GameManager.instance.shotgun, GameManager.instance.assaultRifle,
            DataDex.instance.ToDataHub.ToArray(), GameManager.instance.DataTeam.RemoveNullReferencesFromArray());

        string savedJson = JsonUtility.ToJson(savedData, true);
        System.IO.File.WriteAllText(/*SavesFilePath + "" + */SavesFilePath + ".json", savedJson);
    }
    public void SaveHub()
    {

        ResourceLoadManager.instance.LoadAllResources();
        savedData = new SavedData(GameManager.instance.Databytes, GameManager.instance.Player,
            GameManager.instance.player_progress, GameManager.instance.DataBallLauncher, GameManager.instance.huntingRifle,
            GameManager.instance.shotgun, GameManager.instance.assaultRifle, 
            DataDex.GetAllDataMonsInHub(), DataDex.instance.DataTeam.RemoveNullReferencesFromArray());

        string savedJson = JsonUtility.ToJson(savedData, true);
        System.IO.File.WriteAllText(SavesFilePath +".json", savedJson);
    }
}
[System.Serializable]
public class SavedData
{
    public string SaveDataName;
    
    public SavedPlayerProgress playerProgress;
    public SavedDataMons DataMons;
    public WeaponType DataballLauncher, HuntingRifle, Shotgun, AssaultRifle;
    public SavedData(int DataBytes, GameObject Player, PlayerProgress _playerProgress, WeaponType _DataBallLauncher,
        WeaponType _HuntingRifle, WeaponType _Shotgun, WeaponType _AssaultRifle, DataMonHolder[] DataMonsInBank, DataMonHolder[] DataTeam)
    {

        
        playerProgress = new SavedPlayerProgress(_playerProgress, DataBytes);

        //playerProgress.savedProgress.AssaultRifle.prefabName = playerProgress.savedProgress.AssaultRifle.ItemPrefab.name;
        //playerProgress.savedProgress.AssaultRifle.prefabName = playerProgress.savedProgress.AssaultRifle.ItemPrefab.name;
        //playerProgress.savedProgress.AssaultRifle.prefabName = playerProgress.savedProgress.AssaultRifle.ItemPrefab.name;
        //playerProgress.savedProgress.AssaultRifle.prefabName = playerProgress.savedProgress.AssaultRifle.ItemPrefab.name;

        DataMons = new SavedDataMons(DataMonsInBank, DataTeam);

        DataballLauncher = _DataBallLauncher;
        HuntingRifle = _HuntingRifle;
        Shotgun = _Shotgun;
        AssaultRifle = _AssaultRifle;

    }
}
[System.Serializable]
public class SavedPlayerProgress
{
    public PlayerProgress savedProgress = new PlayerProgress();
    public int DataBytes;
    public SavedPlayerProgress(PlayerProgress playerProgress, int _Databytes)
    {
        savedProgress = playerProgress;
        DataBytes = _Databytes;
    }
}
[System.Serializable]
public class SavedWeaponData
{
    public int AmmoAmount, ClipAmount, CurrentClipAmount;

}
[System.Serializable]
public class SavedDataMons
{
    public DataMonHolder[] DataMons = new DataMonHolder[] { };
    public DataMonHolder[] DataMonsInTeam = new DataMonHolder[] { };
    //public AttackScriptableObject DataMonsAttacks;
    public SavedDataMons(DataMonHolder[] _DataMons, DataMonHolder[] _DataMonsInTeam)
    {
        //DataMonsAttacks = ScriptableObject.CreateInstance<AttackScriptableObject>();
        //DataMonsAttacks.AllAttacks = _DataMons[_DataMons.Length - 1].dataMonData.AttacksObjects.AllAttacks;
        DataMons = _DataMons;
        DataMonsInTeam = _DataMonsInTeam;
    }
}
[System.Serializable]
public class SavedEntityData
{
    public float PosX, PosY;
    public float RotX, RotY, RotZ;
    public DataMonHolder DataMon;
    public SavedEntityData(Vector2 position, Vector3 rotation, bool isPlayer)
    {
        PosX = position.x;
        PosY = position.y;
        RotX = rotation.x;
        RotY = rotation.y;
        RotZ = rotation.z;
    }
    public SavedEntityData(Vector2 position, Vector3 rotation, DataMonHolder _DataMon)
    {
        PosX = position.x;
        PosY = position.y;
        RotX = rotation.x;
        RotY = rotation.y;
        RotZ = rotation.z;
        DataMon = _DataMon;
    }
}