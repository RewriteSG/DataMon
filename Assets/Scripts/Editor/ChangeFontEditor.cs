using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
[CustomEditor(typeof(ChangeFont))]
public class ChangeFontEditor : Editor
{
    ChangeFont cf;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        cf = (ChangeFont)target;
        TextMeshProUGUI[] textMeshProUGUIs;
        if (GUILayout.Button("Change FONTS!") && !cf.fontAsset.isNull())
        {
            textMeshProUGUIs = cf.transform.GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < textMeshProUGUIs.Length; i++)
            {
                textMeshProUGUIs[i].font = cf.fontAsset;
            }
        }

    }
}
