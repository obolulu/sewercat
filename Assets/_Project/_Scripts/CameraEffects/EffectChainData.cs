using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.CameraEffects;
using _Project._Scripts.CameraEffects.Easing;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("Camera Effects/Camera Effect Chain")]
public class EffectChainData : MonoBehaviour
{
    [System.Serializable]
    public class ChainedEffect
    {
        public CameraEffectData effect;
        public float            delay = 0.5f;

    }

    //[ListDrawerSettings(Expanded = true, ShowIndexLabels = true, DraggableItems = true)]
    public List<ChainedEffect> effects = new List<ChainedEffect>();

    private Camera targetCamera;

    private void Awake()
    {
        targetCamera = Camera.main; // Ensure the correct camera is assigned.
    }
/*
    public void PlayChain(System.Action onComplete = null)
    {
        StartCoroutine(ExecuteChain(onComplete));
    }
    /*
    private IEnumerator ExecuteChain(System.Action onComplete)
    {
        foreach (var chainedEffect in effects)
        {
            if (chainedEffect.delay > 0)
                yield return new WaitForSeconds(chainedEffect.delay);

            if (chainedEffect.effect != null)
                yield return StartCoroutine(chainedEffect.effect.Execute(targetCamera));
        }

        onComplete?.Invoke();
    }
    */

/*
    private IEnumerator ExecuteChain(System.Action onComplete)
    {
        // List to keep track of active coroutines
        List<Coroutine> coroutines = new List<Coroutine>();

        // Start all effects concurrently
        foreach (var chainedEffect in effects)
        {
            if (chainedEffect.effect != null)
            {
                // Start each effect in a new coroutine and add it to the list
                coroutines.Add(StartCoroutine(chainedEffect.effect.Execute(targetCamera)));
            }
        }

        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }

        onComplete?.Invoke();


    }
    */
}