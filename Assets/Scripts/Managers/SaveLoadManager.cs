using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class SaveLoadManager : MonoBehaviour
{
    
    public static SaveLoadManager instance;
    public string FileDirectory;
    public string[] AllSavesFile;
    public const string Saves = "Saves";
    string SavesFilePath;
    public bool DoLoadWorld = false;
    SavedData savedData;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        FileDirectory = Application.persistentDataPath;

        SavesFilePath = FileDirectory + "/" + Saves;
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
        if (DoLoadWorld)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveWorld()
    {

        savedData = new SavedData(RoamingSpawner.ALLDataMons.ToArray(), GameManager.instance.Player,
            GameManager.instance.player_progress, GameManager.instance.DataBallLauncher, GameManager.instance.huntingRifle,
            GameManager.instance.shotgun, GameManager.instance.assaultRifle, DataDex.instance.DataMonObtained.GetDataMonHoldersFromArray(), DataDex.instance.DataTeam.RemoveNullReferencesFromArray());
        string savedJson = JsonUtility.ToJson(savedData, true);
        System.IO.File.WriteAllText(SavesFilePath + "/Save " + (AllSavesFile.Length + 1) + ".json", savedJson);
    }
}
[System.Serializable]
public class SavedData
{
    public string SaveDataName;
    public SavedEntityData[] DataWorldEntities = new SavedEntityData[] { };
    public SavedPlayerProgress playerProgress;
    public SavedDataMons DataMons;
    public SavedData(IndividualDataMon.DataMon[] _DataMons, GameObject Player, PlayerProgress _playerProgress,WeaponType DataBallLauncher,
        WeaponType HuntingRifle, WeaponType Shotgun, WeaponType AssaultRifle, DataMonHolder[] DataMonsInBank, DataMonHolder[] DataTeam)
    {
        List<SavedEntityData> temp_DataWorldEntities = new List<SavedEntityData>();
        for (int i = 0; i < _DataMons.Length; i++)
        {
            if (_DataMons[i].dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion)
                continue;
            temp_DataWorldEntities.Add(new SavedEntityData(_DataMons[i].transform.position,
                _DataMons[i].transform.rotation.eulerAngles,new DataMonHolder(_DataMons[i])));
        }
        temp_DataWorldEntities.Add(new SavedEntityData(Player.transform.position, Player.transform.rotation.eulerAngles, true));
        DataWorldEntities = temp_DataWorldEntities.ToArray();
        playerProgress = new SavedPlayerProgress(_playerProgress);

        DataMons = new SavedDataMons(DataMonsInBank, DataTeam);

    }
}
[System.Serializable]
public class SavedPlayerProgress
{
    public PlayerProgress savedProgress = new PlayerProgress();
    
    public SavedPlayerProgress(PlayerProgress playerProgress)
    {
        savedProgress = playerProgress;
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
    public SavedDataMons(DataMonHolder[] _DataMons, DataMonHolder[] _DataMonsInTeam)
    {
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