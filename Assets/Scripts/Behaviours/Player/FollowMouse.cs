using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float rotateDamp;
    float reference;
    private void Start()
    {
        GameManager.instance.Entity_Updates += ToUpdate;

    }
    // Update is called once per frame
    void ToUpdate()
    {
        if (GameManager.instance.PlayerisDashing)
            return;
        Vector3 objPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        //mousePos.y = Camera.main.transform.position.y - transform.position.y;

        mousePos -= objPos;
        mousePos = mousePos.normalized;
        //print("mos" + mousePos);
        float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
        float RotateSmoothly = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, -angle, ref reference, rotateDamp);
        RotateSmoothly = Mathf.Clamp(RotateSmoothly, float.MinValue, float.MaxValue);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, RotateSmoothly));
    }
    private void OnDestroy()
    {
        GameManager.instance.Entity_Updates -= ToUpdate;

    }
    //Vector3 objPos = Camera.main.WorldToScreenPoint(transform.position);
    //Vector3 mousePos = Input.mousePosition;
    ////mousePos.y = Camera.main.transform.position.y - transform.position.y;

    //mousePos -= objPos;
    //    float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
    //float RotateSmoothly = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, angle, ref reference, rotateSpeed);
    //transform.rotation = Quaternion.Euler(new Vector3(0, RotateSmoothly, 0));
}
