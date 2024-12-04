using System.Collections;
using _Project._Scripts.CameraEffects;
using _Project._Scripts.CameraEffects.Easing;
using UnityEngine;
[CreateAssetMenu(menuName = "Camera Effects/Screen flash")]

public class ScreenflashEffectData : CameraEffectData
{
    public Color flashColor = Color.white;
    public float maxAlpha   = 0.5f;

    public override IEnumerator Execute(Camera camera)
    {
        // Assume there's an overlay image set up on a Canvas
        CanvasGroup overlay = camera.GetComponentInChildren<CanvasGroup>();
        overlay.alpha = maxAlpha;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            overlay.alpha =  Mathf.Lerp(maxAlpha, 0, EasingUtils.GetEasing(easeType, elapsed / duration));
            elapsed       += Time.deltaTime;
            yield return null;
        }

        overlay.alpha = 0;
    }
}
