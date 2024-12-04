using System.Collections;
using _Project._Scripts.CameraEffects.Easing;
using UnityEngine;

namespace _Project._Scripts.CameraEffects
{
    public abstract class CameraEffectData : ScriptableObject
    {
        public          float       duration = 0.5f;
        public          EaseType    easeType = EaseType.Linear;
        public abstract IEnumerator Execute(Camera camera);
    }
}