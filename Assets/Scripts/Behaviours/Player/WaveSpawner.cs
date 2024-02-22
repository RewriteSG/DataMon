using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using IndividualDataMon;
public class WaveSpawner : MonoBehaviour
{
    public DataMonsData[] dataMonsDatas;
    public GameObject[] AllDataMonPrefabs = new GameObject[] {};
    public enum SpawnState { Spawning, Waiting, Counting};

    //[System.Serializable]
    //public class Wave
    //{
    //    public string name;
    //    public Transform enemy;
    //    public int Count;
    //    public float rate;
    //}
    public WaveManager wm;
    public Wave[] waves;
    private int NextWave = 0;

    public Transform[] SpawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    public int WavesBeforeIncreaseDifficulty;

    private SpawnState state = SpawnState.Counting;

    public Wave.WaveDifficulty currentDifficulty;

    public RoamingSpawner roamingSpawner;

    private float searchCountDown = 1f;

    public TextMeshProUGUI waveIncomingText;

    public static int enemyCount;

    // Start is called before the first frame update
    private void Awake()
    {
        DataMon dataMon;
        for (int i = 0; i < dataMonsDatas.Length; i++)
        {
            for (int x = 0; x < dataMonsDatas[i].DataMons.Length; x++)
            {
                dataMon = dataMonsDatas[i].DataMons[x].DataMonPrefab.GetComponent<DataMon>();
                dataMon.SetDataMon(dataMon.dataMonData.DataMons.GetDataMonInDataArray(dataMon.gameObject));
            }
        }

        //for (int i = 0; i < AllDataMonPrefabs.Length; i++)
        //{
        //    dataMon = AllDataMonPrefabs[i].GetComponent<DataMon>();
        //    dataMon.SetDataMon(dataMon.dataMonData.DataMons.GetDataMonInDataArray(dataMon.gameObject));
        //}
    }
    void Start()
    {
        wm = GetComponent<WaveManager>();
        waves = wm.Waves.ToArray();
        if (SpawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points referenced. ");
        }

        waveCountdown = timeBetweenWaves;
        enemyCount = 0;
        ChangeDifficulty(Wave.WaveDifficulty.Easy);
    }
    public void ChangeDifficulty(Wave.WaveDifficulty toDifficulty)
    {
        currentDifficulty = toDifficulty;
        //roamingSpawner.AddDataMonToRoamingDatabase(toDifficulty);
    }
    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                // begin a new round

                WaveCompleted();
            }
            else
            {
                if (NextWave != waves.Length - 1)
                    waveIncomingText.text = waves[NextWave].name;
                else
                    waveIncomingText.text = "";

                return;
            }
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {
                //start spawning wave
#if UNITY_EDITOR
                NextWave = waves.Length - 1;
#endif
                StartCoroutine(SpawnWave(waves[NextWave]));

            }
        }
        else
        {
            waveIncomingText.text = "Wave Incoming: " + waveCountdown.ToString("0.#s");
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        RoamingSpawner.ClearRoamingDataMons = false;

        state = SpawnState.Counting;

        //switch (currentDifficulty)
        //{
        //    case Wave.WaveDifficulty.Easy:
        //        waveCountdown = timeBetweenWaves;
        //        break;
        //    case Wave.WaveDifficulty.Normal:
        //        waveCountdown = timeBetweenWaves*1.3f;
        //        break;
        //    case Wave.WaveDifficulty.Hard:
        //        waveCountdown = timeBetweenWaves * 1.5f;
        //        break;
        //    case Wave.WaveDifficulty.Difficult:

        //        waveCountdown = timeBetweenWaves * 1.6f;
        //        break;
        //    case Wave.WaveDifficulty.Hell:
        //        waveCountdown = timeBetweenWaves * 1.7f;
        //        break;
        //}
        waveCountdown = timeBetweenWaves;

        if (NextWave +1 == WavesBeforeIncreaseDifficulty)
        {
            WavesBeforeIncreaseDifficulty *= 2;
            currentDifficulty +=  1;
            //roamingSpawner.AddDataMonToRoamingDatabase(currentDifficulty);
        }

        if (NextWave + 1>waves.Length-1)
        {
            NextWave = 0;
            Debug.Log("ALL WAVES COMPLETE! Looping...");
        }
        else
        {
            NextWave++;
        }

        
    }

    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if(searchCountDown<=0f)
        {
            searchCountDown = 1f;

            if (enemyCount <= 0)
            {
                return false;
            }
        }
        
        return true;
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave: " + _wave.name);



        state = SpawnState.Spawning;

        RoamingSpawner.ClearRoamingDataMons = true;

        for (int x = 0; x < _wave._EnemiesInWave.Count; x++)
        {
            SpawnEnemy(_wave._EnemiesInWave[x]);
        }
        

        state = SpawnState.Waiting;


        yield break;
    }
    void SpawnEnemy(EnemiesInWave _Enemy)
    {
        
        Debug.Log("Spawning enemy: " + _Enemy.name);
        Transform _sp; 
        for (int i = 0; i < _Enemy.Count; i++)
        {
            _sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            weTryNormalMethod(Instantiate(_Enemy.DataMon.DataMonPrefab, (Vector2)_sp.position, _sp.rotation), _Enemy._DataMonsData, _Enemy.DataMon);
            enemyCount++;
        }
        
        //StartCoroutine(SetEnemyAngry(Instantiate(, (Vector2)_sp.position, _sp.rotation)));
    }
    IndividualDataMon.DataMon dataMon;
    void weTryNormalMethod(GameObject enemy, DataMonsData dataMonsData, DataMonIndividualData dataMonIndividualData)
    {
        print("Object is destroyed?");
        dataMon = enemy.GetComponent<DataMon>();

        dataMon.SetDataMonData(dataMonsData);

        dataMon.SetDataMon(dataMonIndividualData);

        dataMon.SetAttributes(new DataMonInstancedAttributes(dataMon.dataMon.BaseAttributes));

        dataMon.gameObject.AddComponent<WaveEnemyScript>();


        dataMon.isWaveEnemy = true;
        Debug.Log("How are ");
        dataMon.SetDataMonHostile();

        dataMon.dataMonAI.ChangeAttackTargetEnemy(GameManager.instance.Player.gameObject);
    }

    IEnumerator SetEnemyAngry(GameObject enemy, DataMonsData dataMonsData, DataMonIndividualData dataMonIndividualData)
    {

        print("Object is destroyed?");
        dataMon = enemy.GetComponent<DataMon>();

        dataMon.SetDataMonData(dataMonsData);

        dataMon.SetDataMon(dataMonIndividualData);

        dataMon.SetAttributes(new DataMonInstancedAttributes(dataMon.dataMon.BaseAttributes));


        yield return new WaitForEndOfFrame();

        dataMon.isWaveEnemy = true;
        Debug.Log("How are ");
        dataMon.SetDataMonHostile();

        dataMon.dataMonAI.ChangeAttackTargetEnemy(GameManager.instance.Player.gameObject);


    }
}
