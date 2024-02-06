using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DataDex : MonoBehaviour
{
    public static DataDex instance;
    public Transform Content;
    public GameObject Label, DataMonContainer, DataMonInDataDex;
    public List<DataMonsData> AllDataMons = new List<DataMonsData>();
    public List<DataMonHolder> CompanionsDataMon = new List<DataMonHolder>();
    public List<DataMonHolder> DataMonTeam = new List<DataMonHolder>();
    List<GameObject> DataMonListInDex = new List<GameObject>();
    List<GameObject> DataMonObtained = new List<GameObject>();

    public delegate void DataMonBtnDelegate(DataMonButton dataMon);

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        int DataMonEvoCount;
        GameObject temp;
        for (int i = 0; i < AllDataMons.Count; i++)
        {
            DataMonEvoCount = AllDataMons[i]._DataMon.Length;
            for (int x = 0; x < DataMonEvoCount; x++)
            {
                temp = Instantiate(Label, Content);
                temp.GetComponentInChildren<TextMeshProUGUI>().text =/* "Datamon #"+x+" : " + */AllDataMons[i]._DataMon[x].DataMonName;
                DataMonListInDex.Add(Instantiate(DataMonContainer, Content));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int indexOfDataMon;
    VerticalLayoutGroup verticalLayout;
    DataMonButton dataMonBtn;
    internal void AddToDataDex(DataMonHolder toDataDex)
    {
        indexOfDataMon = AllDataMons.IndexOf(toDataDex.dataMonData);
        DataMonObtained.Add(Instantiate(DataMonInDataDex,DataMonListInDex[indexOfDataMon].transform));
        verticalLayout = DataMonListInDex[indexOfDataMon].transform.parent.GetComponent<VerticalLayoutGroup>();

        dataMonBtn = DataMonObtained[DataMonObtained.Count - 1].AddComponent<DataMonButton>();
        
        dataMonBtn.dataMonHolder = new DataMonHolder(toDataDex);
        dataMonBtn.AddToTeam += DataDexAddToTeam;

        StartCoroutine(RearrangeContent(verticalLayout));
    }
    IEnumerator RearrangeContent(VerticalLayoutGroup toRearrange)
    {
        toRearrange.padding.top = 1;
        yield return new WaitForEndOfFrame();

        toRearrange.padding.top = 0;

    }
    public void DataDexAddToTeam(DataMonButton dataMonButton)
    {

        Destroy(dataMonButton.gameObject);
    }
}
public class DataDexIO
{
    public DataMonHolder toDataDex;
   
    public void SendToDataDex()
    {
        DataDex.instance.CompanionsDataMon.Add(toDataDex);
        DataDex.instance.AddToDataDex(toDataDex);
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
    public DataMonHolder(DataMonHolder toHold)
    {
        dataMonData = toHold.dataMonData;
        dataMon = toHold.dataMon;
        dataMonAttributes = new DataMonInstancedAttributes(toHold.dataMonAttributes);
    }
}