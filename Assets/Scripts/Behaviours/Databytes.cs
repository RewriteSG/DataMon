using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Databytes : MonoBehaviour
{
    public GameObject Data_bytes;
    DataMon.IndividualDataMon.DataMon dataMon;
    bool isQuitting;

    // Start is called before the first frame update
    void Start()
    {
        dataMon = GetComponent<DataMon.IndividualDataMon.DataMon>();
        isQuitting = false;
    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        if(Input.GetMouseButtonDown(0))
    //        {

    //            Destroy(this.gameObject);

    //        }
    //    }
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    private void OnDestroy()
    {
        if (dataMon == null || isQuitting)
            return;
        if (!dataMon.isBeingCaptured)
            Instantiate(Data_bytes, transform.position, Quaternion.identity);
    }
}
