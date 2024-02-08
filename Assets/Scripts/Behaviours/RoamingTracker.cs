using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingTracker : MonoBehaviour
{
    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameManager.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Player.transform.position) > GameManager.instance.RenderDistance)
        {
            RoamingSpawner.doot_doot--;
            Destroy(this.gameObject);
        }
    }
}
