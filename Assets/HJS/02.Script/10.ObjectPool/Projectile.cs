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
    /// ������ �߻�ü�� ��󿡰� ��
    /// </summary>
    public void Shoot()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShootOnTarget());
    }

    /// <summary>
    /// �߻�ü�� Ÿ�ٿ��� ���ư��� �ڷ�ƾ
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
    /// Ÿ�ٿ� �¾��� �� �������� �ָ鼭 Ǯ�� �ٽ� ��
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
