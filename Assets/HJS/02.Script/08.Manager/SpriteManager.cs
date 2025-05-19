using UnityEngine;
using UnityEngine.U2D;

public class SpriteManager : SingleTon<SpriteManager>
{
    [SerializeField] SpriteAtlas spriteAltas;

    public Sprite GetSprite(string spriteName)
    {
        return spriteAltas.GetSprite(spriteName);
    }
}
