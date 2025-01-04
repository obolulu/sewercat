using _Project._Scripts.PlayerScripts.Weapons.ComboSystem;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.NewSystem
{
    public class ClawsComboStateMachine: StateManager<ComboState>
    {
        //[SerializeField] private ComboWeaponHandler weaponHandler;
        //[SerializeField] private ComboManager comboManager;
        private ComboWeaponHandler _weaponHandler;
        private ComboManager       _comboManager;

        public void Initialize(ComboWeaponHandler weaponHandler)
        {
            _weaponHandler                   = weaponHandler;
            _comboManager                    = _weaponHandler.GetComponent<ComboManager>();
            
            States[ComboState.Idle]          = new ClawsIdleState(ComboState.Idle, this);
            States[ComboState.Attack]        = new ClawsAttackState(ComboState.Attack, this);
            States[ComboState.SpecialAttack] = new ClawsSpecialAttackState(ComboState.SpecialAttack, this);
            States[ComboState.ComboWindow]   = new ClawsComboWindowState(ComboState.ComboWindow, this);

            
            CurrentState = States[ComboState.Idle];
        }
        public ComboWeaponHandler WeaponHandler => _weaponHandler;
        public ComboManager ComboManager => _comboManager;
        
    }
}