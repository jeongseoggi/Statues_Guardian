using UnityEngine;

public class AoERangeDrawer : MonoBehaviour
{
    public Vector2 radius = new Vector2(3.5f, 3.5f);
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, radius);
    }
}
