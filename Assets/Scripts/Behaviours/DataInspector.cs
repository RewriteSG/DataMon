using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DataInspector : MonoBehaviour
{
    public static DataMonHolder DataMonHovering;
    public TextMeshProUGUI txt_HP, txt_Atk, txt_Role, txt_Evolution, txt_DataName, txt_Ability;
    int DataMonEvolveTier = -1;
    // Start is called before the first frame update
    void Start()
    {
        DataMonHovering = null;
        DataMonEvolveTier = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (DataMonHovering == null)
        {
            DataMonEvolveTier = -1;
            return;
        }
        if (DataMonEvolveTier == -1)
            DataMonEvolveTier = DataMonHovering.dataMonData._DataMon.GetDataMonIndexInDataArray(DataMonHovering.dataMon);


        txt_HP.text = "HP: " + DataMonHovering.dataMonCurrentAttributes.CurrentHealth.ToString("0.##") 
            + "/" + DataMonHovering.dataMon.BaseAttributes.BaseHealth;

        txt_Atk.text = "ATK: " + DataMonHovering.dataMonCurrentAttributes.CurrentAttack;

        txt_Evolution.text = "Evolution: " + DataMonEvolveTier;

        txt_DataName.text = DataMonHovering.dataMon.DataMonName;

        txt_Role.text = "Role: " + nameof(DataMonHovering.dataMonData.MonRole);
    }
}
