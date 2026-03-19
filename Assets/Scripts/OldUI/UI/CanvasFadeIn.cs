using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFadeIn : MonoBehaviour
{
    private void Awake()
    {
        var group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        StartCoroutine(FadeIn(group));
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        // wait for safe area to settle
        yield return null;
        yield return null;
        yield return new WaitForEndOfFrame();
        group.alpha = 1f;
    }
}
