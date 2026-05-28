using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashUI : MonoBehaviour
{
    [Header("闪红参数")]
    public float fadeInDuration = 0.1f;
    public float holdDuration = 0.05f;
    public float fadeOutDuration = 0.6f;
    [Range(0f, 1f)]
    public float maxAlpha = 0.35f;

    private Image overlay;
    private Coroutine flashRoutine;

    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;
        }

        GameObject imgObj = new GameObject("DamageOverlay");
        imgObj.transform.SetParent(transform, false);

        overlay = imgObj.AddComponent<Image>();
        overlay.color = new Color(1, 0, 0, 0);
        overlay.raycastTarget = false;

        RectTransform rt = overlay.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float t = 0;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0, maxAlpha, t / fadeInDuration);
            overlay.color = new Color(1, 0, 0, a);
            yield return null;
        }

        overlay.color = new Color(1, 0, 0, maxAlpha);
        yield return new WaitForSeconds(holdDuration);

        t = 0;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(maxAlpha, 0, t / fadeOutDuration);
            overlay.color = new Color(1, 0, 0, a);
            yield return null;
        }

        overlay.color = new Color(1, 0, 0, 0);
    }
}
