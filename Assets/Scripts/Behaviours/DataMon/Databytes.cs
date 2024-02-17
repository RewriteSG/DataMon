using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Databytes : MonoBehaviour
{
    IndividualDataMon.DataMon dataMon;
    [HideInInspector] public bool isQuitting;

    // Start is called before the first frame update
    void Start()
    {
        dataMon = GetComponent<IndividualDataMon.DataMon>();
        dataMon._databytes = this;
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
    public void DataMonIsDestroyed()
    {
        if (dataMon == null || isQuitting)
            return;
        if (!dataMon.isBeingCaptured && dataMon.dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)
            Instantiate(GameManager.instance.Data_bytes, transform.position, Quaternion.identity);
    }
}
