using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageScriptableObjectCreater : EditorWindow
{
    private string inputText = "�������� �̸�";
    private int waveCount = 1;
    private List<int> monsterPerWaveList = new List<int>() { 0 };


    [MenuItem("Tools/�������� ������")]
    public static void ShowWindow()
    {
        GetWindow<StageScriptableObjectCreater>("�������� ������");
    }

    private void OnGUI()
    {
        GUILayout.Label("Stage Wave ScriptableObject ������", EditorStyles.boldLabel);

        inputText = EditorGUILayout.TextField("�������� �̸�", inputText);
        waveCount = EditorGUILayout.IntField("���̺� ��", waveCount);

        while (monsterPerWaveList.Count < waveCount)
            monsterPerWaveList.Add(0);

        while (monsterPerWaveList.Count > waveCount)
            monsterPerWaveList.RemoveAt(monsterPerWaveList.Count - 1);

        EditorGUILayout.Space(5);
        GUILayout.Label("�� ���̺� ���� ��");

        for (int i = 0; i < waveCount; i++)
        {
            monsterPerWaveList[i] = EditorGUILayout.IntField($"Wave {i + 1} ���� ��", monsterPerWaveList[i]);
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("ScriptableObject ����"))
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

        Debug.Log($"ScriptableObject ���� �Ϸ�: {path}");
    }
}


