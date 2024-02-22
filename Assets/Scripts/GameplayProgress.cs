using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayProgress : MonoBehaviour
{
    public GameObject LoadSavePanel;
    public void CheckSaveExist()
    {
        string FileDirectory = Application.persistentDataPath;
        string SavesFilePath = FileDirectory + "/" + SaveLoadManager.SaveFile;
        print(SavesFilePath);
        bool isCreated = false; 
        if (!System.IO.Directory.Exists(SavesFilePath))
        {
            System.IO.Directory.CreateDirectory(SavesFilePath);
            //UnityEditor.AssetDatabase.CreateFolder("Assets", Saves);
            isCreated = true;
        }
        else
        {
            print("Does it Exist?");
            isCreated = !System.IO.File.Exists(FileDirectory+"/"+SaveLoadManager.SaveFile+".json");
        }
        if (isCreated)
        {
            StartGame();
        }
        else
        {
            LoadSavePanel.SetActive(true);
        }

    }

    public void StartGame()
    {
        SaveLoadManager.LoadWorld = false;
        SceneChanger.ChangeScene("TestScene");
    }

    public void LoadWorldTrue()
    {
        SaveLoadManager.LoadWorld = true;
    }
    
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
