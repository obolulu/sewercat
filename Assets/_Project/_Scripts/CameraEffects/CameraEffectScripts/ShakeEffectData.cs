using System.Collections;
using _Project._Scripts.CameraEffects.Easing;
using UnityEngine;

namespace _Project._Scripts.CameraEffects
{
    [CreateAssetMenu(menuName = "Camera Effects/Shake")]
    public class ShakeEffectData : CameraEffectData
    {
        public float   magnitude       = 0.5f;
        public Vector2 horizontalRange = new Vector2(-1f, 1f);
        public Vector2 verticalRange   = new Vector2(-1f, 1f);

        public override IEnumerator Execute(Camera camera)
        {
            Vector3 originalPosition = camera.transform.localPosition;
            float   elapsed          = 0f;

            while (elapsed < duration)
            {
                float strength = magnitude * (1 - EasingUtils.GetEasing(easeType, elapsed / duration));
                float x        = UnityEngine.Random.Range(horizontalRange.x, horizontalRange.y) * strength;
                float y        = UnityEngine.Random.Range(verticalRange.x, verticalRange.y) * strength;

                camera.transform.localPosition =  new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
                elapsed                        += Time.deltaTime;
                yield return null;
            }

            camera.transform.localPosition = originalPosition;
        }
    }
}