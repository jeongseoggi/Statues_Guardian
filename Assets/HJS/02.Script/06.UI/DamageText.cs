using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dmgText;
    public void Init(float dmg)
    { 
        dmgText.text = dmg > 0 ? dmg.ToString() : "Miss";
        transform.localScale = Vector3.one;
        dmgText.color = Color.red;

        transform.DOMoveY(30f, 0.5f).SetRelative();
        dmgText.DOFade(0f, 0.5f).OnComplete(() =>
        {
            DungeonUIManager.Instance.dmgManager.ReturnPool(this);
        });
    }
}