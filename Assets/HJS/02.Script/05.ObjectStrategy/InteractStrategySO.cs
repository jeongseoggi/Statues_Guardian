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
    /// 상호작용 가능한 오브젝트에 충돌했을 때 호출되는 함수입니다.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="door"></param>
    /// <param name="doorAction"></param>
    public abstract void Interact(Player player, GameObject door, UnityAction<string> doorAction = null);


    public abstract void SetInteractionType();
}
