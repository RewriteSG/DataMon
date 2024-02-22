using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHubCam : MonoBehaviour
{
    //public void MoveToEvolutionPosition(Camera pov)
    //{
    //}

    public delegate void DataHubCamUpdate(Camera pov);

    public DataHubCamUpdate CamUpdate;
    Vector3 OriginalPosition;
    float originalSize;
    private void Start()
    {
        thisCam = GetComponent<Camera>();
        OriginalPosition = transform.position;
        originalSize = thisCam.orthographicSize;
    }
    Camera thisCam;
    Camera toCam;
    private void Update()
    {
        if (CamUpdate != null)
            CamUpdate(toCam);

    }


    public float damp;
    Vector3 referenceVelocity = Vector3.zero, newCamPos;

    float referenceFloat;
    Quaternion /*referenceRotation = Quaternion.identity,*/ newRotation;
    public void MoveToPosition(Camera pov)
    {
        toCam = pov;
        CamUpdate = UpdateCamPos;
        referenceFloat = 0;
        referenceVelocity = Vector3.zero;
    }
    void UpdateCamPos(Camera pov)
    {

        newCamPos = Vector3.SmoothDamp(transform.position, pov.transform.position, ref referenceVelocity, damp);
        thisCam.orthographicSize = Mathf.SmoothDamp(thisCam.orthographicSize, pov.orthographicSize, ref referenceFloat, damp);

        newRotation = Quaternion.Slerp(transform.rotation, pov.transform.rotation, Time.deltaTime);

        transform.SetPositionAndRotation(newCamPos, newRotation);

        if(Vector3.Distance(transform.position, pov.transform.position) < 0.01f)
        {
            CamUpdate -= UpdateCamPos;
        }

    }
}
