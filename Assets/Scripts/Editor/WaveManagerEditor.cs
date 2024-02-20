using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(WaveManager),true)]
public class WaveManagerEditor : Editor
{
    WaveManager wm;
    Wave selectedWave;
    EnemiesInWave selectedEnemy;
    int SelectedWaveIndex, SelectedWaveEnemyIndex, SelectedDataMonIndex;
    SerializedProperty wave, _EnemiesInWave, /*difficulty,*/ _DataMonData;
    //private void Start()
    //{
    //    wm = (WaveManager)target; 
    //}
    void OnEnable()
    {

        wave = serializedObject.FindProperty(nameof(wm.serializeWave));
        _EnemiesInWave = serializedObject.FindProperty(nameof(wm.serializeEnemiesInWave));
        //difficulty = serializedObject.FindProperty(nameof(wm.serializedifficulty));
        _DataMonData = serializedObject.FindProperty(nameof(wm.serializeDataMonData));



    }
    public override void OnInspectorGUI()
    {
        wm = (WaveManager)target;

        //SerializeFields();

        SelectedWaveIndex = EditorGUILayout.Popup("Waves", SelectedWaveIndex, wm.GetAllWavesInWaveManager());
        if (SelectedWaveIndex < wm.Waves.Count)
            selectedWave = wm.Waves[SelectedWaveIndex];
        if (SelectedWaveIndex == wm.Waves.Count)
            wm.AddWave();
        if (selectedWave == null)
            return;

        wm.serializeWave = wm.Waves[SelectedWaveIndex];
        EditorGUILayout.PropertyField(wave);
        //EditorGUILayout.PropertyField(difficulty);
        serializedObject.ApplyModifiedProperties();

        GUILayout.Label("Total Number Of enemies " + wm.serializeWave.GetTotalNumberOfEnemies());


        wm.serializedifficulty = wm.Waves[SelectedWaveIndex].difficulty;

        SelectedWaveEnemyIndex = EditorGUILayout.Popup("Enemies", SelectedWaveEnemyIndex, wm.Waves[SelectedWaveIndex].GetAllDataMonsInWave());


        if (SelectedWaveEnemyIndex < wm.Waves[SelectedWaveIndex]._EnemiesInWave.Count)
            selectedEnemy = wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex];
        if (SelectedWaveEnemyIndex == wm.Waves[SelectedWaveIndex]._EnemiesInWave.Count)
            wm.Waves[SelectedWaveIndex].AddEnemy();
        if (selectedEnemy == null)
            return;

        wm.serializeEnemiesInWave = wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex];
        EditorGUILayout.PropertyField(_EnemiesInWave);

        //wm.serializeDataMonData = wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex]._DataMonsData;
        //EditorGUILayout.PropertyField(_DataMonData, false);
        serializedObject.ApplyModifiedProperties();

        if (wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex]._DataMonsData == null)
            goto Skip;

        wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].DataMonsDataIndex = 
            EditorGUILayout.Popup("Enemies",
            wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].DataMonsDataIndex,
            wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].GetAllDataMonsInData());

        if (wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].DataMon.DataMonName !=
            wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].
            _DataMonsData.DataMons[wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].DataMonsDataIndex].DataMonName)
            Undo.RecordObject(wm, "WHY");
        wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].
            SetEnemy(wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex]._DataMonsData.DataMons[SelectedDataMonIndex]);
        wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].name = wm.Waves[SelectedWaveIndex]._EnemiesInWave[SelectedWaveEnemyIndex].DataMon.DataMonName;



        serializedObject.ApplyModifiedProperties();
        Skip:
        GUILayout.Space(67);
        if (GUILayout.Button("Remove Enemy"))
        {
            wm.Waves[SelectedWaveIndex].RemoveEnemy(SelectedWaveEnemyIndex);
            SelectedWaveEnemyIndex--;
        }

    }
}
