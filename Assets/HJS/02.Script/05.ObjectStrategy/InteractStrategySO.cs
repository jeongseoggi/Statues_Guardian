using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionType
{
    Box,
    Door
}

public abstract class InteractStrategySO : ScriptableObject
{
  
    public Dictionary<InteractionType, InteractableObject> typeByInteractionObject = new Dictionary<InteractionType, InteractableObject>();
    public InteractionType interactionType;

    /// <summary>
    /// ��ȣ�ۿ� ������ ������Ʈ�� �浹���� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="door"></param>
    /// <param name="doorAction"></param>
    public abstract void Interact(Player player, GameObject door, UnityAction<string> doorAction = null);


    public abstract void SetInteractionType();
}
