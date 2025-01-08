using UnityEngine;

namespace _Project._Scripts.Audio.Footsteps
{
    public class FootstepSystem : MonoBehaviour
    {
        public static event System.Action<FMODEventSO, Vector3> OnPlaySound;

        public static void PlaySound(FMODEventSO soundEvent, Vector3 position)
        {
            OnPlaySound?.Invoke(soundEvent, position);
        }
    }

}