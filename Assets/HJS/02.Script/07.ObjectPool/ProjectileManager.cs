using UnityEngine;

public class ProjectileManager : PoolableObject<Projectile>
{
    public static ProjectileManager instance;

    private void Awake()
    {
        instance = this;
    }
}
