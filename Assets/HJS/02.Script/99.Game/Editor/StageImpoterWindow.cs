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
    private string[] options = { "스테이지", "아이템" };
    private int selectedIndex = 0;
    private string type;


    [MenuItem("Importer/SheetDataImporter")]
    public static void ShowWindow()
    {
        GetWindow<StageImpoterWindow>("시트 정보 불러오기");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("시트 JSON URL", sheetUrl);

        GUILayout.Label("어떤 데이터를 가져오시나요?", EditorStyles.boldLabel);

        selectedIndex = EditorGUILayout.Popup("옵션 선택", selectedIndex, options);

        GUILayout.Space(10);
        GUILayout.Label("선택된 값: " + options[selectedIndex]);

        if (GUILayout.Button("데이터 가져오기 및 SO 생성"))
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
            Debug.LogError("데이터 가져오기 실패: " + www.error);
            yield break;
        }

        string json = www.downloadHandler.text;
        Debug.Log("가져온 JSON: " + json);


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
        Debug.Log("ScriptableObject 생성 완료!");

    }

    /// <summary>
    /// SO 생성 중복된 파일은 건너뜀
    /// </summary>
    /// <param name="data"></param>
    private static void CreateStageWaveSO(StageData data)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = $"{directory}/{data.stageName}.asset";

        //파일이 있다면 해당 경로로 가서 리스트에 넣어주기
        if (File.Exists(path))
        {
            Debug.LogWarning($"[StageImporter] 파일 중복으로 생성 건너뜀: {path}");
            var existing = AssetDatabase.LoadAssetAtPath<StageData>(path);
            if (existing != null)
            {
                stageDataList.gameStageDataList.Add(existing);
                EditorUtility.SetDirty(stageDataList);  // 에디터에서 dirty 플래그 설정
                AssetDatabase.SaveAssets();             // 변경사항 저장
            }
            return;
        }

        //파일이 없다면 만든 후 저장하고 리스트에 저장하기(생성 후 저장이 중요함)
        var asset = ScriptableObject.CreateInstance<StageData>();
        asset.stageName = data.stageName;
        asset.totalWave = data.totalWave;
        asset.monstersPerWave = data.monstersPerWave;
        asset.spawnMonsterType = data.spawnMonsterType;

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets(); // 저장 꼭 해주기

        var loadedAsset = AssetDatabase.LoadAssetAtPath<StageData>(path); 
        if (loadedAsset != null)
        {
            stageDataList.gameStageDataList.Add(loadedAsset);
            EditorUtility.SetDirty(stageDataList);  // 에디터에서 dirty 플래그 설정
            AssetDatabase.SaveAssets();             // 변경사항 저장
        }

    }


    private static void CreateItemDataSO(ItemData data)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        itemDataList.itemData.Add(data);
        EditorUtility.SetDirty(itemDataList);  // 에디터에서 dirty 플래그 설정
        AssetDatabase.SaveAssets();             // 변경사항 저장
    }

    /// <summary>
    /// 스테이지 정보 JSON 파싱
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

            //웨이브 별 몬스터 개수 저장
            string monsterStr = json[i]["MonstersPerWave"];
            data.monstersPerWave = ParsingDataCovertArray(monsterStr);
            
            //스테이지에 등장하는 몬스터 타입 설정
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
                //힐량 설정
                data.healRatio = json[i]["HealRatio"].AsFloat;
            }
            itemDataList.Add(data);
        }
        return itemDataList;
    }

    /// <summary>
    /// 배열 int 배열 형태로 파싱해서 리턴 해주는 함수
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int[] ParsingDataCovertArray(string data)
    {
        data = data.Trim('"'); // 앞뒤 따옴표 제거

        string[] split = data.Split(',');

        int[] covertArray = new int[split.Length];
        for (int j = 0; j < split.Length; j++)
        {
            int.TryParse(split[j], out covertArray[j]);
        }

        return covertArray;
    }

    /// <summary>
    /// 스테이지 정보 리스트 스크립터블 오브젝트 가져오기(없으면 생성)
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
    /// 아이템 데이터 리스트 스크립터블 오브젝트 가져오기(없으면 생성)
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