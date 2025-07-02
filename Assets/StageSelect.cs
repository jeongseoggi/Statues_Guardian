using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    #region private
    [SerializeField] private GameObject         lockObject;
    [SerializeField] private TextMeshProUGUI    stageNameTMP;
    [SerializeField] private Button             stageEnterBtn;
                     private int                stageIndex;
    #endregion


    public void Init(string stageName, int stageIndex)
    {
        stageNameTMP.text = stageName;
        this.stageIndex = stageIndex;

        if (GameManager.Instance.PlayerData.Stage < stageIndex)
        {
            lockObject.SetActive(true);
            stageEnterBtn.interactable = false;
        }
        else
        {
            lockObject.SetActive(false);
            stageEnterBtn.interactable = true;
        }
    }
}