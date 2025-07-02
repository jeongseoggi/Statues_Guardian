using UnityEngine;

public class StageEnterInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>() != null)
        {
            VillageUIManager.Instance.stageSelectWindow.SetWindow(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            VillageUIManager.Instance.stageSelectWindow.SetWindow(false);
        }
    }
}
