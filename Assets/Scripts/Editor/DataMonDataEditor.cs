using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DataMonsData))]
public class DataMonDataEditor : Editor
{
    DataMonsData data;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        data = (DataMonsData)target;
        GUILayout.Label("-----Evolution Cost-----------");
        if (data.EvolutionCosts.Count < data.DataMons.Length - 1)
        {
            for (int i = 0; i < data.DataMons.Length - 1; i++)
            {
                data.EvolutionCosts.Add(0);
            }
        }
        if (data.EvolutionCosts.Count > data.DataMons.Length - 1)
        {
            data.EvolutionCosts.RemoveAt(data.EvolutionCosts.Count - 1);
        }
        for (int i = 0; i < data.EvolutionCosts.Count; i++)
        {
            data.EvolutionCosts[i] = EditorGUILayout.IntField("Evolution" + (i + 1) + "Cost:", data.EvolutionCosts[i]);
        }
    }
}
