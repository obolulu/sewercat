using UnityEngine;
using FMODUnity;
using System;
using System.Collections.Generic;

namespace _Project._Scripts.Audio
{


    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
    public class AudioLibrary : ScriptableObject
    {
        public AudioCategory[] categories;

        private Dictionary<string, EventReference> eventDictionary;

        public void Initialize()
        {
            eventDictionary = new Dictionary<string, EventReference>();
            foreach (var category in categories)
            {
                foreach (var eventData in category.events)
                {
                    eventDictionary[eventData.eventName] = eventData.eventReference;
                }
            }
        }

        public EventReference GetEventReference(string eventName)
        {
            if (eventDictionary == null)
                Initialize();

            return eventDictionary.GetValueOrDefault(eventName);
        }

        public AudioEventData GetEventData(string eventName)
        {
            foreach (var category in categories)
            {
                var eventData = Array.Find(category.events, e => e.eventName == eventName);
                if (eventData != null)
                    return eventData;
            }

            return null;
        }
    }
}