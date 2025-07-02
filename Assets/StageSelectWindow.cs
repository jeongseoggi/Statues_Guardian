using System.Collections.Generic;
using UnityEngine;

public class StageSelectWindow : MonoBehaviour
{
    #region private
    [SerializeField] private StageSelect stageSelectPrefab;
    [SerializeField] private GameObject prefabContent;
                     private List<StageSelect> stageSelectList;
    #endregion

    private void OnEnable()
    {
        if(stageSelectList != null && stageSelectList.Count > 0)
        {
            return;
        }
        else
        {
            stageSelectList = new List<StageSelect>();
            for(int i = 0; i < DataManager.Instance.StageDatabase.gameStageDataList.Count; i++)
            {
                StageSelect stageSelect = Instantiate(stageSelectPrefab, prefabContent.transform);
                stageSelect.Init(DataManager.Instance.StageDatabase.gameStageDataList[i].stageName,
                    DataManager.Instance.StageDatabase.gameStageDataList[i].stageID + 1);
                stageSelectList.Add(stageSelect);
            }
        }
    }

    public void SetWindow(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

}
