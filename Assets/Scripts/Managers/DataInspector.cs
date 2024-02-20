using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DataInspector : MonoBehaviour
{
    public static DataMonHolder DataMonHovering;
    public GameObject DataInspectorUI;
    public TextMeshProUGUI txt_HP, txt_Atk, txt_Role, txt_Evolution, txt_DataName, txt_Ability;
    public HealthBarScript DataInspectorHealthBar;
    int DataMonEvolveTier = -1;
    // Start is called before the first frame update
    //void Start()
    //{
    //    DataMonHovering = null;
    //    DataMonEvolveTier = -1;
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    //switch (DataDex.instance.CurrentModule)
    //    //{
    //    //    case DataPadModules.DataDex:
    //    //        InspectDataMon();
    //    //        break;
    //    //    case DataPadModules.Evolve:
    //    //        InspectDataMonEvolution();
    //    //        break;
    //    //}
    //}
    void InspectDataMonEvolution()
    {
        if (DataMonHovering == null)
        {
            DataMonEvolveTier = -1;
            return;
        }
        if (DataMonEvolveTier == -1)
        {
            DataMonEvolveTier = DataMonHovering.dataMonData.DataMons.GetDataMonIndexInDataArray(DataMonHovering.dataMon.DataMonName);
        }
    }
    private void InspectDataMon()
    {
        if (DataMonHovering == null)
        {
            DataMonEvolveTier = -1;
            DataInspectorUI.SetActive(false);
            return;
        }
        if (DataMonEvolveTier == -1)
        {
            DataInspectorUI.SetActive(true);
            DataMonEvolveTier = DataMonHovering.dataMonData.DataMons.GetDataMonIndexInDataArray(DataMonHovering.dataMon.DataMonName);
        }

        if (DataInspectorHealthBar.slider.maxValue != DataMonHovering.dataMonBaseAttributes.BaseHealth)
        {
            DataInspectorHealthBar.SetMaxHealth(Mathf.RoundToInt(DataMonHovering.dataMonBaseAttributes.BaseHealth));
        }
        DataInspectorHealthBar.SetHealth(Mathf.RoundToInt(DataMonHovering.dataMonCurrentAttributes.CurrentHealth));
        txt_HP.text = "HP: " + Mathf.RoundToInt(DataMonHovering.dataMonCurrentAttributes.CurrentHealth)
            + "/" + DataMonHovering.dataMonBaseAttributes.BaseHealth;

        txt_Atk.text = "ATK: " + DataMonHovering.dataMonCurrentAttributes.CurrentAttack;

        txt_Evolution.text = "Evolution: " + DataMonEvolveTier;

        txt_DataName.text = DataMonHovering.dataMon.DataMonName;

        txt_Role.text = "Role: " + DataMonHovering.dataMonData.MonRole.ToString();
    }
}
