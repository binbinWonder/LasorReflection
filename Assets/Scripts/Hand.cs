using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("抓取参数")]
    public float grabRange = 4f;
    public float holdDistance = 2f;
    public float throwForce = 8f;
    public LayerMask grabMask;

    [Header("按键")]
    public KeyCode grabKey = KeyCode.E;

    private Grabbable currentGrabbable;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 velocity;

    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        if (currentGrabbable != null)
            HandleHolding();
        else
            HandleLooking();
    }

    void HandleLooking()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, grabMask))
        {
            Grabbable g = hit.collider.GetComponentInParent<Grabbable>();
            if (g != null && Input.GetKeyDown(grabKey))
            {
                PickUp(g);
            }
        }
    }

    void HandleHolding()
    {
        if (Input.GetKeyDown(grabKey))
        {
            Drop();
            return;
        }

        Vector3 targetPos = transform.position + transform.forward * holdDistance;
        currentGrabbable.transform.position = Vector3.Lerp(
            currentGrabbable.transform.position, targetPos, Time.deltaTime * 20f);

        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        lastRotation = transform.rotation;

        if (Input.GetMouseButtonDown(0))
            Throw();
    }

    void PickUp(Grabbable g)
    {
        currentGrabbable = g;
        currentGrabbable.OnGrab(transform);
    }

    void Drop()
    {
        currentGrabbable.OnRelease(Vector3.zero);
        currentGrabbable = null;
    }

    void Throw()
    {
        Vector3 throwVelocity = transform.forward * throwForce;
        currentGrabbable.OnRelease(throwVelocity);
        currentGrabbable = null;
    }
}
