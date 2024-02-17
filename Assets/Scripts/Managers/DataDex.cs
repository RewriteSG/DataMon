using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DataDex : MonoBehaviour
{
    public static DataDex instance;
    public Transform Content;
    public GameObject Label, DataMonContainer, DataMonPanel, DataTeamContainer;

    public List<DataMonsData> AllDataMonsData = new List<DataMonsData>();
    public List<string> AllDataMons = new List<string>();

    public List<DataMonHolder> CompanionsDataMon = new List<DataMonHolder>();
    public DataMonHolder[] DataTeam = new DataMonHolder[] { };

    List<GameObject> DataMonListInDex = new List<GameObject>();
    List<GameObject> DataMonObtained = new List<GameObject>();

    List<GameObject> DataTeamPanels = new List<GameObject>();

    public delegate void DataMonBtnDelegate(DataMonButton dataMon);

    public bool DisplayDataDex;
    public GameObject DataPad;
    public Color Testcolor;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        int DataMonEvoCount;
        GameObject temp;
        for (int i = 0; i < AllDataMonsData.Count; i++)
        {
            DataMonEvoCount = AllDataMonsData[i].DataMons.Length;
            for (int x = 0; x < DataMonEvoCount; x++)
            {
                temp = Instantiate(Label, Content);
                temp.GetComponentInChildren<TextMeshProUGUI>().text =/* "Datamon #"+x+" : " + */AllDataMonsData[i].DataMons[x].DataMonName;
                DataMonListInDex.Add(Instantiate(DataMonContainer, Content));
                AllDataMons.Add(AllDataMonsData[i].DataMons[x].DataMonName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DisplayDataDex = !DisplayDataDex;
        }
        if (DataTeam.Length != GameManager.instance.NumberOfDataMonsInTeam)
        {
            DataTeam = DataTeam.ResizeToArray(GameManager.instance.NumberOfDataMonsInTeam);
            
        }
        // Add DataMonTeamPanels
        if(DataTeamContainer.transform.childCount < DataTeam.Length)
        {
            for (int i = 0; i < DataTeam.Length-DataTeamContainer.transform.childCount; i++)
            {
                DataTeamPanels.Add(Instantiate(DataMonPanel, DataTeamContainer.transform));
                dataMonBtn = DataTeamPanels[DataTeamPanels.Count - 1].AddComponent<DataMonButton>();
                dataMonBtn.RemoveFromTeam += DataDexRemoveFromTeam;
                dataMonBtn.dataMonHolder = null;
                dataMonBtn.inDataBank = false;
            }
        }
        // Remove DataMonTeamPanels
        if(DataTeamContainer.transform.childCount > DataTeam.Length)
        {
            AddToDataDex(DataTeamPanels[DataTeamPanels.Count - 1].GetComponent<DataMonButton>().dataMonHolder);
            Destroy(DataTeamPanels[DataTeamPanels.Count - 1]);
            DataTeamPanels.RemoveAt(DataTeamPanels.Count - 1);

        }
        if(DataPad.activeInHierarchy!=DisplayDataDex)
            DataPad.SetActive(DisplayDataDex);
    }

    int indexOfDataMon;
    VerticalLayoutGroup verticalLayout;
    DataMonButton dataMonBtn;
    internal void AddToDataDex(DataMonHolder toDataDex)
    {
        if (toDataDex.isNull())
            return;
        indexOfDataMon = AllDataMons.IndexOf(toDataDex.dataMon.DataMonName);
        DataMonObtained.Add(Instantiate(DataMonPanel, DataMonListInDex[indexOfDataMon].transform));
        verticalLayout = DataMonListInDex[indexOfDataMon].transform.parent.GetComponent<VerticalLayoutGroup>();

        dataMonBtn = DataMonObtained[DataMonObtained.Count - 1].AddComponent<DataMonButton>();
        
        dataMonBtn.dataMonHolder = new DataMonHolder(toDataDex);
        dataMonBtn.AddToTeam += DataDexAddToTeam;
        dataMonBtn.inDataBank = true;
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
        
        indexOfDataMon = GetEmptySlotInTeam();
        if (indexOfDataMon != -1)
        {
            DataTeam[indexOfDataMon] = dataMonButton.dataMonHolder;
            DataTeamPanels[indexOfDataMon].name = dataMonButton.dataMonHolder.dataMon.DataMonName;
            DataTeamPanels[indexOfDataMon].GetComponent<Image>().color = Testcolor;
            dataMonBtn = DataTeamPanels[indexOfDataMon].GetComponent<DataMonButton>();
            dataMonBtn.dataMonHolder = dataMonButton.dataMonHolder;
            dataMonBtn.DataMonSummoned = 
            Instantiate(dataMonButton.dataMonHolder.dataMonData.DataMons.GetDataMonInDataArray(dataMonButton.dataMonHolder.dataMon.DataMonName).DataMonPrefab,
                GameManager.instance.Player.transform.position,Quaternion.identity);
            dataMonBtn.DataMonSummoned.GetComponent<IndividualDataMon.DataMon>().SetDataMonCompanion();



            verticalLayout = dataMonButton.transform.parent.parent.GetComponent<VerticalLayoutGroup>();
            StartCoroutine(RearrangeContent(verticalLayout));

            Destroy(dataMonButton.gameObject);
        }
    }
    public void DataDexRemoveFromTeam(DataMonButton dataMonButton)
    {
        if (dataMonButton.dataMonHolder.isNull())
        {
            return;
        }

        AddToDataDex(dataMonButton.dataMonHolder);

        indexOfDataMon = DataTeam.IndexOf(dataMonButton.dataMonHolder);
        DataTeam = DataTeam.RemoveAt(indexOfDataMon);
        DataTeamPanels.RemoveAt(indexOfDataMon);

        Destroy(dataMonButton.DataMonSummoned);
        Destroy(dataMonButton.gameObject);

        


    }
    public int GetEmptySlotInTeam()
    {
        int temp = -1;
        for (int i = 0; i < DataTeam.Length; i++)
        {
            if(DataTeam[i].isNull())
            {
                temp = i;
                break;
            }

        }
        return temp;
    }
}
public class DataDexIO
{
    public DataMonHolder toDataDex;
   
    public void SendToDataDex()
    {
        toDataDex.dataMon.MonBehaviourState = DataMonBehaviourState.isCompanion;
        DataDex.instance.CompanionsDataMon.Add(toDataDex);
        DataDex.instance.AddToDataDex(toDataDex);
    }
}
[System.Serializable]
public class DataMonHolder
{
    public DataMonsData dataMonData;
    public DataMonIndividualData dataMon;
    public DataMonInstancedAttributes dataMonCurrentAttributes;
    public DataMonHolder() { }
    public DataMonHolder(IndividualDataMon.DataMon toHold)
    {
        dataMonData = toHold.dataMonData;
        dataMon = toHold.dataMon;
        dataMonCurrentAttributes = new DataMonInstancedAttributes(toHold.dataMonCurrentAttributes);
    }
    public DataMonHolder(DataMonHolder toHold)
    {
        dataMonData = toHold.dataMonData;
        dataMon = toHold.dataMon;
        dataMonCurrentAttributes = new DataMonInstancedAttributes(toHold.dataMonCurrentAttributes);
    }
}
