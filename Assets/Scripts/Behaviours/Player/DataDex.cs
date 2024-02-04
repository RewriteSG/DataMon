using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDex : MonoBehaviour
{
    public static DataDex instance;
    public List<DataMonsData> AllDataMons = new List<DataMonsData>();
    public List<DataMonHolder> CompanionsDataMon = new List<DataMonHolder>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class DataDexIO
{
    public DataMonHolder toDataDex;
   
    public void SendToDataDex()
    {
        DataDex.instance.CompanionsDataMon.Add(toDataDex);
    }
}
[System.Serializable]
public class DataMonHolder
{
    public DataMonsData dataMonData;
    public DataMonIndividualData dataMon;
    public DataMonInstancedAttributes dataMonAttributes;
    public DataMonHolder() { }
    public DataMonHolder(DataMon.IndividualDataMon.DataMon toHold)
    {
        dataMonData = toHold.dataMonData;
        dataMon = toHold.dataMon;
        dataMonAttributes = new DataMonInstancedAttributes(toHold.dataMonCurrentAttributes);
    }
}