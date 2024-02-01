using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject gunPoint;
    public GameObject DataBall;
    public float DestroyBallAtDelay =3;
    // Update is called once per frame
    //GameObject spawnedDataBall;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Destroy(Instantiate(DataBall, gunPoint.transform.position, gunPoint.transform.rotation),DestroyBallAtDelay);
        }
    }
}
