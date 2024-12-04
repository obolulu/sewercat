using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.CameraEffects;
using UnityEngine;

public class CameraEffectsManager : MonoBehaviour
{
    private static CameraEffectsManager instance;
    public static CameraEffectsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraEffectsManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("Camera Effects Manager");
                    instance = go.AddComponent<CameraEffectsManager>();
                }
            }
            return instance;
        }
    }
    private Camera                        mainCamera;
    private Dictionary<string, Coroutine> activeEffects = new Dictionary<string, Coroutine>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance   = this;
        mainCamera = Camera.main;
    }

    public void PlayEffect(CameraEffectData effect, Action onComplete = null)
    {
        StartCoroutine(ExecuteEffect(effect, onComplete));
    }

    public void PlayChain(EffectChainData chain, Action onComplete = null)
    {
        StartCoroutine(ExecuteChain(chain, onComplete));
    }

    private IEnumerator ExecuteEffect(CameraEffectData effect, Action onComplete = null)
    {
        yield return StartCoroutine(effect.Execute(mainCamera));
        onComplete?.Invoke();
    }

    private IEnumerator ExecuteChain(EffectChainData chain, Action onComplete = null)
    {
        foreach (var chainedEffect in chain.effects)
        {
            if (chainedEffect.delay > 0)
                yield return new WaitForSeconds(chainedEffect.delay);

            //yield return StartCoroutine(chainedEffect.effect.Execute(mainCamera));
            StartCoroutine(chainedEffect.effect.Execute(mainCamera));
        }
        
        onComplete?.Invoke();
    }

    public void StopAllEffects()
    {
        foreach (var effect in activeEffects.Values)
        {
            if (effect != null)
                StopCoroutine(effect);
        }
        activeEffects.Clear();
        
        // Reset camera
        if (mainCamera != null)
        {
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.fieldOfView             = 60f;
        }
    }
}