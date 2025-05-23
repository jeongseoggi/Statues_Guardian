using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Unity.EditorCoroutines.Editor;
using SimpleJSON;
using System;
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
    /// 스테이지 데이터 추가 함수
    /// </summary>
    /// <param name="data"></param>
    private static void CreateStageWaveSO(StageData data)
    {
        CreateOrLoadSO<StageData>(data.stageName, "Stage",
            () => ScriptableObject.CreateInstance<StageData>(),
            (so) =>
            {
                so.stageName = data.stageName;
                so.totalWave = data.totalWave;
                so.monstersPerWave = data.monstersPerWave;
                so.spawnMonsterType = data.spawnMonsterType;
            },
            stageDataList.gameStageDataList,
            stageDataList);
    }

    /// <summary>
    /// 아이템 데이터 추가 함수
    /// </summary>
    /// <param name="data"></param>
    private static void CreateItemDataSO(ItemData data)
    {
        CreateOrLoadSO<ItemData>(data.spriteName, "Item",
        () =>
        {
            if(data is HealItemData)
            {
                return ScriptableObject.CreateInstance<HealItemData>();
            }
            else if(data is UpgradeItemData)
            {
                return ScriptableObject.CreateInstance<UpgradeItemData>();
            }
            else
            {
                return ScriptableObject.CreateInstance<GambleItemData>();
            }
        },
        (so) =>
        {
            so.itemName = data.itemName;
            so.itemDesc = data.itemDesc;
            so.itemType = data.itemType;
            so.itemUseStrategy = data.itemUseStrategy;
            so.price = data.price;
            so.spriteName = data.spriteName;
        },
        itemDataList.itemData,
        itemDataList);


    }


    /// <summary>
    /// 스크립터블 오브젝트 생성 통합 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <param name="subFolderName"></param>
    /// <param name="createFunc"></param>
    /// <param name="copyData"></param>
    /// <param name="addList"></param>
    public static void CreateOrLoadSO<T>(string fileName, string subFolderName, Func<T> createFunc,  Action<T> copyData, List<T> addList, UnityEngine.Object soListClass) where T : ScriptableObject
    {
        string fullPath = Path.Combine(directory, subFolderName);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        string itemPath = $"{fullPath}/{fileName}.asset";

        if (File.Exists(itemPath))
        {
            Debug.LogWarning($"[SO Importer] 파일 중복으로 생성 건너뜀: {itemPath}");
            var existing = AssetDatabase.LoadAssetAtPath<T>(itemPath);
            if (existing != null)
            {
                addList.Add(existing);
                EditorUtility.SetDirty(soListClass);  // ScriptableObject로 캐스팅해서 처리
                AssetDatabase.SaveAssets();
            }
            return;
        }

        var asset = createFunc.Invoke();
        copyData?.Invoke(asset);

        AssetDatabase.CreateAsset(asset, itemPath);
        AssetDatabase.SaveAssets();

        var loaded = AssetDatabase.LoadAssetAtPath<T>(itemPath);
        if (loaded != null)
        {
            addList.Add(loaded);
            EditorUtility.SetDirty(soListClass);
            AssetDatabase.SaveAssets();
        }
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
            switch((ItemType)json[i]["ItemType"].AsInt)
            {
                case ItemType.Heal:
                    AddItemDataList(new HealItemData(), itemDataList, json, i);
                    break;
                case ItemType.Upgrade:
                    AddItemDataList(new UpgradeItemData(), itemDataList, json, i);
                    break;
                case ItemType.Gamble:
                    AddItemDataList(new GambleItemData(), itemDataList, json, i);
                    break;
                case ItemType.Buff:
                    break;
            }
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

    /// <summary>
    /// 아이템 데이터 추가
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="itemDataList"></param>
    /// <param name="jsonData"></param>
    /// <param name="index"></param>
    public void AddItemDataList(ItemData itemData, List<ItemData> itemDataList, JSONNode jsonData, int index)
    {
        itemData.itemName = jsonData[index]["ItemName"];
        itemData.itemDesc = jsonData[index]["Description"];
        itemData.price = jsonData[index]["Price"].AsInt;
        itemData.spriteName = jsonData[index]["SpriteName"];
        itemData.itemType = (ItemType)jsonData[index]["ItemType"].AsInt;

        if (itemData is HealItemData healItemData)
        {
            healItemData.healRatio = jsonData[index]["HealRatio"].AsFloat;
            if(itemData.itemName.Contains("HP"))
            {
                healItemData.healType = HealType.HP;
            }
            else
            {
                healItemData.healType = HealType.MP;
            }
            itemDataList.Add(healItemData);
        }
        else if(itemData is UpgradeItemData upgradeItemData)
        {
            if (itemData.itemName.Contains("공격력"))
            {
                upgradeItemData.upgradeType = UpgradeType.Atk;
            }
            else
            {
                upgradeItemData.upgradeType = UpgradeType.Def;
            }
            itemDataList.Add(upgradeItemData);
        }
        else
        {
            itemDataList.Add(itemData);
        }
    }

    public static ItemData SetItemData(ItemData data)
    {
        switch (data.itemType)
        {
            case ItemType.Heal:
                HealItemData healTypeAsset = ScriptableObject.CreateInstance<HealItemData>();
                if(data is HealItemData healItemData)
                {
                    healTypeAsset.healType = healItemData.healType;
                    healTypeAsset.healRatio = healItemData.healRatio;
                }
                return healTypeAsset;
            case ItemType.Upgrade:
                UpgradeItemData upgradeTypeAsset = ScriptableObject.CreateInstance<UpgradeItemData>();
                if (data is UpgradeItemData upgradeItemData)
                {
                    upgradeTypeAsset.upgradeType = upgradeItemData.upgradeType;
                }
                return upgradeTypeAsset;
            case ItemType.Gamble:
                GambleItemData gambleTypeAsset = ScriptableObject.CreateInstance<GambleItemData>();
                return gambleTypeAsset;
            default:
                Debug.LogError($"[CreateItemDataSO] 알 수 없는 ItemType: {data.itemType}");
                return null;
        }
    }
}