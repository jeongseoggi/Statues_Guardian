using UnityEngine;

public class DamageTextManager : PoolableObject<DamageText>
{
    [SerializeField] Transform canvasform;

    public void Show(float dmg, Vector3 pos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        DamageText dmgText = SpawnPool();

        //풀링에서 꺼낸 프리펩 위치 조정 및 Init
        dmgText.gameObject.transform.SetParent(canvasform);
        dmgText.gameObject.transform.position = screenPos;
        dmgText.gameObject.SetActive(true);
        dmgText.Init(dmg);
    }
}
