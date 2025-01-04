using _Project._Scripts.PlayerScripts.Weapons.ComboSystem;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.NewSystem
{
    public class ClawsComboWindowState: ComboStateBase
    {
        private float _windowTime;
        private AttackSO _currentAttack;
    
        public ClawsComboWindowState(ComboState key, ClawsComboStateMachine manager) : base(key, manager) { }
    
        public override void EnterState()
        {
            _windowTime = 0f;
            _currentAttack = Manager.ComboManager.CurrentAttack;
        }
    
        public override void ExitState() { }
    
        public override void UpdateState()
        {
            _windowTime += Time.deltaTime;
            Manager.ComboManager.UpdateComboTimer();
        }
    
        public override ComboState GetNextState()
        {
            if (_windowTime >= _currentAttack.attackData.comboWindowEnd - 
                _currentAttack.attackData.comboWindowStart)
            {
                return ComboState.Idle;
            }
        
            if (Manager.WeaponHandler.ConsumeAttackInput() && 
                Manager.ComboManager.CanContinueCombo())
            {
                return ComboState.Attack;
            }
        
            return StateKey;
        }
    }
}