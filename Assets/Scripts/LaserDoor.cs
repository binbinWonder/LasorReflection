using UnityEngine;

public class LaserDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float requiredTime = 5f;
    public float decayRate = 0.5f;
    public Color chargedColor = Color.green;

    private float charge;
    private bool isHitThisFrame;
    [HideInInspector]
    public bool isComplete;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    public void OnLaserHit(float deltaTime)
    {
        if (isComplete) return;
        isHitThisFrame = true;
    }

    void Update()
    {
        if (isComplete) return;

        if (isHitThisFrame)
        {
            charge += Time.deltaTime;
            if (charge >= requiredTime)
            {
                charge = requiredTime;
                isComplete = true;
                Debug.Log("Clear!");
            }
        }
        else if (charge > 0)
        {
            charge -= decayRate * Time.deltaTime;
            if (charge < 0) charge = 0;
        }

        if (rend != null)
            rend.material.color = Color.Lerp(originalColor, chargedColor, charge / requiredTime);

        isHitThisFrame = false;
    }
}
