using _Project._Scripts.CameraEffects.Easing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.CameraEffects
{
    public abstract class CameraEffectComponent : MonoBehaviour
    {
        public CameraEffectData effectData;
        /*
        [Header("Override Settings")]
        public bool overrideDuration;
        [ShowIf("overrideDuration")] public float duration;
    
        public                              bool     overrideEaseType;
        [ShowIf("overrideEaseType")] public EaseType easeType;
        */
        protected Camera    targetCamera;
        protected Coroutine activeCoroutine;

        protected virtual void Awake()
        {
            targetCamera = GetComponent<Camera>();
        }

        public void Play()
        {
            if (activeCoroutine != null) Stop();
            activeCoroutine = StartCoroutine(effectData.Execute(targetCamera));
        }

        public void Stop()
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }
        }
    }
}