using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject Player;

    public float PlayerDataMonPatrolMinDist;
    public float PlayerDataMonPatrolMaxDist;
    public float MaxDistForCompanionDataMon;
    public float DataMonsRotationSpeed;

    public int Databytes_Count;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Player.transform.position, PlayerDataMonPatrolMinDist);
        Gizmos.DrawWireSphere(Player.transform.position, PlayerDataMonPatrolMaxDist);

        Gizmos.DrawWireSphere(Player.transform.position, MaxDistForCompanionDataMon);
    }
   
}
