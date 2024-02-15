using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IndividualDataMon;
[CustomEditor(typeof(DataMon))]
public class DataMonEditor : Editor
{
    //DataMon dataMon;
    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    dataMon = (DataMon)target;
    //    if (dataMon.dataMonData == null || dataMon.dataMonData.DataMons[0] == null)
    //        return;
    //    if(dataMon.DataMonNames.Count != dataMon.dataMonData.DataMons.Length || dataMon.DataMonNames[0] != dataMon.dataMonData.DataMons[0].DataMonName)
    //    {
    //        dataMon.DataMonNames.Clear();
    //        for (int i = 0; i < dataMon.dataMonData.DataMons.Length; i++)
    //        {
    //            dataMon.DataMonNames.Add(dataMon.dataMonData.DataMons[i].DataMonName);
    //        }

    //    }
    //    //GUILayout.BeginHorizontal();
    //    ////GUILayout.Label("DataMon -> ");
    //    //dataMon.selected = EditorGUILayout.Popup("DataMon -> ", dataMon.selected, dataMon.DataMonNames.ToArray());
    //    //if (dataMon.dataMon.DataMonName != dataMon.dataMonData.DataMons[dataMon.selected].DataMonName)
    //    //{
    //    //    dataMon.SetDataMonInEditor(dataMon.dataMonData.DataMons[dataMon.selected]);
    //    //}
    //    //GUILayout.EndHorizontal();
    //}
}
