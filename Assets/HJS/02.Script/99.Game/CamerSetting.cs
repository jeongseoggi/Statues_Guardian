using Unity.Cinemachine;
using UnityEngine;

public class CamerSetting : MonoBehaviour
{
    #region private
    [SerializeField] CinemachineCamera cam;
    #endregion

    void Start()
    {
        if(GameManager.Instance.PlayerStatData != null)
        {
            SetFollow();
        }
        else
        {
            GameManager.OnPlayerReady += SetFollow;
        }
    }

    private void SetFollow()
    {
        cam.Follow = GameManager.Instance.Player.transform;
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerReady -= SetFollow;
    }
}
