using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int Count;
        public float rate;
    }
    public Wave[] waves;
    private int NextWave = 0;

    public Transform[] SpawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private SpawnState state = SpawnState.Counting;

    private float searchCountDown = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (SpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced. ");
        }

        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.Waiting)
        {
            if(!EnemyIsAlive())
            {
                // begin a new round

                WaveCompleted();
            }
            else
            {
                return;
            }
        }
        if(waveCountdown<=0)
        {
            if(state!=SpawnState.Spawning)
            {
                //start spawning wave
                StartCoroutine(SpawnWave(waves[NextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        state = SpawnState.Counting;

        waveCountdown = timeBetweenWaves;

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

            if (GameObject.FindGameObjectWithTag("enemy") == null)
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

        for (int i = 0;i<_wave.Count;i++)
        {
            SpawnEnemy(_wave.enemy);

            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.Waiting;


        yield break;
    }
    void SpawnEnemy(Transform _enemy)
    {
        
        Debug.Log("Spawning enemy: " + _enemy.name);

        
        Transform _sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}
