using UnityEngine;

namespace _Project._Scripts.Terrain
{
    [CreateAssetMenu(fileName = "SurfaceType", menuName = "Audio/SurfaceType")]
    public class SurfaceTypeSO: ScriptableObject
    {
        public FMODEventSO footstepSound;
        public FMODEventSO jumpSound;
        public FMODEventSO landSound;
    }
}