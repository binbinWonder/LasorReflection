using UnityEngine;

public class ReflectiveSurface : MonoBehaviour
{
    [Tooltip("反转法线方向（默认关闭，使用命中面的实际法线）")]
    public bool flipNormal;

    public Vector3 GetReflectionNormal(Vector3 hitNormal)
    {
        return flipNormal ? -hitNormal : hitNormal;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 normal = flipNormal ? -transform.forward : transform.forward;
        Vector3 center = transform.position;
        Gizmos.DrawRay(center, normal * 0.5f);
        Gizmos.DrawWireSphere(center + normal * 0.5f, 0.05f);
    }
}
