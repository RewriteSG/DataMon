using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject gunPoint;
    public GameObject DataBall;
    public float _DestroyBallAtDelay =3;
    public static float DestroyBallAtDelay;
    // Update is called once per frame
    //GameObject spawnedDataBall;
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DestroyBallAtDelay = _DestroyBallAtDelay;
            Instantiate(DataBall, gunPoint.transform.position, gunPoint.transform.rotation);
        }
    }
}
