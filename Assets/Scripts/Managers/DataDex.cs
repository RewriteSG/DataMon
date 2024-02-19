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

    public delegate void DataMonBtnDelegate(DataMonButton dataMon);

    public DataMonBtnDelegate RightClick;

    public DataMonBtnDelegate ClickAndDrag;

    public bool DisplayDataPad;
    public GameObject DataPad, DataTeamPanel, EvolvePanel, CraftPanel;
    public Color Testcolor;
    public DataPadModules CurrentModule;
    List<GameObject> DataMonListInDex = new List<GameObject>();
    public List<GameObject> DataMonObtained = new List<GameObject>();

    List<GameObject> DataTeamPanels = new List<GameObject>();
    [HideInInspector] public DatamonEvolution datamonEvolution = null;
    public DataMonButton DataMonOnTeamIsSelectedForEvolve = null;

    DataMonButton dataMonBtn;

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
        RightClick += DataMonButtonRightClick;

        CurrentModule = DataPadModules.DataDex;
        datamonEvolution = GetComponent<DatamonEvolution>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DisplayDataPad = !DisplayDataPad;
            StartRearrangeDataBank();
        }
        switch (CurrentModule)
        {
            case DataPadModules.DataDex:
                DataTeamPanel.SetActive(true);
                EvolvePanel.SetActive(false);
                CraftPanel.SetActive(false);

                break;

            case DataPadModules.Evolve:
                DataTeamPanel.SetActive(true);
                EvolvePanel.SetActive(true);
                CraftPanel.SetActive(false);
                break;

            case DataPadModules.Craft:
                DataTeamPanel.SetActive(false);
                EvolvePanel.SetActive(false);
                CraftPanel.SetActive(true);
                break;

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
                AddTeamDataMonPanel();
            }
        }
        // Remove DataMonTeamPanels
        if(DataTeamContainer.transform.childCount > DataTeam.Length)
        {
            AddToDataDex(DataTeamPanels[DataTeamPanels.Count - 1].GetComponent<DataMonButton>().dataMonHolder);
            Destroy(DataTeamPanels[DataTeamPanels.Count - 1]);
            DataTeamPanels.RemoveAt(DataTeamPanels.Count - 1);

        }
        if(DataPad.activeInHierarchy!=DisplayDataPad)
            DataPad.SetActive(DisplayDataPad);
    }

    private void AddTeamDataMonPanel()
    {
        DataTeamPanels.Add(Instantiate(DataMonPanel, DataTeamContainer.transform));
        dataMonBtn = DataTeamPanels[DataTeamPanels.Count - 1].AddComponent<DataMonButton>();
        dataMonBtn.dataMonHolder = null;
        dataMonBtn.inDataBank = false;
    }

    int indexOfDataMon;
    VerticalLayoutGroup verticalLayout;

    /// <summary>
    /// Add DataMon To DataDex, Basically Sending the datamon to the Bank
    /// </summary>
    /// <param name="toRemove"></param>
    public void AddToDataDex(DataMonHolder toDataDex)
    {
        if (toDataDex.isNull())
            return;
        indexOfDataMon = AllDataMons.IndexOf(toDataDex.dataMon.DataMonName);
        DataMonObtained.Add(Instantiate(DataMonPanel, DataMonListInDex[indexOfDataMon].transform));
        verticalLayout = DataMonListInDex[indexOfDataMon].transform.parent.GetComponent<VerticalLayoutGroup>();

        dataMonBtn = DataMonObtained[DataMonObtained.Count - 1].AddComponent<DataMonButton>();
        
        dataMonBtn.dataMonHolder = new DataMonHolder(toDataDex);
        dataMonBtn.inDataBank = true;
        StartCoroutine(RearrangeContent(verticalLayout));
    }
    public void EvolveDataMon(DataMonHolder EvolvedDataMon)
    {

        if (DataMonOnTeamIsSelectedForEvolve == null)
            return;


        indexOfDataMon = DataTeam.IndexOf(DataMonOnTeamIsSelectedForEvolve.dataMonHolder);
        DataTeam = DataTeam.RemoveAt(indexOfDataMon);
        DataTeamPanels.RemoveAt(indexOfDataMon);
        RemoveFromDataDex(DataMonOnTeamIsSelectedForEvolve);
        DataMonOnTeamIsSelectedForEvolve = null;
        AddTeamDataMonPanel();

        AddToDataDex(EvolvedDataMon);

        DataDexAddToTeam(dataMonBtn);
        
    }
    bool rearrangingContent = false;
    IEnumerator RearrangeContent(VerticalLayoutGroup toRearrange)
    {
        toRearrange.padding.top = 1;
        if (rearrangingContent)
            yield break;
        rearrangingContent = true;
        yield return new WaitForEndOfFrame();
        toRearrange.padding.top = 0;
        rearrangingContent = false;

    }
    public void StartRearrangeDataBank()
    {
        VerticalLayoutGroup[] allLayoutGroup = Content.transform.GetComponentsInChildren<VerticalLayoutGroup>();
        for (int i = 0; i < allLayoutGroup.Length; i++)
        {
            StartCoroutine(RearrangeContent(allLayoutGroup[i]));

        }
    }
    /// <summary>
    /// Removes DataMon From DataDex, Basically Sending the datamon to the abyss
    /// </summary>
    /// <param name="toRemove"></param>
    public void RemoveFromDataDex(DataMonButton toRemove)
    {
        if (toRemove.isNull())
            return;

        Destroy(toRemove.gameObject);

    }
    void DataMonButtonRightClick(DataMonButton dataMonButton)
    {
        switch (CurrentModule)
        {
            case DataPadModules.DataDex:
                if (dataMonButton.inDataBank)
                {

                    DataDexAddToTeam(dataMonButton);
                    
                }
                else
                {
                    DataDexRemoveFromTeam(dataMonButton);
                }
                break;
            case DataPadModules.Evolve:
                bool isDataMonSelected = datamonEvolution.SelectDataMon(dataMonButton.dataMonHolder);
                if (!isDataMonSelected)
                    return;
                DataMonOnTeamIsSelectedForEvolve = null;
                if (dataMonButton.inDataBank)
                {
                    Destroy(dataMonButton.gameObject);
                    
                }
                if (!dataMonButton.inDataBank)
                {
                    dataMonButton.image.color = Color.green;
                    DataMonOnTeamIsSelectedForEvolve = dataMonButton;
                }
                
                break;

        }
    }

    void DataDexAddToTeam(DataMonButton dataMonButton)
    {
        
        indexOfDataMon = GetEmptySlotInTeam();
        if (indexOfDataMon != -1)
        {
            DataTeam[indexOfDataMon] = dataMonButton.dataMonHolder;
            DataTeamPanels[indexOfDataMon].name = dataMonButton.dataMonHolder.dataMon.DataMonName;
            DataTeamPanels[indexOfDataMon].GetComponent<Image>().color = Testcolor;
            dataMonBtn = DataTeamPanels[indexOfDataMon].GetComponent<DataMonButton>();
            dataMonBtn.dataMonHolder = dataMonButton.dataMonHolder;
            dataMonBtn.DataMonSummoned = SpawnCompanionDataMon(dataMonBtn);
            dataMonBtn.dataMon = dataMonBtn.DataMonSummoned.GetComponent<IndividualDataMon.DataMon>();

            verticalLayout = dataMonButton.transform.parent.parent.GetComponent<VerticalLayoutGroup>();
            StartCoroutine(RearrangeContent(verticalLayout));

            RemoveFromDataDex(dataMonButton);
        }
    }

    public GameObject SpawnCompanionDataMon(DataMonButton dataMonButton)
    {
        GameObject summoned = Instantiate(dataMonButton.dataMonHolder.dataMonData.DataMons.
            GetDataMonInDataArray(dataMonButton.dataMonHolder.dataMon.DataMonName).DataMonPrefab,
                        GameManager.instance.Player.transform.position, Quaternion.identity);

        IndividualDataMon.DataMon dataMon = summoned.GetComponent<IndividualDataMon.DataMon>();
        dataMon.SetDataMonCompanion();
        dataMon.SetAttributes(dataMonButton.dataMonHolder.dataMonCurrentAttributes);

        return summoned;
    }

    void DataDexRemoveFromTeam(DataMonButton dataMonButton)
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
    int GetEmptySlotInTeam()
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
    public void SetDataPadToDataDex()
    {
        CurrentModule = DataPadModules.DataDex;
    }
    public void SetDataPadToEvolve()
    {
        CurrentModule = DataPadModules.Evolve;
    }
    public void SetDataPadToCraft()
    {
        CurrentModule = DataPadModules.Craft;
    }
    private void OnDestroy()
    {
        RightClick -= DataMonButtonRightClick;

    }
    public DataMonButton GetDataMonFromTeam(DataMonsData dataMonsData, int evolution)
    {
        DataMonButton temp = null;
        List<DataMonButton> tempList = new List<DataMonButton>();
        for (int i = 0; i < DataTeam.Length; i++)
        {
            if (DataTeam[i] == null)
                continue;
#if UNITY_EDITOR
            if (DataTeam[i].dataMonData == null)
                continue;
#endif 
            if (DataTeam[i].dataMonData != dataMonsData)
                continue;
            if (DataTeam[i].dataMonData.DataMons.GetDataMonIndexInDataArray(DataTeam[i].dataMon.DataMonName) < evolution)
                continue;
            temp = DataTeamPanels[i].GetComponent<DataMonButton>();
            tempList.Add(temp);

        }
        bool isAvailable = false;
        for (int i = 0; i < tempList.Count; i++)
        {
            if(tempList[i].dataMon.dataMonAI.AI_state != AI_State.Produce)
            {
                temp = tempList[i];
                isAvailable = true;
                break;
            }
        }
        if (!isAvailable && tempList.Count>0)
            temp = tempList[0];
        return temp;
    }
}

public enum DataPadModules
{
    DataDex, Evolve, Craft
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
