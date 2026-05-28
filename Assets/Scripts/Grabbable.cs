using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    [Header("抓取设置")]
    public bool disableGravityWhenHeld = true;
    public bool freezeRotationWhenHeld = true;

    [HideInInspector]
    public bool isHeld;

    private Rigidbody rb;
    private bool originalGravity;
    private RigidbodyConstraints originalConstraints;
    private Transform originalParent;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnGrab(Transform holder)
    {
        isHeld = true;
        originalParent = transform.parent;
        originalGravity = rb.useGravity;
        originalConstraints = rb.constraints;

        transform.SetParent(holder);
        rb.useGravity = false;

        if (freezeRotationWhenHeld)
            rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void OnRelease(Vector3 throwVelocity)
    {
        isHeld = false;
        transform.SetParent(originalParent);
        rb.useGravity = originalGravity;
        rb.constraints = originalConstraints;
        rb.velocity = throwVelocity;
    }
}
