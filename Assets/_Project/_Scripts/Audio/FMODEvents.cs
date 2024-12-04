using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public enum AudioType
{
    SFX,
    Music,
    Ambience,
    Voice,
    UI
}

[CreateAssetMenu(fileName = "New FMOD Event", menuName = "Audio/FMOD Event")]
public class FMODEventSO : ScriptableObject
{
    [Header("Event Settings")]
    public EventReference eventReference;
    public AudioType audioType;
    public bool      is3D      = true;
    public bool      isLooping = false;
    
    [Header("Default Parameters")]
    public FMODEventParameter[] defaultParameters;
    
    [Header("Optional Settings")]
    public float minDistance = 1f;
    public float maxDistance = 20f;
}


[System.Serializable]
public struct FMODEventParameter
{
    public string name;
    public float  defaultValue;
}