using UnityEngine;
using UnityEngine.SceneManagement;

public class GambleItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData, int useCount = 1)
    {
        //마을이면 리턴 
        if(SceneManager.GetActiveScene().name.Equals("VillageScene"))
        {
            PopupManager.Instance.noticePopup.Init(GameString.DO_NOT_USE_THIS_SCENE, () => { PopupManager.Instance.noticePopup.Close(); }, true);
            return;
        }

        //모든 버프가 활성화 되어 있으면 리턴
        if (UIManager.Instance.buffManager.activeBuffWindowList.Count >= DataManager.Instance.BuffDatabase.buffDataList.Count)
        {
            UIManager.Instance.SetWarningText(GameString.ALL_USE_BUFF);
            return;
        }

        //연출중이면 리턴
        if(DungeonUIManager.Instance.randomBuffUI.IsPlaying)
        {
            return;
        }


        if (user is IBuffUsable buffUser)
        {
            GetRandomBuffId(buffUser);
        }

        GameManager.Instance.PlayerInventoryData.UseItem(itemData, useCount);
        SoundManager.Instance.SpawnPool().Play(DataManager.Instance.GetAudioClip("UseItem"));
    }

    public void GetRandomBuffId(IBuffUsable buffUser)
    {
        while(true)
        {
            //랜덤 버프 데이터 가져오기
            BuffData data = DataManager.Instance.BuffDatabase.buffDataList
                [UnityEngine.Random.Range(0, DataManager.Instance.BuffDatabase.buffDataList.Count)];

            //활성화 된 버프가 아니라면 적용
            if (!UIManager.Instance.buffManager.activeBuffWindowList.Find((x) => x.buffName.Equals(data.buffName)))
            {
                DungeonUIManager.Instance.randomBuffUI.Init(SpriteManager.Instance.GetBuffSprite(data.spriteName),  
                    data.buffName, data.buffDesc,
                () =>
                {
                    DataManager.Instance.BuffDatabase.buffDataList[data.buffID].BuffEffect(buffUser);
                });
                break;
            }
        }
    }
}
