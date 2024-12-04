using UnityEngine;

namespace _Project._Scripts.CameraEffects.Easing
{
    public class EasingUtils
    {
        public static float GetEasing(EaseType type, float t)
        {
            return type switch
            {
                EaseType.Linear         => t,
                EaseType.EaseInOut      => EaseInOut(t),
                EaseType.EaseOutBounce  => EaseOutBounce(t),
                EaseType.EaseOutElastic => EaseOutElastic(t),
                EaseType.EaseInBack     => EaseInBack(t),
                EaseType.EaseOutExpo    => EaseOutExpo(t),
                _                       => t
            };

        }

        public static float EaseInOut(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

        public static float EaseOutBounce(float t)
        {
            if (t < 1          / 2.75f)
                return 7.5625f * t * t;
            else if (t < 2     / 2.75f)
                return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
            else if (t < 2.5 / 2.75f)
                return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
            else
                return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
        }

        public static float EaseOutElastic(float t)
        {
            float p = 0.3f;
            return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - p / 4) * (2 * Mathf.PI) / p) + 1;
        }

        public static float EaseInBack(float t)
        {
            float c1 = 1.70158f;
            return c1 * t * t * t - c1 * t * t;
        }

        public static float EaseOutExpo(float t)
        {
            return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
        }
    }
}