using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TestPlayer : MonoBehaviour, IBuffUsable
{
    public void ApplyBuff(BuffType buffType, float increse)
    {
        throw new System.NotImplementedException();
    }

    public float GetReturnValue(BuffType buffType)
    {
        throw new System.NotImplementedException();
    }

    public void ReturnBuffValue(BuffType buffType, float returnVal)
    {
        throw new System.NotImplementedException();
    }

    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            DataManager.Instance.BuffDatabase.buffDataList.Find((x) => x.buffName.Equals("ºÐ³ë")).BuffEffect(this);
        }
    }

}
