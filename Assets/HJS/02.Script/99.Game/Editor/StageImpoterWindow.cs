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
using UnityEngine.Experimental.AI;
using Unity.VisualScripting;

public class StageImpoterWindow : EditorWindow
{
    private string sheetUrl = "https://script.google.com/macros/s/AKfycbxL_PJlFo4U4ko1xq14aEVnyYbS3OLwRI8EpZkqHZ-AdK1J7jMCoQSFNYYWqUu1SLyvwg/exec";
    private static StageDataList stageDataList;
    private static ItemScriptableObject itemDataList;
    private static SkillDataList skillDataList;

    private static string directory = "Assets/HJS/06.SciptableObject/";
    private string[] options = { "스테이지", "아이템" , "스킬" };
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
        if (selectedIndex == 0)
            type = "stage";
        else if (selectedIndex == 1)
            type = "item";
        else
            type = "skill";

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
        else if(selectedIndex == 1)
        {
            GetItemDataList();
            JSONNode jsonData = JSONNode.Parse(json);
            List<ItemData> itemDataList = JsonParseItemData(jsonData);

            foreach (var item in itemDataList)
            {
                CreateItemDataSO(item);
            }
        }
        else
        {
            GetSkillDataList();
            JSONNode jsonData = JSONNode.Parse(json);
            List<SkillData> skillDataList = JsonParseSkillData(jsonData);

            foreach (var skill in skillDataList)
            {
                CreateSkillDataSO(skill);
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

    public static void CreateSkillDataSO(SkillData data)
    {
        CreateOrLoadSO<SkillData>(data.assetName, "Skill",
            () =>
            {
                if(data is PassiveSkill)
                {
                    return ScriptableObject.CreateInstance<PassiveSkill>();
                }
                else
                {
                    return ScriptableObject.CreateInstance<ActiveSkill>();
                }
            },
            (so)=>
            {
                so.skillName = data.skillName;
                so.skillDescription = data.skillDescription;
                so.coolTime = data.coolTime;
                so.mpCost = data.mpCost;
                so.spriteName = data.spriteName;
                so.skillType = data.skillType;
                so.damageType = data.damageType;
                so.damage = data.damage;
                so.duration = data.duration;
                so.increase = data.increase;
                so.assetName = data.assetName;
            },
            skillDataList.skillDatas,
            skillDataList);
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
                    AddItemDataList(ScriptableObject.CreateInstance<HealItemData>(), itemDataList, json, i);
                    break;
                case ItemType.Upgrade:
                    AddItemDataList(ScriptableObject.CreateInstance<UpgradeItemData>(), itemDataList, json, i);
                    break;
                case ItemType.Gamble:
                    AddItemDataList(ScriptableObject.CreateInstance<GambleItemData>(), itemDataList, json, i);
                    break;
                case ItemType.Buff:
                    break;
            }
        }
        return itemDataList;
    }

    public List<SkillData> JsonParseSkillData(JSONNode json)
    {
        List<SkillData> skillDataList = new List<SkillData>();

        for (int i = 0; i < json.Count; i++)
        {
            switch ((SkillType)json[i]["SkillType"].AsInt)
            {
                case SkillType.Passive:
                    AddSkillDataList(ScriptableObject.CreateInstance<PassiveSkill>(), skillDataList, json, i);
                    break;
                case SkillType.Active:
                    AddSkillDataList(ScriptableObject.CreateInstance<ActiveSkill>(), skillDataList, json, i);
                    break;
            }
        }

        return skillDataList;
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

    public void GetSkillDataList()
    {
        string path = $"{directory}/{type}/SkillDataList.asset";


        if (!File.Exists(path))
        {
            var asset = ScriptableObject.CreateInstance<SkillDataList>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            skillDataList = asset;
        }
        else
        {
            skillDataList = AssetDatabase.LoadAssetAtPath<SkillDataList>(path);
        }
        skillDataList.Initalize();
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
        itemData.Init(
            jsonData[index]["ItemName"], 
            jsonData[index]["Description"], 
            jsonData[index]["Price"].AsInt,
            jsonData[index]["SpriteName"], 
            (ItemType)jsonData[index]["ItemType"].AsInt);


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

    public void AddSkillDataList(SkillData skillData, List<SkillData> skillDatas, JSONNode jsonData, int index)
    {
        Debug.Log(jsonData[index]["Damage"].AsFloat);
        skillData.Init(
            jsonData[index]["SkillName"],
            jsonData[index]["SkillDescription"],
            jsonData[index]["CoolTime"].AsInt,
            jsonData[index]["MpCost"].AsInt,
            jsonData[index]["Damage"].AsFloat,
            jsonData[index]["Duration"].AsFloat,
            jsonData[index]["Increase"].AsFloat,
            jsonData[index]["SpriteName"],
            jsonData[index]["AssetName"],
            (SkillType)jsonData[index]["SkillType"].AsInt,
            (DamageType)jsonData[index]["DamageType"].AsInt
            );

        skillDatas.Add(skillData);
    }
}