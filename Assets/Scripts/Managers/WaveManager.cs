using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public float IncomingWaveInSeconds;
    float timer;
    public List<Wave> Waves = new List<Wave>();

    public Wave serializeWave;
    public EnemiesInWave serializeEnemiesInWave;
    public Wave.WaveDifficulty serializedifficulty;
    public DataMonsData serializeDataMonData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddWave()
    {
        Waves.Add(new Wave());
    }
    public string[] GetAllWavesInWaveManager()
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < Waves.Count; i++)
        {
            temp.Add("Wave "+i+": "+Waves[i].name);
        }
        temp.Add("Add Wave");
        return temp.ToArray();
    }
    public void RemoveWave(int index)
    {
        Waves.RemoveAt(index);
    }
}

[System.Serializable]
public class Wave
{
    public string name;
    [HideInInspector]public int NumberOfEnemiesInWave;
    [HideInInspector] public List<EnemiesInWave>_EnemiesInWave = new List<EnemiesInWave>();
    public WaveDifficulty difficulty = WaveDifficulty.Easy;
    public void AddEnemy()
    {
        Debug.Log("How are u not printing");
        EnemiesInWave temp = new EnemiesInWave();
        temp._DataMonsData = null;
        _EnemiesInWave.Add(temp);
    }
    public void RemoveEnemy(int index)
    {
        _EnemiesInWave.RemoveAt(index);
    }
    public string[] GetAllDataMonsInWave()
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < _EnemiesInWave.Count; i++)
        {
            temp.Add("Enemy "+i+": "+_EnemiesInWave[i].name);
        }
        temp.Add("Add Enemy To Wave");
        return temp.ToArray();
    }
    public enum WaveDifficulty
    {
        Easy = 1,
        Normal = 2,
        Hard = 3, 
        Difficult = 4,
        Hell = 5
    }
    public int GetTotalNumberOfEnemies()
    {
        if (_EnemiesInWave.Count == 0)
            return 0;
        int temp =0;
        for (int i = 0; i < _EnemiesInWave.Count; i++)
        {
            temp += _EnemiesInWave[i].Count;
        }
        return temp;
    }
}
[System.Serializable]
public class EnemiesInWave
{
    public string name;
    [HideInInspector] public DataMonsData _DataMonsData;
    [HideInInspector] public DataMonIndividualData DataMon = new DataMonIndividualData();
    [HideInInspector]public int DataMonsDataIndex = 0;
    public int Count;
    public string[] GetAllDataMonsInData()
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < _DataMonsData.DataMons.Length; i++)
        {
            temp.Add(_DataMonsData.DataMons[i].DataMonName);
        }
        return temp.ToArray();
    }
    public void SetEnemy(DataMonIndividualData _dataMon)
    {

        DataMon = _dataMon;
    }
}
