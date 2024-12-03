namespace _Project._Scripts.Audio
{
    using FMODUnity;
    using System;
    
    [Serializable]
    public class AudioEventData
    {
        public string         eventName;
        public EventReference eventReference;
        public bool           is3D      = true;
        public bool           isLooping = false;
    }
    
    [Serializable]
    public class AudioCategory
    {
        public string           categoryName;
        public AudioEventData[] events;
    }
}