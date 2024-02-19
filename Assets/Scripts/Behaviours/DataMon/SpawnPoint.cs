using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    bool isReferenced;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("DataMon"))
        //{
        //    isReferenced = RoamingSpawner.MonsInChunk.TryGetValue(transform.position, out DataMonsInChunk value);
        //    if (!isReferenced)
        //        return;
        //}
        if (collision.gameObject.CompareTag("PlayerRenderDist"))
        {
            RoamingSpawner.SpawnPointsInRenderDist.Add(transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("DataMon"))
        //{
        //    isReferenced = RoamingSpawner.MonsInChunk.TryGetValue(transform.position, out DataMonsInChunk value);
        //    if (!isReferenced)
        //        return;
        //    value.datamons--;

        //}
        if (collision.gameObject.CompareTag("PlayerRenderDist"))
        {
            RoamingSpawner.SpawnPointsInRenderDist.Remove(transform);
            //print(RoamingSpawner.SpawnPointsInRenderDist.Count);

        }
    }
}
