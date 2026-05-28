using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LaserMode
{
    Static,
    Moving,
    Swinging
}

public enum MoveAxis
{
    X_Axis,
    Y_Axis,
    Z_Axis
}

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : MonoBehaviour
{
    [Header("Mode")]
    public LaserMode mode = LaserMode.Static;

    [Header("Core")]
    public Transform firePoint;

    [Header("Movement (Moving mode)")]
    public MoveAxis moveDirection = MoveAxis.X_Axis;
    public float moveSpeed = 2.0f;
    public float moveDistance = 5.0f;
    private Vector3 startPos;
    private Quaternion startRotation;

    [Header("Swing (Swinging mode)")]
    public MoveAxis swingDirection = MoveAxis.Y_Axis;
    public float swingSpeed = 45.0f;
    public float swingAngle = 30.0f;

    [Header("Laser Settings")]
    public bool startActive = true;
    public float initialDelay = 0f;
    public bool isIntermittent = false;
    public float laserOnDuration = 1.0f;
    public float laserOffDuration = 1.0f;

    public float laserLength = 100f;
    public int maxBounces = 4;
    public int laserDamage = 1;
    public LayerMask hitMask;

    private LineRenderer lr;
    private bool isLaserActive;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        startPos = transform.position;
        startRotation = transform.rotation;

        if (firePoint == null)
        {
            Transform fp = transform.Find("FirePoint");
            if (fp != null) firePoint = fp;
        }

        lr.positionCount = 0;
        lr.enabled = false;
        isLaserActive = false;

        if (initialDelay > 0f)
            StartCoroutine(DelayedStart());
        else
            ActivateLaser();
    }

    void ActivateLaser()
    {
        if (isIntermittent)
        {
            StartCoroutine(IntermittentLaserRoutine());
        }
        else if (startActive)
        {
            isLaserActive = true;
            lr.enabled = true;
            lr.positionCount = 2;
        }
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(initialDelay);
        ActivateLaser();
    }

    void Update()
    {
        HandleMovement();
        UpdateLaser();
    }

    void HandleMovement()
    {
        if (mode == LaserMode.Moving)
        {
            float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            Vector3 dir = Vector3.zero;

            if (moveDirection == MoveAxis.X_Axis) dir = transform.right * offset;
            else if (moveDirection == MoveAxis.Y_Axis) dir = transform.up * offset;
            else if (moveDirection == MoveAxis.Z_Axis) dir = transform.forward * offset;

            transform.position = startPos + dir;
        }
        else if (mode == LaserMode.Swinging)
        {
            float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
            Quaternion rotOffset = Quaternion.identity;

            if (swingDirection == MoveAxis.X_Axis) rotOffset = Quaternion.Euler(angle, 0, 0);
            else if (swingDirection == MoveAxis.Y_Axis) rotOffset = Quaternion.Euler(0, angle, 0);
            else if (swingDirection == MoveAxis.Z_Axis) rotOffset = Quaternion.Euler(0, 0, angle);

            transform.rotation = startRotation * rotOffset;
        }
    }

    void UpdateLaser()
    {
        if (isLaserActive)
        {
            lr.enabled = true;

            if (firePoint != null)
            {
                List<Vector3> points = new List<Vector3>();
                points.Add(firePoint.position);

                Vector3 origin = firePoint.position;
                Vector3 direction = firePoint.forward;
                float traveledDistance = 0f;

                for (int i = 0; i <= maxBounces; i++)
                {
                    float remaining = laserLength - traveledDistance;
                    if (remaining <= 0f) break;

                    RaycastHit hit;
                    if (Physics.Raycast(origin, direction, out hit, remaining, hitMask))
                    {
                        float segmentDist = Vector3.Distance(origin, hit.point);
                        traveledDistance += segmentDist;
                        points.Add(hit.point);

                        PlayerHealth health = hit.collider.GetComponentInParent<PlayerHealth>();
                        if (health != null)
                        {
                            health.TakeDamage(laserDamage);
                        }

                        LaserDoor door = hit.collider.GetComponent<LaserDoor>();
                        if (door != null)
                        {
                            door.OnLaserHit(Time.deltaTime);
                        }

                        ReflectiveSurface reflector = hit.collider.GetComponent<ReflectiveSurface>();
                        if (reflector != null && i < maxBounces)
                        {
                            Vector3 normal = reflector.GetWorldNormal(hit.normal);
                            direction = Vector3.Reflect(direction, normal);
                            origin = hit.point + direction * 0.001f;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        points.Add(origin + direction * remaining);
                        break;
                    }
                }

                lr.positionCount = points.Count;
                lr.SetPositions(points.ToArray());
            }
        }
        else
        {
            if (lr.enabled)
            {
                lr.enabled = false;
                lr.positionCount = 0;
            }
        }
    }

    IEnumerator IntermittentLaserRoutine()
    {
        if (startActive)
        {
            isLaserActive = true;
            yield return new WaitForSeconds(laserOnDuration);
            isLaserActive = false;
        }

        while (true)
        {
            yield return new WaitForSeconds(laserOffDuration);
            isLaserActive = true;
            yield return new WaitForSeconds(laserOnDuration);
            isLaserActive = false;
        }
    }
}
