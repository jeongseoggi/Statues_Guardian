using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Strategy/NormalDoor")]
public class NormalDoorStrategy : InteractStrategySO
{
    public override void Interact(Player player, GameObject door, UnityAction<string> doorAction = null)
    {
        doorAction?.Invoke("���� ���� ���ðڽ��ϱ�?");
    }

    public override void SetInteractionType()
    {
        interactionType = InteractionType.Door;
    }
}
