using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DataMonsData))]
public class DataMonDataEditor : Editor
{
    DataMonsData data;
    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    data = (DataMonsData)target;
    //    for (int i = 0; i < data.DataMons.Length; i++)
    //    {
    //        data.DataMons[i].DataMonPrefab.GetComponent<IndividualDataMon.DataMon>().dataMon = data.DataMons[i];
    //    }

    //}
}
