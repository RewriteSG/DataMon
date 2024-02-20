using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndividualDataMon;
[DefaultExecutionOrder(2)]
public class RoamingSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] AllDataMonPrefabs;
    List<DataMon> Datamons_roamingData = new List<DataMon>();
    public static Dictionary<string, List<DataMon>> DataMonsPool = new Dictionary<string, List<DataMon>>();
    public static Dictionary<Vector2, DataMonsInChunk> MonsInChunk = new Dictionary<Vector2, DataMonsInChunk>();
    public static List<DataMon> ALLDataMons = new List<DataMon>();
    
    //Easy = 1,
    //    Normal = 2,
    //    Hard = 3, 
    //    Difficult = 4,
    //    Hell = 5

    public List<DataMon> RoamingDataBase = new List<DataMon>();

    public List<DataMon> EasyDataMons = new List<DataMon>(), NormalDataMons = new List<DataMon>(),
        HardDataMons = new List<DataMon>(), DifficultDataMons = new List<DataMon>(), HellDataMons = new List<DataMon>();



    private Vector3 SpawnPosition;
    public float MaxNumberOfDataMonInChunk = 20;
    [SerializeField] int serializedDoot;
    public static int doot_doot;
    public static List<Transform> SpawnPointsInRenderDist = new List<Transform>();
    public static GameObject DataMonPoolGO;
    Vector3 RandomSpawnPoint;
    // Start is called before the first frame update
    void Awake()
    {
        SpawnPointsInRenderDist.Clear();
        ALLDataMons.Clear();
        doot_doot = 0;
        for (int i = 0; i < AllDataMonPrefabs.Length; i++)
        {
            Datamons_roamingData.Add(AllDataMonPrefabs[i].GetComponent<DataMon>());
            Datamons_roamingData[Datamons_roamingData.Count - 1].SetDataMon(
                Datamons_roamingData[Datamons_roamingData.Count - 1].dataMonData.DataMons.
                GetDataMonInDataArray(Datamons_roamingData[Datamons_roamingData.Count - 1].gameObject));
        }
        List<DataMon> temp;
        DataMonPoolGO = new GameObject("DataMons");
        //DataMonPoolGO.transform.position = new Vector3(0, -9000);
        DataMon _datamon;
        if(!SaveLoadManager.instance.DoLoadWorld)
            for (int i = 0; i < AllDataMonPrefabs.Length; i++)
            {
                temp = new List<DataMon>();

                for (int x = 0; x < GameManager.instance.MaxNumberOfWildDataMons; x++)
                {
                    _datamon = Instantiate(AllDataMonPrefabs[i], DataMonPoolGO.transform).GetComponent<DataMon>();
                    temp.Add(_datamon);
                    ALLDataMons.Add(_datamon);
                    temp[temp.Count - 1].gameObject.SetActive(false);
                }
                DataMonsPool.Add(Datamons_roamingData[i].dataMon.DataMonName, temp);
               
            }
        ClearRoamingDataMons = false;
        StartCoroutine(SpawnInBatches(3, 10));
    }

    bool isReferenced;
    int rejected;
    public static bool ClearRoamingDataMons = false;
    IEnumerator SpawnInBatches(float delay, int DataMonPerBatch)
    {
        while (isActiveAndEnabled)
        {
           


            doot_doot = Mathf.Clamp(doot_doot, 0, GameManager.instance.MaxNumberOfWildDataMons + 1);
            if ((doot_doot >= GameManager.instance.MaxNumberOfWildDataMons || SpawnPointsInRenderDist.Count == 0) || ClearRoamingDataMons)
            {
                goto StopSpawning;
            }

            //var wanted = Random.Range(MinY, MaxY);
            //var wanted2 = Random.Range(MinX, MaxX);
            //if (Vector2.Distance(RandomSpawnPoint,GameManager.instance.RenderDistance))
            for (int i = 0; i <= DataMonPerBatch; i++)
            {
                if (doot_doot >= GameManager.instance.MaxNumberOfWildDataMons || SpawnPointsInRenderDist.Count == 0)
                {
                    break;
                }
                RandomSpawnPoint = SpawnPointsInRenderDist[Random.Range(0, SpawnPointsInRenderDist.Count)].position;
                isReferenced = MonsInChunk.TryGetValue(RandomSpawnPoint, out DataMonsInChunk value);
                if(!isReferenced)
                {
                    continue;
                }

                if (value.datamons >= MaxNumberOfDataMonInChunk)
                {
                    continue;
                }
                SpawnPosition = Random.insideUnitCircle.normalized * Random.Range(1, DataWorldSpawnPoints.SpawnPointsRadius);
                SpawnPosition += RandomSpawnPoint;

                //SpawnPosition = new Vector2(Mathf.Clamp(SpawnPosition.x, GameManager.instance.DataWorldBorderLeftX,
                //GameManager.instance.DataWorldBorderRightX),
                //Mathf.Clamp(SpawnPosition.y, GameManager.instance.DataWorldBorderDownY, GameManager.instance.DataWorldBorderUpY));
                if (Vector3.Distance(SpawnPosition, GameManager.instance.Player.transform.position) >
                    GameManager.instance.DataMonSpawnRadiusFromPlayer &&
                    Vector3.Distance(SpawnPosition, GameManager.instance.Player.transform.position) <
                    GameManager.instance.RenderDistance)
                {
                    value.datamons++;
                    SpawnNewDataMon(RandomSpawnPoint);
                }
                else
                {
                    i--;
                    //yield return new WaitForEndOfFrame();
                }

            }
            yield return new WaitForSeconds(delay);
            StopSpawning:
            if (ClearRoamingDataMons)
            {

            }
            yield return new WaitForSeconds(0.1f);

        }

    }
    public void AddDataMonToRoamingDatabase(Wave.WaveDifficulty difficulty)
    {
        RoamingDataBase.Clear();
        switch (difficulty)
        {
            case Wave.WaveDifficulty.Easy:
                RoamingDataBase = RoamingDataBase.Add(EasyDataMons);
                break;
            case Wave.WaveDifficulty.Normal:
                RoamingDataBase = RoamingDataBase.Add(NormalDataMons);
                break;
            case Wave.WaveDifficulty.Hard:
                RoamingDataBase = RoamingDataBase.Add(HardDataMons);
                break;
            case Wave.WaveDifficulty.Difficult:
                RoamingDataBase = RoamingDataBase.Add(DifficultDataMons);
                break;
            case Wave.WaveDifficulty.Hell:
                RoamingDataBase = RoamingDataBase.Add(HellDataMons);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        serializedDoot = doot_doot;
        
        //else
        //{
        //    SpawnPosition = prevSpawnPos;
        //    SpawnNewDataMon();

        //}


    }

    private void SpawnNewDataMon(Vector2 _RandomSpawnPoint)
    {
        DataMonsPool.TryGetValue(RoamingDataBase[Random.Range(0, RoamingDataBase.Count)].dataMon.DataMonName, out List<DataMon> temp);
        temp[0].transform.position = SpawnPosition;
        temp[0].gameObject.SetActive(true);
        temp[0].dataMonAI.SetNewPatrollingAnchorPos();
        temp[0].ResetAttributes();
        temp[0].SpawnedFromChunk = _RandomSpawnPoint;
        switch (temp[0].dataMonData.DataMons[temp[0].tier].MonBehaviourState)
        {
            case DataMonBehaviourState.isNeutral:
                temp[0].SetDataMonNeutral();
                break;
            case DataMonBehaviourState.isHostile:
                temp[0].SetDataMonHostile();
                break;
        }
        temp.RemoveAt(0);
        doot_doot += 1;
    }

    bool ValidateSpawnPoint(Vector3 spawnpointPos)
    {
        return false;
    }
}
public class DataMonsInChunk
{
    public int datamons = 0;
    public DataMonsInChunk(int value)
    {
        datamons = value;
    }
}
