// DataMonEvolution
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DatamonEvolution : MonoBehaviour
{
    public GameObject SelectDataMonToEvolve, EvolutionAvailablePanel, DataMonNotAvailableText, SelectDataMonName;
    public Button EvolveButton;

    public DataMonHolder DataMonToEvolve;
    public List<DataMonHolder> Sacrifices = new List<DataMonHolder>();
    public TextMeshProUGUI DataBytesCost;
    public Image[] SacrificesPNG = new Image[] { };

    public DataMonHolder EvolutionResult;
    int EvolutionCost, toEvolveEvolutionTier, EvolutionResultEvolutionTier;

    public const int NumberOfDataMonsToSacrifice = 5;
    public const int ModifierValueForSacrifices = 2;

    public DataMonStatsTexts HoveringTexts = new DataMonStatsTexts(), ToEvolveText = new DataMonStatsTexts(), EvolutionResultText = new DataMonStatsTexts();

    private void Start()
    {
        //GameManager.instance.ent
        for (int i = 0; i < SacrificesPNG.Length; i++)
        {
            SacrificesPNG[i].color = Color.black;
        }
    }
    private void Update()
    {
        if (!DataDex.instance.EvolvePanel.activeSelf && DataMonToEvolve != null)
        {
            CancelEvolution();
        }

        if (!DataDex.instance.EvolvePanel.activeSelf && EvolutionAvailablePanel.activeSelf)
        {
            ResetEvolutionPanels();
            DataMonToEvolve = null;
        }

        if (DataMonToEvolve != null)
        {
#if UNITY_EDITOR
            if (DataMonToEvolve.dataMonData != null)
            {
#endif
                DataBytesCost.text = EvolutionCost.ToString();
                EvolveButton.interactable = GameManager.instance.Databytes >= EvolutionCost;

                ToEvolveText.SetTexts(DataMonToEvolve.dataMon.BaseAttributes.BaseHealth,
                    DataMonToEvolve.dataMon.BaseAttributes.BaseAttack, DataMonToEvolve.dataMonData.MonRole.ToString(),
                    toEvolveEvolutionTier, DataMonToEvolve.dataMon.DataMonName);

                //EvolutionResultText.SetTexts(EvolutionResult.dataMon.BaseAttributes.BaseHealth,
                //    EvolutionResult.dataMon.BaseAttributes.BaseAttack, EvolutionResult.dataMonData.MonRole.ToString(),
                //    EvolutionResultEvolutionTier, EvolutionResult.dataMon.DataMonName);
#if UNITY_EDITOR
            }
#endif

        }
        else if (DataInspector.DataMonHovering != null)
        {
            SelectDataMonName.SetActive(true);
            HoveringTexts.SetTexts(DataInspector.DataMonHovering.dataMon.DataMonName);
        }

        if (DataInspector.DataMonHovering == null)
        {

            SelectDataMonName.SetActive(false);

        }

    }
    public bool SelectDataMon(DataMonHolder dataMonHolder)
    {
        try
        {
            if (DataMonToEvolve.dataMonData != null)
                return false;

        }
        catch (System.NullReferenceException)
        {
            if (DataMonToEvolve != null)
                return false;
        }

        if (dataMonHolder.dataMonData == null)
            return false;


        bool toReturn = NextDataMonIsValid(dataMonHolder, out int index);
        if (toReturn)
        {
            SelectDataMonToEvolve.SetActive(false);
            EvolutionAvailablePanel.SetActive(true);
            toEvolveEvolutionTier = index;
            EvolutionResultEvolutionTier = index + 1;
            DataMonToEvolve = dataMonHolder;
            EvolutionResult = new DataMonHolder();
            EvolutionResult.dataMonData = dataMonHolder.dataMonData;

            EvolutionResult.dataMonBaseAttributes = dataMonHolder.dataMonBaseAttributes;
            EvolutionResult.dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMonHolder.dataMonData.DataMons[index + 1].BaseAttributes);
            EvolutionResult.dataMonAttacks = Attack.InstanceAttack(dataMonHolder.dataMonAttacks);
            EvolutionResult.dataMon = DataMonIndividualData.CloneDataMonClass(dataMonHolder.dataMonData.DataMons[index + 1]);
            EvolutionResult.dataMon.DataMonPrefab = dataMonHolder.dataMonData.DataMons[EvolutionResultEvolutionTier].DataMonPrefab;
            EvolutionResult.dataMon.DataMonPrefabName = dataMonHolder.dataMonData.DataMons[EvolutionResultEvolutionTier].DataMonPrefab.name;
            EvolutionResult.dataMon.UIsprite = dataMonHolder.dataMonData.DataMons[EvolutionResultEvolutionTier].UIsprite;
            EvolutionResult.dataMon.UIspriteName = dataMonHolder.dataMonData.DataMons[EvolutionResultEvolutionTier].UIsprite.name;

            EvolutionCost = dataMonHolder.dataMonData.EvolutionCosts[index];

            Sacrifices.Clear();
            for (int i = NumberOfDataMonsToSacrifice - ModifierValueForSacrifices * toEvolveEvolutionTier; i < NumberOfDataMonsToSacrifice; i++)
            {
                SacrificesPNG[i].color = Color.clear;
            }
        }
        else
        {
            DataMonNotAvailableText.SetActive(true);
        }
        return toReturn;
    }
    public bool SelectSacrifice(DataMonHolder dataMonHolder)
    {
        try
        {
            if (dataMonHolder.dataMonData == null)
                return false;

        }
        catch (System.NullReferenceException)
        {
            if (dataMonHolder == null)
                return false;
        }

        //if (dataMonHolder.dataMonData == null)
        //    return false;
        if (Sacrifices.Count >= NumberOfDataMonsToSacrifice - ModifierValueForSacrifices * toEvolveEvolutionTier ||
            Sacrifices.Contains(dataMonHolder) || DataMonToEvolve == dataMonHolder)
            return false;

        bool toReturn = DataMonIsValidForSacrifice(dataMonHolder, toEvolveEvolutionTier);
        if (toReturn)
        {
            SelectDataMonToEvolve.SetActive(false);
            EvolutionAvailablePanel.SetActive(true);
            Sacrifices.Add(dataMonHolder);
            SacrificesPNG[Sacrifices.Count - 1].color = Color.red;
            //toEvolveEvolutionTier = index;
            //EvolutionResultEvolutionTier = index + 1;
            //DataMonHolder temp = new DataMonHolder();
            //EvolutionResult.dataMonData = dataMonHolder.dataMonData;

            //EvolutionResult.dataMonBaseAttributes = dataMonHolder.dataMonBaseAttributes;
            //EvolutionResult.dataMonCurrentAttributes = new DataMonInstancedAttributes(dataMonHolder.dataMonData.DataMons[index + 1].BaseAttributes);
            //EvolutionResult.dataMonAttacks = Attack.InstanceAttack(dataMonHolder.dataMonAttacks);
            //EvolutionResult.dataMon = DataMonIndividualData.CloneDataMonClass(dataMonHolder.dataMonData.DataMons[index + 1]);
            //EvolutionCost = dataMonHolder.dataMonData.EvolutionCosts[index];
        }
        else
        {
            DataMonNotAvailableText.SetActive(true);
        }
        return toReturn;
    }
    
    public void EvolveDataMon()
    {
        if (GameManager.instance.Databytes >= EvolutionCost && Sacrifices.Count == NumberOfDataMonsToSacrifice - ModifierValueForSacrifices * toEvolveEvolutionTier)
        {
            DataDex.instance.EvolveDataMon(EvolutionResult);

            GameManager.instance.Databytes -= EvolutionCost;
            CancelEvolution(true);
        }
    }
    public void CancelEvolution()
    {
        if (DataDex.instance.DataMonOnTeamIsSelectedForEvolve == null)
        {

            DataDex.instance.AddToDataDex(DataMonToEvolve);
        }
        for (int i = 0; i < Sacrifices.Count; i++)
        {
            if (Sacrifices[i].inBank)
                DataDex.instance.AddToDataDex(Sacrifices[i]);
        }
        ResetEvolutionPanels();

        DataMonToEvolve = null;
    }
    public void CancelEvolution(bool isEvolved)
    {
        ResetEvolutionPanels();

        DataMonToEvolve = null;
    }
    private void ResetEvolutionPanels()
    {
        SelectDataMonToEvolve.SetActive(true);
        DataMonNotAvailableText.SetActive(false);
        EvolutionAvailablePanel.SetActive(false);
        for (int i = 0; i < SacrificesPNG.Length; i++)
        {
            SacrificesPNG[i].color = Color.black;
        }
    }

    bool NextDataMonIsValid(DataMonHolder dataMonHolder, out int NextEvolutionIndex)
    {
        int index = dataMonHolder.dataMonData.DataMons.GetDataMonIndexInDataArray(dataMonHolder.dataMon.DataMonName);
        print(index);
        NextEvolutionIndex = index;
        if (NextEvolutionIndex >= dataMonHolder.dataMonData.DataMons.Length - 1)
        {
            return false;
        }
        else
            return true;
    }
    bool DataMonIsValidForSacrifice(DataMonHolder dataMonHolder, int Index)
    {
        return Index == dataMonHolder.dataMonData.DataMons.GetDataMonIndexInDataArray(dataMonHolder.dataMon.DataMonName);
        //print(index);
        //NextEvolutionIndex = index;
        //if (NextEvolutionIndex >= dataMonHolder.dataMonData.DataMons.Length - 1)
        //{
        //    return false;
        //}
        //else
        //    return true;
    }
}
[System.Serializable]
public class DataMonStatsTexts
{
    public TextMeshProUGUI txt_HP, txt_Atk, txt_Role, txt_Evolution, txt_DataName, txt_Ability;
    public void SetTexts(string name)
    {
        txt_DataName.text = name;
    }
    public void SetTexts(float hp, float atk, string role, int evolution, string name)
    {
        //txt_HP.text = "HP: " + hp;
        //txt_Atk.text = "ATK: " + atk;
        //txt_Role.text = "Role: " + role;
        //txt_Evolution.text = "Evolution: " + hp;
        if(txt_DataName!=null)
        txt_DataName.text = name;
    }
}
