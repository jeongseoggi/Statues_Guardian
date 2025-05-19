using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageScriptableObjectCreater : EditorWindow
{
    private string inputText = "스테이지 이름";
    private int waveCount = 1;
    private List<int> monsterPerWaveList = new List<int>() { 0 };


    [MenuItem("Tools/스테이지 생성기")]
    public static void ShowWindow()
    {
        GetWindow<StageScriptableObjectCreater>("스테이지 생성기");
    }

    private void OnGUI()
    {
        GUILayout.Label("Stage Wave ScriptableObject 생성기", EditorStyles.boldLabel);

        inputText = EditorGUILayout.TextField("스테이지 이름", inputText);
        waveCount = EditorGUILayout.IntField("웨이브 수", waveCount);

        while (monsterPerWaveList.Count < waveCount)
            monsterPerWaveList.Add(0);

        while (monsterPerWaveList.Count > waveCount)
            monsterPerWaveList.RemoveAt(monsterPerWaveList.Count - 1);

        EditorGUILayout.Space(5);
        GUILayout.Label("각 웨이브 몬스터 수");

        for (int i = 0; i < waveCount; i++)
        {
            monsterPerWaveList[i] = EditorGUILayout.IntField($"Wave {i + 1} 몬스터 수", monsterPerWaveList[i]);
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("ScriptableObject 생성"))
        {
            CreateStageWaveSO(inputText, waveCount, monsterPerWaveList.ToArray());
        }
    }


    private void CreateStageWaveSO(string name, int waveCount, int[] monstersPerWave)
    {
        var asset = ScriptableObject.CreateInstance<StageData>();
        asset.totalWave = waveCount;
        asset.monstersPerWave = monstersPerWave;

        string path = $"Assets/HJS/06.SciptableObject/Stage/{name}.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        Debug.Log($"ScriptableObject 생성 완료: {path}");
    }
}


