// DataDex
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class DataDex : MonoBehaviour
{
    [Header("Important: Set This To False if your exploring")]
    public bool isExploring;
    public static DataDex instance;
    public Transform Content;
    public GameObject Label, DataMonContainer, DataMonPanel, DataTeamContainer;

    public List<DataMonsData> AllDataMonsData = new List<DataMonsData>();
    List<string> AllDataMons = new List<string>();

    DataMonHolder[] DataMonsInBank;

    [Header("To DataHub")]
    public List<DataMonHolder> ToDataHub = new List<DataMonHolder>();

    public DataMonHolder[] DataTeam = new DataMonHolder[] { };

    public delegate void DataMonBtnDelegate(DataMonButton dataMon);

    public DataMonBtnDelegate RightClick;

    public DataMonBtnDelegate ClickAndDrag;

    public bool DisplayDataPad;
    public GameObject DataTeamPanel, EvolvePanel, CraftPanel, ExpeditionPanel;
    public Color Testcolor;
    public DataPadModules CurrentModule;
    List<GameObject> DataMonListInDex = new List<GameObject>();
    public List<GameObject> DataMonObtained = new List<GameObject>();

    List<GameObject> DataTeamPanels = new List<GameObject>();
    [HideInInspector] public DatamonEvolution datamonEvolution = null;
    public DataMonButton DataMonOnTeamIsSelectedForEvolve = null;

    DataMonButton dataMonBtn;

    int indexOfDataMon;
    VerticalLayoutGroup verticalLayout;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        int DataMonEvoCount;
        GameObject temp;
        print("how");
        if (SaveLoadManager.instance.DoLoadWorld)
            DataMonsInBank = SaveLoadManager.LoadDataMonsInBank();
        else
            DataMonsInBank = new DataMonHolder[] { };
        for (int i = 0; i < DataMonsInBank.Length; i++)
        {
            ToDataHub.Add(DataMonsInBank[i]);
        }
        if (isExploring)
        {
            if (SaveLoadManager.instance.DoLoadWorld)
                DataTeam = SaveLoadManager.LoadDataTeamFromSave();
            return;
        }
        DataMonListInDex.Clear();
        AllDataMons.Clear();
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


        for (int i = 0; i < ToDataHub.Count; i++)
        {
            AddToDataDex(ToDataHub[i]);
        }
        if (DataTeam.Length != GameManager.instance.NumberOfDataMonsInTeam)
        {
            DataTeam = DataTeam.ResizeToArray(GameManager.instance.NumberOfDataMonsInTeam);

        }
        // Add DataMonTeamPanels
        if (DataTeamContainer.transform.childCount < DataTeam.Length)
        {
            int diff = DataTeam.Length - DataTeamContainer.transform.childCount;
            for (int i = 0; i < diff; i++)
            {
                AddTeamDataMonPanel();
            }
        }
        // Remove DataMonTeamPanels
        while (DataTeamContainer.transform.childCount > DataTeam.Length)
        {
            AddToDataDex(DataTeamPanels[DataTeamPanels.Count - 1].GetComponent<DataMonButton>().dataMonHolder);
            Destroy(DataTeamPanels[DataTeamPanels.Count - 1]);
            DataTeamPanels.RemoveAt(DataTeamPanels.Count - 1);
        }

        if (SaveLoadManager.instance.DoLoadWorld)
        {
            DataMonHolder[] loadedTeam = SaveLoadManager.LoadDataTeamFromSave();

            DataMonButton tempButton;
            for (int i = 0; i < loadedTeam.Length; i++)
            {
                tempButton = AddToDataDex(loadedTeam[i]);
                DataDexAddToTeam(tempButton);
            }
        }

        RightClick += DataMonButtonRightClick;

        //CurrentModule = DataPadModules.DataDex;

        datamonEvolution = GetComponent<DatamonEvolution>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isExploring)
            return;
        switch (CurrentModule)
        {
            case DataPadModules.DataDex:
                DataTeamPanel.SetActive(true);
                EvolvePanel.SetActive(false);
                CraftPanel.SetActive(false);
                ExpeditionPanel.SetActive(false);
                break;

            case DataPadModules.Evolve:
                DataTeamPanel.SetActive(true);
                EvolvePanel.SetActive(true);
                CraftPanel.SetActive(false);
                ExpeditionPanel.SetActive(false);
                break;

            case DataPadModules.Craft:
                DataTeamPanel.SetActive(false);
                EvolvePanel.SetActive(false);
                CraftPanel.SetActive(true);
                ExpeditionPanel.SetActive(false);
                break;
            case DataPadModules.ConfirmToExpedition:
                DataTeamPanel.SetActive(false);
                EvolvePanel.SetActive(false);
                CraftPanel.SetActive(false);
                ExpeditionPanel.SetActive(true);
                break;
            case DataPadModules.None:
                DataTeamPanel.SetActive(false);
                EvolvePanel.SetActive(false);
                CraftPanel.SetActive(false);
                ExpeditionPanel.SetActive(false);
                break;


        }
        if (DataTeam.Length != GameManager.instance.NumberOfDataMonsInTeam)
        {
            DataTeam = DataTeam.ResizeToArray(GameManager.instance.NumberOfDataMonsInTeam);

        }
        // Add DataMonTeamPanels
        if (DataTeamContainer.transform.childCount < DataTeam.Length)
        {
            int diff = DataTeam.Length - DataTeamContainer.transform.childCount;
            for (int i = 0; i < diff; i++) 
            { 
                AddTeamDataMonPanel();
            }
        }
        // Remove DataMonTeamPanels
        if (DataTeamContainer.transform.childCount > DataTeam.Length)
        {
            AddToDataDex(DataTeamPanels[DataTeamPanels.Count - 1].GetComponent<DataMonButton>().dataMonHolder);
            Destroy(DataTeamPanels[DataTeamPanels.Count - 1]);
            DataTeamPanels.RemoveAt(DataTeamPanels.Count - 1);
        }
    }

    private void AddTeamDataMonPanel()
    {
        DataTeamPanels.Add(Instantiate(DataMonPanel, DataTeamContainer.transform));
        dataMonBtn = DataTeamPanels[DataTeamPanels.Count - 1].AddComponent<DataMonButton>();
        dataMonBtn.transform.GetComponentInChildren<SpriteSet>().gameObject.GetComponent<Image>().sprite = null;
        dataMonBtn.dataMonHolder = null;
        dataMonBtn.inDataBank = false;
        //dataMonBtn.dataMonHolder.inBank = false; 
    }


    /// <summary>
    /// Add DataMon To DataDex, Basically Sending the datamon to the Bank
    /// </summary>
    /// <param name="toRemove"></param>
    public DataMonButton AddToDataDex(DataMonHolder toDataDex)
    {

        if (isExploring)
        {
            AddToDataDexInExploring(toDataDex);
            return null;
        }
        if (toDataDex.isNull())
            return null;

        indexOfDataMon = AllDataMons.IndexOf(toDataDex.dataMon.DataMonName);
        DataMonObtained.Add(Instantiate(DataMonPanel, DataMonListInDex[indexOfDataMon].transform));
        verticalLayout = DataMonListInDex[indexOfDataMon].transform.parent.GetComponent<VerticalLayoutGroup>();

        dataMonBtn = DataMonObtained[DataMonObtained.Count - 1].AddComponent<DataMonButton>();
        dataMonBtn.transform.GetComponentInChildren<SpriteSet>().GetComponent<Image>().sprite = toDataDex.dataMon.UIsprite;
        dataMonBtn.transform.GetComponentInChildren<SpriteSet>().GetComponent<Image>().color = Color.white;
        dataMonBtn.dataMonHolder = new DataMonHolder(toDataDex);

        dataMonBtn.dataMonHolder.dataMon.DataMonPrefab = 
            dataMonBtn.dataMonHolder.dataMonData.
            DataMons.GetDataMonInDataArray(dataMonBtn.dataMonHolder.dataMon.DataMonName).DataMonPrefab;

        //print(dataMonBtn.dataMonHolder.dataMon.DataMonPrefab + "my Prafab");

        //print(dataMonBtn.dataMonHolder.dataMonData.
        //    DataMons.GetDataMonInDataArray(dataMonBtn.dataMonHolder.dataMon.DataMonName).DataMonPrefab.name + "Prefab");

        dataMonBtn.dataMonHolder.inBank = true;
        dataMonBtn.inDataBank = true;
        StartCoroutine(RearrangeContent(verticalLayout));

        return dataMonBtn;
    }
    public void AddToDataDexInExploring(DataMonHolder toDataDex)
    {
        ToDataHub.Add(toDataDex);
    }
    public void EvolveDataMon(DataMonHolder EvolvedDataMon)
    {
        bool isOnTeam = DataMonOnTeamIsSelectedForEvolve != null;

        if (DataMonOnTeamIsSelectedForEvolve != null)
        {
            indexOfDataMon = DataTeam.IndexOf(DataMonOnTeamIsSelectedForEvolve.dataMonHolder);
            DataTeam = DataTeam.RemoveAt(indexOfDataMon);
            DataTeamPanels.RemoveAt(indexOfDataMon);
            RemoveFromDataDex(DataMonOnTeamIsSelectedForEvolve);
            DataMonOnTeamIsSelectedForEvolve = null;
            AddTeamDataMonPanel();
        }

        AddToDataDex(EvolvedDataMon);

        if (isOnTeam)
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

        DataMonObtained.Remove(toRemove.gameObject);
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
                if (!isDataMonSelected && datamonEvolution.DataMonToEvolve == null)
                    return;
                else if (!isDataMonSelected && datamonEvolution.SelectSacrifice(dataMonButton.dataMonHolder))
                {
                    if (dataMonButton.inDataBank)
                    {
                        Destroy(dataMonButton.gameObject);

                    }
                    if (!dataMonButton.inDataBank)
                    {
                        dataMonButton.image.color = Color.blue;
                    }
                }
                if(!isDataMonSelected)
                    return;
                //if (datamonEvolution.DataMonToEvolve != null)
                //    return;
                print("oh hey");
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
            dataMonBtn.dataMonHolder = DataTeam[indexOfDataMon];
            dataMonBtn.dataMonHolder.inBank = false;
            //dataMonBtn.DataMonSummoned = SpawnCompanionDataMon(dataMonBtn);
            //dataMonBtn.dataMon = dataMonBtn.DataMonSummoned.GetComponent<IndividualDataMon.DataMon>();
            //dataMonBtn.dataMon.StartPassive();
            dataMonBtn.dataMonHolder.dataMonBaseAttributes = dataMonButton.dataMonHolder.dataMonBaseAttributes;
            verticalLayout = dataMonButton.transform.parent.parent.GetComponent<VerticalLayoutGroup>();

            dataMonBtn.transform.GetComponentInChildren<SpriteSet>().GetComponent<Image>().sprite = dataMonBtn.dataMonHolder.dataMon.UIsprite;
            dataMonBtn.transform.GetComponentInChildren<SpriteSet>().GetComponent<Image>().color = Color.white;
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
        dataMon.SetAttributes(dataMonButton.dataMonHolder.dataMonCurrentAttributes);
        dataMon.SetDataMonCompanion();
        dataMon.SetDataMonsAttacks(dataMonButton.dataMonHolder.dataMonAttacks);

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

        //DataTeamPanels[indexOfDataMon].GetComponent<DataMonButton>().dataMon.SetAttributesByModifier(1);

        DataTeamPanels.RemoveAt(indexOfDataMon);
        Destroy(dataMonButton.gameObject);

        //Destroy(dataMonButton.DataMonSummoned);
    }
    int GetEmptySlotInTeam()
    {
        int temp = -1;
        for (int i = 0; i < DataTeam.Length; i++)
        {
            if (DataTeam[i].isNull())
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
    public void SetDataPadToExpedition()
    {
        CurrentModule = DataPadModules.ConfirmToExpedition;
    }

    public void SetDataPadToNone()
    {
        CurrentModule = DataPadModules.None;
    }
    private void OnDestroy()
    {
        RightClick -= DataMonButtonRightClick;

    }
    public DataMonButton[] GetDataMonFromTeam(DataMonsData dataMonsData)
    {
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
            tempList.Add(DataTeamPanels[i].GetComponent<DataMonButton>());

        }
        return tempList.ToArray();
    }
//    public DataMonButton GetProductionDataMonFromTeam(DataMonsData dataMonsData, int evolution, bool isProduction)
//    {
//        DataMonButton temp = null;
//        List<DataMonButton> tempList = new List<DataMonButton>();
//        for (int i = 0; i < DataTeam.Length; i++)
//        {
//            if (DataTeam[i] == null)
//                continue;
//#if UNITY_EDITOR
//            if (DataTeam[i].dataMonData == null)
//                continue;
//#endif 
//            if (DataTeam[i].dataMonData != dataMonsData)
//                continue;
//            if (DataTeam[i].dataMonData.DataMons.GetDataMonIndexInDataArray(DataTeam[i].dataMon.DataMonName) < evolution)
//                continue;
//            temp = DataTeamPanels[i].GetComponent<DataMonButton>();
//            tempList.Add(temp);

//        }
//        bool isAvailable = false;
//        for (int i = 0; i < tempList.Count; i++)
//        {
//            if (tempList[i].dataMon.dataMonAI.AI_state != AI_State.Produce)
//            {
//                temp = tempList[i];
//                isAvailable = true;
//                break;
//            }
//        }
//        if (!isAvailable && tempList.Count > 0)
//            temp = tempList[0];
//        return temp;
//    }
//    //    public DataMonButton GetDataMonFromTeam(DataMonsData dataMonsData)
//    //    {
//    //        DataMonButton temp = null;
//    //        List<DataMonButton> tempList = new List<DataMonButton>();
//    //        for (int i = 0; i < DataTeam.Length; i++)
//    //        {
//    //            if (DataTeam[i] == null)
//    //                continue;
//    //#if UNITY_EDITOR
//    //            if (DataTeam[i].dataMonData == null)
//    //                continue;
//    //#endif 
//    //            if (DataTeam[i].dataMonData != dataMonsData)
//    //                continue;
//    //            temp = DataTeamPanels[i].GetComponent<DataMonButton>();
//    //            tempList.Add(temp);

//    //        }
//    //        return temp;
//    //    }
    public int GetDataMonCountFromTeam(DataMonsData dataMonsData)
    {
        int count = 0;
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
            count++;

        }
        return count;
    }
    //    public void SetDataMonFromTeamByModifier(float modifier, DataMonsData dataMonsData)
    //    {
    //        for (int i = 0; i < DataTeam.Length; i++)
    //        {
    //            if (DataTeam[i] == null)
    //                continue;
    //#if UNITY_EDITOR
    //            if (DataTeam[i].dataMonData == null)
    //                continue;
    //#endif 
    //            if (DataTeam[i].dataMonData != dataMonsData)
    //                continue;
    //            DataTeam[i].CurrentAttributes.SetAttributesByModifier(DataTeam[i].dataMon.BaseAttributes, modifier);

    //        }
    //}

    public static DataMonHolder[] GetAllDataMonsInHub()
    {
        instance.DataMonObtained = instance.DataMonObtained.RemoveNullReferencesInList();
        DataMonHolder[] dataMonHolders = new DataMonHolder[instance.DataMonObtained.Count];
        for (int i = 0; i < instance.DataMonObtained.Count; i++)
        {
            dataMonHolders[i] = instance.DataMonObtained[i].GetComponent<DataMonButton>().dataMonHolder;
        }
        return dataMonHolders;
    }
}

public enum DataPadModules
{
    DataDex, Evolve, Craft,None, ConfirmToExpedition
}
public class DataDexIO
{
    public DataMonHolder toDataDex;

    public void SendToDataDex()
    {
        //toDataDex.dataMon.MonBehaviourState = DataMonBehaviourState.isCompanion;
        DataDex.instance.AddToDataDex(toDataDex);
    }
}
[System.Serializable]
public class DataMonHolder
{
    public DataMonsData dataMonData;
    public string dataMonDataName;
    public AbilitiesScriptableObjects abilities;
    public string abilitiesName;
    public Attack[] dataMonAttacks;
    public DataMonIndividualData dataMon;
    public DataMonAttributes dataMonBaseAttributes;
    public DataMonInstancedAttributes dataMonCurrentAttributes;
    public bool inBank;
    public DataMonHolder() { }
    public DataMonHolder(IndividualDataMon.DataMon toHold)
    {
        dataMonData = toHold.dataMonData;
        dataMonDataName = toHold.dataMonData.name;
        if(dataMonData.Ability != null)
        abilitiesName = dataMonData.Ability.name;
        dataMon = toHold.dataMon;

        dataMon.UIsprite = toHold.dataMonData.DataMons.GetDataMonInDataArray(toHold.dataMon.DataMonName).UIsprite;
        dataMon.UIspriteName = toHold.dataMonData.DataMons.GetDataMonInDataArray(toHold.dataMon.DataMonName).UIsprite.name;
        dataMon.DataMonPrefabName = toHold.dataMonData.DataMons.GetDataMonInDataArray(toHold.dataMon.DataMonName).DataMonPrefab.name;

        dataMonAttacks = new Attack[toHold.attackObjects.Count];
        //Debug.Log("toHold.attackObjects.Count" + toHold.attackObjects.Count);
        for (int i = 0; i < toHold.attackObjects.Count; i++)
        {
            //Debug.Log("AttacksObjects" + toHold.dataMonData.AttacksObjects.GetAttackByName(toHold.attackObjects[i].AttackName));

            dataMonAttacks[i] = toHold.dataMonData.AttacksObjects.GetAttackByName(toHold.attackObjects[i].AttackName);
            dataMonAttacks[i].AttackPrefabName = dataMonAttacks[i].AttackPrefab.name;
            //Debug.Log("dataMonAttacks[i] " + dataMonAttacks[i].AttackName);
        }

        dataMonBaseAttributes = toHold.baseAttributes;
        dataMonCurrentAttributes = new DataMonInstancedAttributes(toHold.CurrentAttributes);
        //abilities = ;
        abilities = toHold.dataMonData.Ability.Serialize();
    }
    public DataMonHolder(DataMonHolder toHold)
    {
        dataMonData = toHold.dataMonData;
        dataMon = toHold.dataMon;
        //Debug.Log(dataMon.DataMonName);
        dataMonAttacks = new Attack[toHold.dataMonAttacks.Length];
        //Debug.Log("toHold.attackObjects.Count" + toHold.dataMonAttacks.Length);
        for (int i = 0; i < toHold.dataMonAttacks.Length; i++)
        {
            //Debug.Log("AttacksObjects" + toHold.dataMonData.AttacksObjects.GetAttackByName(toHold.dataMonAttacks[i].AttackName));

            dataMonAttacks[i] = toHold.dataMonData.AttacksObjects.GetAttackByName(toHold.dataMonAttacks[i].AttackName);

            //Debug.Log("dataMonAttacks[i] " + dataMonAttacks[i].AttackName);
        }

        dataMon.UIsprite = toHold.dataMonData.DataMons.GetDataMonInDataArray(toHold.dataMon.DataMonName).UIsprite;
        dataMon.UIspriteName = toHold.dataMonData.DataMons.GetDataMonInDataArray(toHold.dataMon.DataMonName).UIsprite.name;
        dataMon.DataMonPrefabName = toHold.dataMonData.DataMons.GetDataMonInDataArray(toHold.dataMon.DataMonName).DataMonPrefab.name;

        dataMonBaseAttributes = toHold.dataMonBaseAttributes;
        dataMonCurrentAttributes = new DataMonInstancedAttributes(toHold.dataMonCurrentAttributes);
        abilities = toHold.dataMonData.Ability.Serialize();

    }
    public int GetTier()
    {
        return dataMonData.DataMons.GetDataMonIndexInDataArray(dataMon);
    }
}
