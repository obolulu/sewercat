using System.Collections.Generic;
using _Project._Scripts.Audio;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [SerializeField] private AudioLibrary audioLibrary;
    private Dictionary<string, EventInstance> activeInstances;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        activeInstances = new Dictionary<string, EventInstance>();
        audioLibrary.Initialize();
    }

    public void PlaySound(string eventName, Vector3? position = null, params (string name, float value)[] parameters)
    {
        Debug.Log($"Playing sound: {eventName}");
        var eventData = audioLibrary.GetEventData(eventName);
        if (eventData == null)
        {
            Debug.LogWarning($"Audio event '{eventName}' not found!");
            return;
        }

        if (eventData.isLooping)
        {
            PlayLoopingSound(eventName, position, parameters);
        }
        else
        {
            Debug.Log("Playing one shot sound");
            PlayOneShot(eventName, position, parameters);
        }
    }

    private void PlayOneShot(string eventName, Vector3? position, params (string name, float value)[] parameters)
    {
        var eventRef = audioLibrary.GetEventReference(eventName);
        var instance = RuntimeManager.CreateInstance(eventRef);

        if (position.HasValue)
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position.Value));

        foreach (var (name, value) in parameters)
            instance.setParameterByName(name, value);

        instance.start();
        instance.release();
    }

    private void PlayLoopingSound(string eventName, Vector3? position, params (string name, float value)[] parameters)
    {
        if (activeInstances.ContainsKey(eventName))
        {
            StopSound(eventName);
        }

        var eventRef = audioLibrary.GetEventReference(eventName);
        var instance = RuntimeManager.CreateInstance(eventRef);

        if (position.HasValue)
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(position.Value));

        foreach (var (name, value) in parameters)
            instance.setParameterByName(name, value);

        instance.start();
        activeInstances[eventName] = instance;
    }

    public void StopSound(string eventName)
    {
        if (activeInstances.TryGetValue(eventName, out EventInstance instance))
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
            activeInstances.Remove(eventName);
        }
    }

    public void SetParameter(string eventName, string parameterName, float value)
    {
        if (activeInstances.TryGetValue(eventName, out EventInstance instance))
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