using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using Unity.EditorCoroutines.Editor;
using SimpleJSON;
using UnityEngine.Rendering;
using System.Data;
using UnityEditor.VersionControl;

public class StageImpoterWindow : EditorWindow
{
    private string sheetUrl = "https://script.google.com/macros/s/AKfycbxL_PJlFo4U4ko1xq14aEVnyYbS3OLwRI8EpZkqHZ-AdK1J7jMCoQSFNYYWqUu1SLyvwg/exec";
    private static StageDataList stageDataList;
    private static ItemScriptableObject itemDataList;

    private static string directory = "Assets/HJS/06.SciptableObject/";
    private string[] options = { "��������", "������" };
    private int selectedIndex = 0;
    private string type;


    [MenuItem("Importer/SheetDataImporter")]
    public static void ShowWindow()
    {
        GetWindow<StageImpoterWindow>("��Ʈ ���� �ҷ�����");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("��Ʈ JSON URL", sheetUrl);

        GUILayout.Label("� �����͸� �������ó���?", EditorStyles.boldLabel);

        selectedIndex = EditorGUILayout.Popup("�ɼ� ����", selectedIndex, options);

        GUILayout.Space(10);
        GUILayout.Label("���õ� ��: " + options[selectedIndex]);

        if (GUILayout.Button("������ �������� �� SO ����"))
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(ImportStageDataFromSheet(sheetUrl));
        }
    }

    private IEnumerator ImportStageDataFromSheet(string url)
    {
        type = selectedIndex  == 0 ? "stage" : "item";

        string urlAddType = $"{url}?type={type}";
        UnityWebRequest www = UnityWebRequest.Get(urlAddType);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("������ �������� ����: " + www.error);
            yield break;
        }

        string json = www.downloadHandler.text;
        Debug.Log("������ JSON: " + json);


        if(selectedIndex == 0)
        {
            GetStageDataList();
            JSONNode jsonData = JSONNode.Parse(json);
            List<StageData> stageDataList = JsonParseStageData(jsonData);

            foreach (var stage in stageDataList)
            {
                CreateStageWaveSO(stage);
            }
        }
        else
        {
            GetItemDataList();
            JSONNode jsonData = JSONNode.Parse(json);
            List<ItemData> itemDataList = JsonParseItemData(jsonData);

            foreach (var item in itemDataList)
            {
                CreateItemDataSO(item);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("ScriptableObject ���� �Ϸ�!");

    }

    /// <summary>
    /// SO ���� �ߺ��� ������ �ǳʶ�
    /// </summary>
    /// <param name="data"></param>
    private static void CreateStageWaveSO(StageData data)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = $"{directory}/{data.stageName}.asset";

        //������ �ִٸ� �ش� ��η� ���� ����Ʈ�� �־��ֱ�
        if (File.Exists(path))
        {
            Debug.LogWarning($"[StageImporter] ���� �ߺ����� ���� �ǳʶ�: {path}");
            var existing = AssetDatabase.LoadAssetAtPath<StageData>(path);
            if (existing != null)
            {
                stageDataList.gameStageDataList.Add(existing);
                EditorUtility.SetDirty(stageDataList);  // �����Ϳ��� dirty �÷��� ����
                AssetDatabase.SaveAssets();             // ������� ����
            }
            return;
        }

        //������ ���ٸ� ���� �� �����ϰ� ����Ʈ�� �����ϱ�(���� �� ������ �߿���)
        var asset = ScriptableObject.CreateInstance<StageData>();
        asset.stageName = data.stageName;
        asset.totalWave = data.totalWave;
        asset.monstersPerWave = data.monstersPerWave;
        asset.spawnMonsterType = data.spawnMonsterType;

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets(); // ���� �� ���ֱ�

        var loadedAsset = AssetDatabase.LoadAssetAtPath<StageData>(path); 
        if (loadedAsset != null)
        {
            stageDataList.gameStageDataList.Add(loadedAsset);
            EditorUtility.SetDirty(stageDataList);  // �����Ϳ��� dirty �÷��� ����
            AssetDatabase.SaveAssets();             // ������� ����
        }

    }


    private static void CreateItemDataSO(ItemData data)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        itemDataList.itemData.Add(data);
        EditorUtility.SetDirty(itemDataList);  // �����Ϳ��� dirty �÷��� ����
        AssetDatabase.SaveAssets();             // ������� ����
    }

    /// <summary>
    /// �������� ���� JSON �Ľ�
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public List<StageData> JsonParseStageData(JSONNode json)
    {
        List<StageData> stageDataList = new List<StageData>();

        for(int i = 0; i < json.Count; i++)
        {
            StageData data = new StageData();
            data.stageName = json[i]["StageName"];
            data.totalWave = json[i]["TotalWave"];

            //���̺� �� ���� ���� ����
            string monsterStr = json[i]["MonstersPerWave"];
            data.monstersPerWave = ParsingDataCovertArray(monsterStr);
            
            //���������� �����ϴ� ���� Ÿ�� ����
            string monsterType = json[i]["SpawnMonsterType"];
            data.spawnMonsterType = ParsingDataCovertArray(monsterType);

            stageDataList.Add(data);
        }
        return stageDataList;
    }

    public List<ItemData> JsonParseItemData(JSONNode json)
    {
        List<ItemData> itemDataList = new List<ItemData>();

        for (int i = 0; i < json.Count; i++)
        {
            ItemData data = new ItemData();
            data.itemName = json[i]["ItemName"];
            data.itemDesc = json[i]["Description"];
            data.price = json[i]["Price"].AsInt;
            data.spriteName = json[i]["SpriteName"];
            data.itemType = (ItemType)json[i]["ItemType"].AsInt;

            if(data.itemType == ItemType.Heal)
            {
                //���� ����
                data.healRatio = json[i]["HealRatio"].AsFloat;
            }
            itemDataList.Add(data);
        }
        return itemDataList;
    }

    /// <summary>
    /// �迭 int �迭 ���·� �Ľ��ؼ� ���� ���ִ� �Լ�
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int[] ParsingDataCovertArray(string data)
    {
        data = data.Trim('"'); // �յ� ����ǥ ����

        string[] split = data.Split(',');

        int[] covertArray = new int[split.Length];
        for (int j = 0; j < split.Length; j++)
        {
            int.TryParse(split[j], out covertArray[j]);
        }

        return covertArray;
    }

    /// <summary>
    /// �������� ���� ����Ʈ ��ũ���ͺ� ������Ʈ ��������(������ ����)
    /// </summary>
    public void GetStageDataList()
    {
        string path = $"{directory}/{type}/StageDataList.asset";

        if (!File.Exists(path))
        {
            var asset = ScriptableObject.CreateInstance<StageDataList>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            stageDataList = asset;
        }
        else
        {
            stageDataList = AssetDatabase.LoadAssetAtPath<StageDataList>(path);
        }
        stageDataList.Initalize();
    }

    /// <summary>
    /// ������ ������ ����Ʈ ��ũ���ͺ� ������Ʈ ��������(������ ����)
    /// </summary>
    public void GetItemDataList()
    {
        
        string path = $"{directory}/{type}/ItemDataList.asset";

        if (!File.Exists(path))
        {
            var asset = ScriptableObject.CreateInstance<ItemScriptableObject>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            itemDataList = asset;
        }
        else
        {
            itemDataList = AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
        }
        itemDataList.Initalize();
    }
}