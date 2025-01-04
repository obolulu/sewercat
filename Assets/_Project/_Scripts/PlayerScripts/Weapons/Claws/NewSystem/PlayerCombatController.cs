using _Project._Scripts.PlayerScripts.Weapons.ComboSystem;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.NewSystem
{
    public class PlayerCombatController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform handsTransform; // Reference to the hands object

        private ClawsComboStateMachine _comboStateManager;
        private ComboWeaponHandler     _currentWeaponHandler;
        private GameObject             _currentWeaponObject;

        private void Awake()
        {
            // Get or add ComboStateManager
            _comboStateManager = GetComponent<ClawsComboStateMachine>();
            if (_comboStateManager == null)
            {
                _comboStateManager = gameObject.AddComponent<ClawsComboStateMachine>();
            }
        }

        public void EquipWeapon(GameObject weaponPrefab)
        {
            // Clean up old weapon if it exists
            if (_currentWeaponObject != null)
            {
                Destroy(_currentWeaponObject);
            }

            // Instantiate new weapon under hands
            _currentWeaponObject = Instantiate(weaponPrefab, handsTransform);

            // Get the weapon handler from the new weapon
            _currentWeaponHandler = _currentWeaponObject.GetComponent<ComboWeaponHandler>();

            // Update references in ComboStateManager
            _comboStateManager.Initialize(_currentWeaponHandler);
        }
    }
}