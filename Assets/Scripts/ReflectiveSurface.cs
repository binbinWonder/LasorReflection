using UnityEngine;

public enum ReflectiveFace
{
    Forward,
    Back,
    Up,
    Down,
    Right,
    Left,
    Custom
}

public class ReflectiveSurface : MonoBehaviour
{
    [Header("反射面设置")]
    public ReflectiveFace reflectiveFace = ReflectiveFace.Forward;

    [Tooltip("当选择 Custom 时，在本地空间定义的法线方向")]
    public Vector3 customLocalNormal = Vector3.up;

    [Tooltip("勾选后将使用射线命中点的实际表面法线（适用于斜面/曲面）")]
    public bool useSurfaceNormal = false;

    public Vector3 GetWorldNormal(Vector3? hitNormal = null)
    {
        if (useSurfaceNormal && hitNormal.HasValue)
            return hitNormal.Value;

        if (reflectiveFace == ReflectiveFace.Custom)
            return transform.TransformDirection(customLocalNormal.normalized);

        switch (reflectiveFace)
        {
            case ReflectiveFace.Forward:  return transform.forward;
            case ReflectiveFace.Back:    return -transform.forward;
            case ReflectiveFace.Up:      return transform.up;
            case ReflectiveFace.Down:    return -transform.up;
            case ReflectiveFace.Right:   return transform.right;
            case ReflectiveFace.Left:    return -transform.right;
            default:                     return transform.forward;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 normal = GetWorldNormal();
        Vector3 center = transform.position;
        Gizmos.DrawRay(center, normal * 0.5f);
        Gizmos.DrawWireSphere(center + normal * 0.5f, 0.05f);
    }
}
