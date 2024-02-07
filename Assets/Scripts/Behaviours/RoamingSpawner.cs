using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Datamons_roaming;

    [SerializeField] float MaxX;
    [SerializeField] float MinX;
    
    [SerializeField] float MinY;
    [SerializeField] float MaxY;

    private Vector3 hazWANHole;

    private int doot_doot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doot_doot<=20)
        {
            var wanted = Random.Range(MinY, MaxY);
            var wanted2 = Random.Range(MinX, MaxX);

            hazWANHole = new Vector3(wanted2, wanted, 0);
            Instantiate(Datamons_roaming[Random.Range(0, Datamons_roaming.Length)], hazWANHole, Quaternion.identity);
            doot_doot += 1;
        }
    }
}
