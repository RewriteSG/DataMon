using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float rotateDamp;
    float reference;
    // Update is called once per frame
    Vector3 Dir;
    Vector3 mousePos;
    void Update()
    {
        Vector3 objPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        //mousePos.y = Camera.main.transform.position.y - transform.position.y;

        mousePos -= objPos;
        mousePos = mousePos.normalized;
        print("mos" + mousePos);
        float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
        float RotateSmoothly = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, -angle, ref reference, rotateDamp);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, RotateSmoothly));
    }
    //Vector3 objPos = Camera.main.WorldToScreenPoint(transform.position);
    //Vector3 mousePos = Input.mousePosition;
    ////mousePos.y = Camera.main.transform.position.y - transform.position.y;

    //mousePos -= objPos;
    //    float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
    //float RotateSmoothly = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle, ref reference, rotateSpeed);
    //transform.rotation = Quaternion.Euler(new Vector3(0, RotateSmoothly, 0));
}
