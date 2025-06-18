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
using System.Linq;

public class StageImpoterWindow : EditorWindow
{
    private string sheetUrl = "https://script.google.com/macros/s/AKfycbxL_PJlFo4U4ko1xq14aEVnyYbS3OLwRI8EpZkqHZ-AdK1J7jMCoQSFNYYWqUu1SLyvwg/exec";
    private static StageDataList stageDataList;
    private static ItemScriptableObject itemDataList;
    private static SkillDataList skillDataList;
    private static BuffDataContainer buffDataContainer;
    private static RewardContainer rewardDataContainer;

    private static string directory = "Assets/HJS/06.SciptableObject/";
    private string[] options = { "스테이지", "아이템" , "스킬", "버프", "보상" };
    private int selectedIndex = 0;
    private static string type;


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
        {
            type = "stage";
        }
        else if (selectedIndex == 1)
        {
            type = "item";
        }
        else if (selectedIndex == 2)
        {
            type = "skill";
        }
        else if (selectedIndex == 3)
        {
            type = "buff";
        }
        else if(selectedIndex == 4)
        {
            type = "reward";
        }

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
            stageDataList = GetDataList<StageDataList>("StageDataList",
                (path) =>
                {
                    var asset = ScriptableObject.CreateInstance<StageDataList>();
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                    return asset;
                },
                (path) =>
                {
                    return AssetDatabase.LoadAssetAtPath<StageDataList>(path);
                });
            stageDataList.Initalize();

            JSONNode jsonData = JSONNode.Parse(json);
            List<StageData> parseList = JsonParseData(jsonData,
                () => 
                {
                    List<StageData> stageDataList = new List<StageData>();

                    for (int i = 0; i < jsonData.Count; i++)
                    {
                        StageData data = new StageData();
                        data.stageID = jsonData[i]["StageID"].AsInt;
                        data.stageName = jsonData[i]["StageName"];
                        data.totalWave = jsonData[i]["TotalWave"];

                        //웨이브 별 몬스터 개수 저장
                        string monsterStr = jsonData[i]["MonstersPerWave"];
                        data.monstersPerWave = ParsingDataCovertArray<int>(monsterStr);

                        //스테이지에 등장하는 몬스터 타입 설정
                        string monsterType = jsonData[i]["SpawnMonsterType"];
                        data.spawnMonsterType = ParsingDataCovertArray<int>(monsterType);

                        stageDataList.Add(data);
                        
                    }
                    return stageDataList;
                });

            foreach (var stage in parseList)
            {
                CreateStageWaveSO(stage);
            }
        }
        else if(selectedIndex == 1)
        {
            itemDataList = GetDataList<ItemScriptableObject>("ItemDataList",
                (path)=>
                {
                    var asset = ScriptableObject.CreateInstance<ItemScriptableObject>();
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                    return asset;
                },
                (path)=>
                {
                    return AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
                });
            itemDataList.Initalize();

            JSONNode jsonData = JSONNode.Parse(json);
            List<ItemData> parseList = JsonParseData(jsonData,
                () =>
                {
                    List<ItemData> itemDataList = new List<ItemData>();

                    for (int i = 0; i < jsonData.Count; i++)
                    {
                        switch ((ItemType)jsonData[i]["ItemType"].AsInt)
                        {
                            case ItemType.Heal:
                                AddItemDataList(ScriptableObject.CreateInstance<HealItemData>(), itemDataList, jsonData, i);
                                break;
                            case ItemType.Upgrade:
                                AddItemDataList(ScriptableObject.CreateInstance<UpgradeItemData>(), itemDataList, jsonData, i);
                                break;
                            case ItemType.Buff:
                                AddItemDataList(ScriptableObject.CreateInstance<BuffItemData>(), itemDataList, jsonData, i);
                                break;
                            case ItemType.Gamble:
                                AddItemDataList(ScriptableObject.CreateInstance<GambleItemData>(), itemDataList, jsonData, i);
                                break;
                        }
                    }
                    return itemDataList;
                });

            foreach (var item in parseList)
            {
                CreateItemDataSO(item);
            }
        }
        else if(selectedIndex == 2)
        {
            skillDataList = GetDataList<SkillDataList>("SkillDataList",
                (path)=>
                {
                    var asset = ScriptableObject.CreateInstance<SkillDataList>();
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                    return asset;
                },
                (path)=>
                {
                    return AssetDatabase.LoadAssetAtPath<SkillDataList>(path);
                });
            skillDataList.Initalize();

            JSONNode jsonData = JSONNode.Parse(json);
            List<SkillData> parseList = JsonParseData(jsonData,
                () =>
                {
                    List<SkillData> skillDataList = new List<SkillData>();

                    for (int i = 0; i < jsonData.Count; i++)
                    {
                        switch ((SkillType)jsonData[i]["SkillType"].AsInt)
                        {
                            case SkillType.Passive:
                                AddSkillDataList(ScriptableObject.CreateInstance<PassiveSkill>(), skillDataList, jsonData, i);
                                break;
                            case SkillType.Active:
                                AddSkillDataList(ScriptableObject.CreateInstance<ActiveSkill>(), skillDataList, jsonData, i);
                                break;
                        }
                    }

                    return skillDataList;
                });

            foreach (var skill in parseList)
            {
                CreateSkillDataSO(skill);
            }
        }
        else if(selectedIndex == 3)
        {
            buffDataContainer = GetDataList<BuffDataContainer>("BuffDataList",
                (path) =>
                {
                    var asset = ScriptableObject.CreateInstance<BuffDataContainer>();
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                    return asset;
                },
                (path) =>
                {
                    return AssetDatabase.LoadAssetAtPath<BuffDataContainer>(path);
                });
            buffDataContainer.Initalize();

            JSONNode jsonData = JSONNode.Parse(json);
            List<BuffData> parseList = JsonParseData(jsonData,
                () =>
                {
                    List<BuffData> buffDataList = new List<BuffData>();
                    for (int i = 0; i < jsonData.Count; i++)
                    {
                        BuffData bfData = new BuffData();
                        bfData.buffID = jsonData[i]["BuffID"].AsInt;
                        bfData.buffName = jsonData[i]["BuffName"];
                        bfData.buffDesc = jsonData[i]["BuffDescription"];
                        bfData.duration = jsonData[i]["Duration"].AsFloat;
                        bfData.increase = jsonData[i]["increase"].AsFloat;
                        bfData.spriteName = jsonData[i]["SpriteName"];
                        string buffTypeStr = jsonData[i]["BuffType"];
                        bfData.buffEfeects = ParsingDataCovertArray<BuffType>(buffTypeStr).ToList();


                        buffDataList.Add(bfData);
                    }
                    return buffDataList;
                });

            foreach (var buffData in parseList)
            {
                CreateBuffDataSO(buffData);
            }
        }
        else if (selectedIndex == 4)
        {
            rewardDataContainer = GetDataList<RewardContainer>("RewardDataList",
                (path) =>
                {
                    var asset = ScriptableObject.CreateInstance<RewardContainer>();
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                    return asset;
                },
                (path) =>
                {
                    return AssetDatabase.LoadAssetAtPath<RewardContainer>(path);
                });

            rewardDataContainer.Initalize();

            JSONNode jsonData = JSONNode.Parse(json);

            List<RewardData> parseList = JsonParseData(jsonData,
                () =>
                {
                    List<RewardData> rewardDataList = new List<RewardData>();
                    for (int i = 0; i < jsonData.Count; i++)
                    {
                        RewardData rewardData = ScriptableObject.CreateInstance<RewardData>();
                        rewardData.Init(
                            jsonData[i]["RewardID"].AsInt,
                            jsonData[i]["StageID"].AsInt,
                            jsonData[i]["RewardName"],
                            jsonData[i]["Gold"].AsInt
                            );

                        string itemIDStr = jsonData[i]["ItemID"];
                        rewardData.itemIDs = ParsingDataCovertArray<int>(itemIDStr).ToList();

                        string amountStr = jsonData[i]["Amount"];
                        rewardData.amounts = ParsingDataCovertArray<int>(amountStr).ToList();

                        rewardDataList.Add(rewardData);
                    }
                    return rewardDataList;
                });




            foreach (var rewardData in parseList)
            {
                CreateRewardDataSO(rewardData);
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
                so.stageID = data.stageID;
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
        CreateOrLoadSO<ItemData>(data.itemName, "Item",
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
            else if(data is BuffItemData)
            {
                return ScriptableObject.CreateInstance<BuffItemData>();
            }
            else
            {
                return ScriptableObject.CreateInstance<GambleItemData>();
            }
        },
        (so) =>
        {
            so.itemID = data.itemID;
            so.itemName = data.itemName;
            so.itemDesc = data.itemDesc;
            so.itemType = data.itemType;
            so.itemUseStrategy = data.itemUseStrategy;
            so.price = data.price;
            so.spriteName = data.spriteName;

            if(so is HealItemData healItem)
            {
                healItem.healRatio = (data as HealItemData).healRatio;
                healItem.healType = (data as HealItemData).healType;
            }
            if(so is BuffItemData buffItem)
            {
                buffItem.buffIds = (data as BuffItemData).buffIds;
            }
            if(so is UpgradeItemData upgradeItem)
            {
                upgradeItem.upgradeType = (data as UpgradeItemData).upgradeType;
            }
        },
        itemDataList.itemData,
        itemDataList);


    }

    /// <summary>
    /// 스킬 데이터 추가 함수
    /// </summary>
    /// <param name="data"></param>
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
                so.skillID = data.skillID;
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
                so.damagePerLevel = data.damagePerLevel;
                so.increasePerLevel = data.increasePerLevel;
                so.skillMasterLevel = data.skillMasterLevel;
                so.assetName = data.assetName;
            },
            skillDataList.skillDatas,
            skillDataList);
    }

    /// <summary>
    /// 버프 데이터 추가 함수
    /// </summary>
    /// <param name="data"></param>
    public static void CreateBuffDataSO(BuffData data)
    {
        CreateOrLoadSO<BuffData>(data.buffName, "Buff",
            () => ScriptableObject.CreateInstance<BuffData>(),
            (so) =>
            {
                so.buffID = data.buffID;
                so.buffName = data.buffName;
                so.buffDesc = data.buffDesc;
                so.duration = data.duration;
                so.increase = data.increase;
                so.buffEfeects = data.buffEfeects;
                so.spriteName= data.spriteName;
            },
            buffDataContainer.buffDataList,
            buffDataContainer);

    }

    public static void CreateRewardDataSO(RewardData data)
    {
        CreateOrLoadSO<RewardData>(data.rewardDataName, "Reward",
            () => ScriptableObject.CreateInstance<RewardData>(),
            (so) =>
            {
                so.rewardID = data.rewardID;
                so.rewardDataName = data.rewardDataName;
                so.stageID = data.stageID;
                so.itemIDs = data.itemIDs;
                so.amounts = data.amounts;
                so.gold = data.gold;
            },
            rewardDataContainer.rewardList,
            rewardDataContainer);
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
            T pasteAsset = AssetDatabase.LoadAssetAtPath<T>(itemPath);
            if (pasteAsset != null)
            {
                Debug.LogWarning($"[SO Importer] 기존 파일 덮어씌움: {itemPath}");
                copyData?.Invoke(pasteAsset);

                EditorUtility.SetDirty(pasteAsset);
                AssetDatabase.SaveAssets();

                addList.Add(pasteAsset);
                EditorUtility.SetDirty(soListClass);
                return;
            }
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
    public static List<T> JsonParseData<T>(JSONNode json, Func<List<T>> parseAction) where T : class
    {
        List<T> parselist = parseAction?.Invoke();
        return parselist;
    }

    /// <summary>
    /// 배열 형태로 파싱해서 리턴 해주는 함수
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public T[] ParsingDataCovertArray<T>(string data)
    {
        data = data.Trim('"'); // 앞뒤 따옴표 제거

        string[] split = data.Split(',');

        T[] covertArray = new T[split.Length];
        for (int j = 0; j < split.Length; j++)
        {
            object value = null;
            if(typeof(T) == typeof(int))
            {
                value = int.Parse(split[j]);
            }
            else
            {
                value = (BuffType)(int.Parse(split[j]));
            }
            covertArray[j] = (T)value;
        }

        return covertArray;
    }

    /// <summary>
    ///  데이터 리스트 스크립터블 오브젝트 가져오기(없으면 생성)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <param name="fileNotExistsFunc"></param>
    /// <param name="fileExistsFunc"></param>
    /// <returns></returns>
    public static T GetDataList<T>(string assetName, Func<string, T> fileNotExistsFunc, Func<string, T> fileExistsFunc) where T : class
    {
        string path = $"{directory}/{type}/{assetName}.asset";

        if(!File.Exists(path))
        {
            return fileNotExistsFunc?.Invoke(path);
        }
        else
        {
            return fileExistsFunc?.Invoke(path);
        }
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
            jsonData[index]["ItemID"].AsInt,
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
            upgradeItemData.upgradeType = (UpgradeType)jsonData[index]["UpgradeType"].AsInt;
            itemDataList.Add(upgradeItemData);
        }
        else if(itemData is BuffItemData buffItemData)
        {
            buffItemData.buffIds = jsonData[index]["BuffId"].AsInt;
            itemDataList.Add(buffItemData);
        }
        else if(itemData is GambleItemData gambleItemData)
        {
            itemDataList.Add(gambleItemData);
        }
    }

    public void AddSkillDataList(SkillData skillData, List<SkillData> skillDatas, JSONNode jsonData, int index)
    {
        Debug.Log(jsonData[index]["Damage"].AsFloat);
        skillData.Init(
            jsonData[index]["SkillID"].AsInt,
            jsonData[index]["SkillName"],
            jsonData[index]["SkillDescription"],
            jsonData[index]["CoolTime"].AsInt,
            jsonData[index]["MpCost"].AsInt,
            jsonData[index]["Damage"].AsFloat,
            jsonData[index]["Duration"].AsFloat,
            jsonData[index]["Increase"].AsFloat,
            jsonData[index]["DamagePerLevel"].AsFloat,
            jsonData[index]["IncreasePerLevel"].AsFloat,
            jsonData[index]["MasterLevel"].AsInt,
            jsonData[index]["SpriteName"],
            jsonData[index]["AssetName"],
            (SkillType)jsonData[index]["SkillType"].AsInt,
            (DamageType)jsonData[index]["DamageType"].AsInt
            );

        skillDatas.Add(skillData);
    }
}