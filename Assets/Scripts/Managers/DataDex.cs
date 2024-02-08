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

    public List<DataMonsData> AllDataMons = new List<DataMonsData>();
    public List<DataMonHolder> CompanionsDataMon = new List<DataMonHolder>();
    public DataMonHolder[] DataTeam = new DataMonHolder[] { };

    List<GameObject> DataMonListInDex = new List<GameObject>();
    List<GameObject> DataMonObtained = new List<GameObject>();

    List<GameObject> DataTeamPanels = new List<GameObject>();

    public delegate void DataMonBtnDelegate(DataMonButton dataMon);

    public bool DisplayDataDex;
    public GameObject DataPad;
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
        if(DataTeamContainer.transform.childCount < DataTeam.Length)
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
        indexOfDataMon = AllDataMons.IndexOf(toDataDex.dataMonData);
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

            dataMonBtn = DataTeamPanels[indexOfDataMon].GetComponent<DataMonButton>();
            dataMonBtn.dataMonHolder = dataMonButton.dataMonHolder;
            dataMonBtn.DataMonSummoned = Instantiate(dataMonButton.dataMonHolder.dataMon.DataMonPrefab,
                GameManager.instance.Player.transform.position,Quaternion.identity);



            verticalLayout = dataMonButton.transform.parent.parent.GetComponent<VerticalLayoutGroup>();
            StartCoroutine(RearrangeContent(verticalLayout));

            Destroy(dataMonButton.gameObject);
        }
    }
    public void DataDexRemoveFromTeam(DataMonButton dataMonButton)
    {
        if (dataMonBtn.dataMonHolder.isNull())
            return;

        AddToDataDex(dataMonButton.dataMonHolder);

        indexOfDataMon = DataTeam.IndexOf(dataMonButton.dataMonHolder);
        DataTeam[indexOfDataMon] = null;
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
    public DataMonHolder(DataMon.IndividualDataMon.DataMon toHold)
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
public static class DataMonHolderExtension
{
    public static bool isNull(this DataMonHolder holder)
    {
        if(holder !=null)
            return holder.dataMon.DataMonPrefab == null;
        return holder == null;
    }
}
public static class ArrayExtensions
{
    public static T[] ResizeToArray<T>(this T[] array, int resizeTo)
    {
        T[] temp = new T[resizeTo];
        
        for (int i = 0; i < array.Length; i++)
        {
            temp[i] = array[i];
        }
        return temp;
    }
    public static int IndexOf<T>(this T[] array, T element)
    {
        int index = -1;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(element))
            {
                index = i;
                break;
            }
        }
        return index;
    }
}