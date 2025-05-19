using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public InteractStrategySO interactStrategySO;
    public SpriteRenderer doorSprite;
    UnityAction<string> interactAction;

    [Header("NormalDoor")]
    public InteractableObject[] interactionObj;

    private void Start()
    {
        
    }

    public void Interact(Player player)
    {
        interactStrategySO?.Interact(player, this.gameObject, interactAction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(interactStrategySO.interactionType == InteractionType.Door)
        {
            interactAction += ShowPopUp;
        }
        Player player;
        if(collision.collider.TryGetComponent<Player>(out player))
        {
            Interact(player);
        }
    }

    public void ShowPopUp(string text)
    {
        //if(!PopupManager.Instance.CheckActive())
        //{
        //    PopupManager.Instance.Init(text, () =>
        //    {
        //        interactionObj[0].StartDoorAction();
        //        interactionObj[1].StartDoorAction();
        //        PopupManager.Instance.gameObject.SetActive(false);
        //    },
        //    ()=>
        //    {
        //        PopupManager.Instance.gameObject.SetActive(false);
        //    });
        //}
    }

    public void StartDoorAction()
    {
        StartCoroutine(StartDoorActionCo(GetComponent<BoxCollider2D>()));
    }

    IEnumerator StartDoorActionCo(BoxCollider2D boxCollider)
    {
        Color originColor = doorSprite.color;
        while(originColor.a > 0)
        {
            originColor.a -= 0.05f;
            doorSprite.color = originColor;
            yield return new WaitForSeconds(0.1f);
        }
        boxCollider.enabled = false;

    }
}
