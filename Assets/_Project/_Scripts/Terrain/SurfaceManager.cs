using UnityEngine;

namespace _Project._Scripts.Terrain
{
    public class SurfaceManager: MonoBehaviour
    {
        public static SurfaceManager Instance { get; private set; }
        [SerializeField] private LayerMask groundLayer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            CinemachinePOVExtension.OnFootstep += HandleFootstep;
        }

        private void OnDisable()
        {
            CinemachinePOVExtension.OnFootstep -= HandleFootstep;
        }

        public void HandleFootstep(Vector3 position)
        {
            if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 1.5f, groundLayer))
            {
                Surface surface = hit.collider.GetComponent<Surface>();
                if (surface != null)
                {
                    surface.PlayFootstep();
                }
            }
        }
    }
}