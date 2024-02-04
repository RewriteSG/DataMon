using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject Player;
    [HideInInspector] public Rigidbody2D playerRb;
    public float PlayerDataMonPatrolMinDist;
    public float PlayerDataMonPatrolMaxDist;
    public float MaxDistForCompanionDataMon;
    public float CaptureDelay=1;
    public float DataMonsRotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (Player == null)
            return;
        playerRb = Player.GetComponent<Rigidbody2D>();
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
