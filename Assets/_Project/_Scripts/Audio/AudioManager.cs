using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private Dictionary<string, EventInstance> activeInstances = new Dictionary<string, EventInstance>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(FMODEventSO eventSO, Vector3? position = null, params (string name, float value)[] parameters)
    {
        if (eventSO == null || eventSO.eventReference.IsNull)
        {
            Debug.LogWarning("Attempted to play null or invalid FMOD event!");
            return;
        }

        if (eventSO.isLooping)
        {
            PlayLoopingSound(eventSO, position, parameters);
        }
        else
        {
            PlayOneShot(eventSO, position, parameters);
        }
    }

    private void PlayOneShot(FMODEventSO eventSO, Vector3? position, params (string name, float value)[] parameters)
    {
        var instance = CreateInstance(eventSO, position);
        ApplyParameters(instance, eventSO.defaultParameters);
        ApplyParameters(instance, parameters);
        
        instance.start();
        instance.release();
    }

    private void PlayLoopingSound(FMODEventSO eventSO, Vector3? position, params (string name, float value)[] parameters)
    {
        string eventKey = eventSO.name;
        
        // Stop existing instance if it exists
        StopSound(eventKey);

        var instance = CreateInstance(eventSO, position);
        ApplyParameters(instance, eventSO.defaultParameters);
        ApplyParameters(instance, parameters);
        
        instance.start();
        activeInstances[eventKey] = instance;
    }

    private EventInstance CreateInstance(FMODEventSO eventSO, Vector3? position)
    {
        var instance = RuntimeManager.CreateInstance(eventSO.eventReference);
        
        if (eventSO.is3D && position.HasValue)
        {
            var attributes = RuntimeUtils.To3DAttributes(position.Value);
            instance.set3DAttributes(attributes);
            instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, eventSO.minDistance);
            instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, eventSO.maxDistance);
        }
        
        return instance;
    }

    private void ApplyParameters(EventInstance instance, FMODEventParameter[] parameters)
    {
        if (parameters == null) return;
        
        foreach (var param in parameters)
        {
            instance.setParameterByName(param.name, param.defaultValue);
        }
    }

    private void ApplyParameters(EventInstance instance, (string name, float value)[] parameters)
    {
        if (parameters == null) return;
        
        foreach (var (name, value) in parameters)
        {
            instance.setParameterByName(name, value);
        }
    }

    public void StopSound(string eventKey)
    {
        if (activeInstances.TryGetValue(eventKey, out EventInstance instance))
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
            activeInstances.Remove(eventKey);
        }
    }

    public void SetParameter(string eventKey, string parameterName, float value)
    {
        if (activeInstances.TryGetValue(eventKey, out EventInstance instance))
        {
            instance.setParameterByName(parameterName, value);
        }
    }

    private void OnDestroy()
    {
        foreach (var instance in activeInstances.Values)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
        activeInstances.Clear();
    }
}