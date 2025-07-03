using Unity.Cinemachine;
using UnityEngine;

public class CamerSetting : MonoBehaviour
{
    void Start()
    {
        GetComponent<CinemachineCamera>().Follow = GameManager.Instance.Player.transform;
    }
}
