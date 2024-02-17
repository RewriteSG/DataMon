using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerRenderDist"))
        {
            RoamingSpawner.SpawnPointsInRenderDist.Add(transform);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerRenderDist"))
        {
            RoamingSpawner.SpawnPointsInRenderDist.Remove(transform);
        }
    }
}
