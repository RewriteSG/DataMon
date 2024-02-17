using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    public Transform toFollow;
    public float Damp;
    public static bool isAiming;
    Vector3 CenterOfScreen;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isAiming = false;
        CenterOfScreen = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
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
        if (isAiming)
        {
#if UNITY_EDITOR
            CenterOfScreen = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
#endif
            Vector3 objPos = CenterOfScreen;
            Vector3 mousePos = Input.mousePosition;
            //mousePos.y = Camera.main.transform.position.y - transform.position.y;
            float dist = Mathf.Clamp(Vector3.Distance(objPos, mousePos),0, 500f);
           
            Vector3 dir =  mousePos - objPos;
            dir = dir.normalized;
            Vector3 newMousePos = dir * dist;
            newMousePos += GameManager.instance.Player.transform.position;
            newMousePos += offset;
            newCamPos = Vector3.SmoothDamp(transform.position, 
                Vector3.Lerp(new Vector3(GameManager.instance.Player.transform.position.x, 
                GameManager.instance.Player.transform.position.y, transform.position.z)
                , newMousePos, 0.01f), ref smoothVelocity, Damp);

        }
        //vector3NewCamPos = new Vector3(newCamPos.x, newCamPos.y, transform.position.z);
        transform.position = newCamPos;
    }
}
