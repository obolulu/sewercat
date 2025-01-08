using _Project._Scripts.Audio.Footsteps;
using UnityEngine;

namespace _Project._Scripts.Terrain
{
    public class Surface: MonoBehaviour
    {
        [SerializeField] private SurfaceTypeSO surfaceType;

        [Tooltip("The surface type effects for this terrain.")]
        public SurfaceTypeSO SurfaceEffects => surfaceType;

        public void PlayFootstep()
        {
            FootstepSystem.PlaySound(surfaceType.footstepSound, transform.position);
        }
        /*
        public FMODEventSO GetSoundEffect()
        {
            return surfaceType.footstepSound;
        }
        */
    }
}