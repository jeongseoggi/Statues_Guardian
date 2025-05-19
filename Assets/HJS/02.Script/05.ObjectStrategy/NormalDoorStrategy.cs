using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Strategy/NormalDoor")]
public class NormalDoorStrategy : InteractStrategySO
{
    public override void Interact(Player player, GameObject door, UnityAction<string> doorAction = null)
    {
        doorAction?.Invoke("던전 문을 여시겠습니까?");
    }

    public override void SetInteractionType()
    {
        interactionType = InteractionType.Door;
    }
}
