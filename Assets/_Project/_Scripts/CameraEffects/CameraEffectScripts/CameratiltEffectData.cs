using System.Collections;
using _Project._Scripts.CameraEffects;
using _Project._Scripts.CameraEffects.Easing;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera Effects/Tilt")]

public class TiltEffectData : CameraEffectData
{
    public float tiltAngle = 10f;

    public override IEnumerator Execute(Camera camera)
    {
        Quaternion originalRotation = camera.transform.localRotation;
        Quaternion targetRotation   = originalRotation * Quaternion.Euler(0, 0, tiltAngle);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = EasingUtils.GetEasing(easeType, elapsed / duration);
            camera.transform.localRotation =  Quaternion.Lerp(originalRotation, targetRotation, t);
            elapsed                        += Time.deltaTime;
            yield return null;
        }

        camera.transform.localRotation = originalRotation;
    }
}