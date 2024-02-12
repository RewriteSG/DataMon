using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Datamons_roaming;

    

    private Vector3 SpawnPosition;

    public static int doot_doot;
    public static List<Transform> SpawnPointsInRenderDist = new List<Transform>();
    Vector3 RandomSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPointsInRenderDist.Clear();
        doot_doot = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        if (doot_doot >= GameManager.instance.MaxNumberOfWildDataMons || SpawnPointsInRenderDist.Count ==0)
            return;

        //var wanted = Random.Range(MinY, MaxY);
        //var wanted2 = Random.Range(MinX, MaxX);

        RandomSpawnPoint = SpawnPointsInRenderDist[Random.Range(0, SpawnPointsInRenderDist.Count)].position;
        //if (Vector2.Distance(RandomSpawnPoint,GameManager.instance.RenderDistance))
        

        SpawnPosition = Random.insideUnitCircle.normalized * Random.Range(GameManager.instance.DataMonSpawnRadiusFromPlayerInner, GameManager.instance.DataMonSpawnRadiusFromPlayerOuter);
        SpawnPosition += RandomSpawnPoint;
        Instantiate(Datamons_roaming[Random.Range(0, Datamons_roaming.Length)], SpawnPosition, Quaternion.identity);
        doot_doot += 1;

    }
    bool ValidateSpawnPoint(Vector3 spawnpointPos)
    {
        return false;
    }
}
