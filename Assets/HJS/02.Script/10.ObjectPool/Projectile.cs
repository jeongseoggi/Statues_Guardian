using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public SpriteRenderer sprite;
    public float damage;
    public float speed;
    bool isHit;

    /// <summary>
    /// 꺼내온 발사체를 대상에게 쏨
    /// </summary>
    public void Shoot()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShootOnTarget());
    }

    /// <summary>
    /// 발사체가 타겟에게 날아가는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator ShootOnTarget()
    {
        Vector3 dir = target.transform.position - transform.position;
        if (dir.x > 0)
            sprite.flipX = false;
        else
            sprite.flipX = true;

        while (isHit == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            yield return null;
        }
    }

    /// <summary>
    /// 타겟에 맞았을 때 데미지를 주면서 풀에 다시 들어감
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IHitable>() != null)
        {
            collision.gameObject.GetComponent<IHitable>().Hit(damage);
            isHit = true;
            ProjectileManager.instance.ReturnPool(this);
        }
    }

    private void OnDisable()
    {
        isHit = false;
    }
}
