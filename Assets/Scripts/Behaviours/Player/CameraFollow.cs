using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    public Transform toFollow;
    public float Damp;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!toFollow.isNull())
        {
            NewCamPos();
            //transform.position = Vector3.Lerp(transform.position, new Vector3(toFollow.position.x, toFollow.position.y, transform.position.z), 0.1f);
        }
    }
    private Vector3 smoothVelocity = Vector3.zero;
    Vector3 targetPos;
    Vector3 newCamPos;
    Vector3 offset;

    private void NewCamPos()
    {
        offset = Vector3.forward * transform.position.z;
        targetPos = toFollow.position + offset;
        newCamPos = Vector3.SmoothDamp(transform.position, targetPos, ref smoothVelocity, Damp);

        //vector3NewCamPos = new Vector3(newCamPos.x, newCamPos.y, transform.position.z);
        transform.position = newCamPos;
    }
}
