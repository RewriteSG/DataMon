using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemyScript : MonoBehaviour
{
    public void OnDestroy()
    {
        WaveSpawner.enemyCount--;
    }
}
