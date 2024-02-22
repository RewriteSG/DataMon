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
    public static PlayerProgress LoadPlayerProgress()
    {
        if (!IsDataUnkown())
        {
            PlayerProgress loadedPlayerProgress = new PlayerProgress();
            loadedPlayerProgress.AssaultRifle = savedData.playerProgress.savedProgress.AssaultRifle;
            loadedPlayerProgress.DataBall = savedData.playerProgress.savedProgress.DataBall;
            loadedPlayerProgress.Shotgun = savedData.playerProgress.savedProgress.Shotgun;
            loadedPlayerProgress.HuntingRifle = savedData.playerProgress.savedProgress.HuntingRifle;
            loadedPlayerProgress.Melee = savedData.playerProgress.savedProgress.Melee;
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
    private static bool DeserializeSaveData(out SavedData DeerializedData)
    {
        DeerializedData = null;
        try
        {

            DeerializedData = (SavedData)JsonUtility.FromJson(System.IO.File.ReadAllText(SavesFilePath + ".json"), typeof(SavedData));
        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.Log("oof");
            DeerializedData = null;
        }

        return DeerializedData != null;
        
    }

    public static void EndExploration()
    {
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