using UnityEngine;
using UnityEngine.U2D;

public class SpriteManager : SingleTon<SpriteManager>
{
    [SerializeField] SpriteAtlas itemSpriteAltas;
    [SerializeField] SpriteAtlas skillSpriteAltas;

    public Sprite GetItemSprite(string spriteName)
    {
        return itemSpriteAltas.GetSprite(spriteName);
    }

    public Sprite GetSkillSprite(string spriteName)
    {
        return skillSpriteAltas.GetSprite(spriteName);
    }
}
