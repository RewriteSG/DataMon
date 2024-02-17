using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndividualDataMon;
public class RoamingSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Datamons_roaming;
    [SerializeField]List<DataMon> Datamons_roamingData = new List<DataMon>();
    public static Dictionary<string, List<DataMon>> DataMonsPool = new Dictionary<string, List<DataMon>>();
    public static Dictionary<Vector2, DataMonsInChunk> MonsInChunk = new Dictionary<Vector2, DataMonsInChunk>();
    

    private Vector3 SpawnPosition;
    public float MaxNumberOfDataMonInChunk = 20;
    [SerializeField] int serializedDoot;
    public static int doot_doot;
    public static List<Transform> SpawnPointsInRenderDist = new List<Transform>();
    public static GameObject DataMonPoolGO;
    Vector3 RandomSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPointsInRenderDist.Clear();
        doot_doot = 0;
        for (int i = 0; i < Datamons_roaming.Length; i++)
        {
            Datamons_roamingData.Add(Datamons_roaming[i].GetComponent<DataMon>());
            Datamons_roamingData[Datamons_roamingData.Count - 1].SetDataMon(
                Datamons_roamingData[Datamons_roamingData.Count - 1].dataMonData.DataMons.
                GetDataMonInDataArray(Datamons_roamingData[Datamons_roamingData.Count - 1].gameObject.name));
        }
        List<DataMon> temp;

        DataMonPoolGO = new GameObject("DataMons");
        for (int i = 0; i < Datamons_roaming.Length; i++)
        {
            temp = new List<DataMon>();

            for (int x = 0; x < GameManager.instance.MaxNumberOfWildDataMons; x++)
            {
                temp.Add(Instantiate(Datamons_roaming[i], DataMonPoolGO.transform).GetComponent<DataMon>());
                temp[temp.Count - 1].gameObject.SetActive(false);
            }
            DataMonsPool.Add(Datamons_roamingData[i].dataMon.DataMonName, temp);
        }
        StartCoroutine(SpawnInBatches(5,10));
    }
    bool isReferenced;
    IEnumerator SpawnInBatches(float delay, int DataMonPerBatch)
    {
        while (isActiveAndEnabled)
        {
           
            doot_doot = Mathf.Clamp(doot_doot, 0, GameManager.instance.MaxNumberOfWildDataMons + 1);
            if (doot_doot >= GameManager.instance.MaxNumberOfWildDataMons || SpawnPointsInRenderDist.Count == 0)
            {
                goto StopSpawning;
            }

            //var wanted = Random.Range(MinY, MaxY);
            //var wanted2 = Random.Range(MinX, MaxX);
            //if (Vector2.Distance(RandomSpawnPoint,GameManager.instance.RenderDistance))
            for (int i = 0; i <= DataMonPerBatch; i++)
            {
                RandomSpawnPoint = SpawnPointsInRenderDist[Random.Range(0, SpawnPointsInRenderDist.Count)].position;
                isReferenced = MonsInChunk.TryGetValue(RandomSpawnPoint, out DataMonsInChunk value);
                if(!isReferenced)
                {
                    i--;
                    continue;
                }

                if (value.datamons >= MaxNumberOfDataMonInChunk)
                {
                    i--;
                    continue;
                }
                SpawnPosition = Random.insideUnitCircle.normalized * Random.Range(1, DataWorldSpawnPoints.SpawnPointsRadius);
                SpawnPosition += RandomSpawnPoint;
                if (Vector3.Distance(SpawnPosition, GameManager.instance.Player.transform.position) >
                    GameManager.instance.DataMonSpawnRadiusFromPlayer &&
                    Vector3.Distance(SpawnPosition, GameManager.instance.Player.transform.position) <
                    GameManager.instance.RenderDistance)
                {
                    SpawnNewDataMon();
                }
                else
                    i--;
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(delay);
            StopSpawning:
            yield return new WaitForEndOfFrame();

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

    private void SpawnNewDataMon()
    {
        DataMonsPool.TryGetValue(Datamons_roamingData[Random.Range(0, Datamons_roaming.Length)].dataMon.DataMonName, out List<DataMon> temp);
        temp[0].transform.position = SpawnPosition;
        temp[0].gameObject.SetActive(true);
        temp[0].dataMonAI.SetNewPatrollingAnchorPos();
        temp[0].ResetAttributes();
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
