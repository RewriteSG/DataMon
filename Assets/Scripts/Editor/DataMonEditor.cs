using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using IndividualDataMon;
[CustomEditor(typeof(DataMon))]
public class DataMonEditor : Editor
{
    DataMon dataMon;
    List<string> DataMonNames = new List<string>();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        dataMon = (DataMon)target;
        if (dataMon.dataMonData == null)
            return;
        if(DataMonNames.Count != dataMon.dataMonData._DataMon.Length)
        {
            DataMonNames.Clear();
            for (int i = 0; i < dataMon.dataMonData._DataMon.Length; i++)
            {
                DataMonNames.Add(dataMon.dataMonData._DataMon[i].DataMonName);
            }
        }
        GUILayout.BeginHorizontal();
        //GUILayout.Label("DataMon -> ");
        dataMon.selected = EditorGUILayout.Popup("DataMon -> ", dataMon.selected, DataMonNames.ToArray());
        if (dataMon.dataMon.DataMonName != dataMon.dataMonData._DataMon[dataMon.selected].DataMonName)
            dataMon.SetDataMon(dataMon.dataMonData._DataMon[dataMon.selected]);
        GUILayout.EndHorizontal();
    }
}
