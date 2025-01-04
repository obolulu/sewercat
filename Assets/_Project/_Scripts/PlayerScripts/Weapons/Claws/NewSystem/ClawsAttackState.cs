using _Project._Scripts.PlayerScripts.Weapons.ComboSystem;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.NewSystem
{
    public class ClawsAttackState : ComboStateBase
    {
        private float _stateTime;
        private AttackSO _currentAttack;
    
        public ClawsAttackState(ComboState key, ClawsComboStateMachine manager) : base(key, manager) { }
    
        public override void EnterState()
        {
            _stateTime = 0f;
        
            if (!Manager.ComboManager.IsInCombo)
            {
                // Start new combo with first attack in chain
                if (Manager.ComboManager.CurrentComboChain != null && 
                    Manager.ComboManager.CurrentComboChain.startingAttacks.Length > 0)
                {
                    _currentAttack = Manager.ComboManager.CurrentComboChain.startingAttacks[0];
                }
                else
                {
                    Debug.LogError("No combo chain or starting attacks defined!");
                    return;
                }
            }
            else
            {
                // Continue combo
                _currentAttack = Manager.ComboManager.CurrentAttack;
            }
        
            Manager.ComboManager.SetCurrentAttack(_currentAttack);
            Manager.ComboManager.Animancer.Play(_currentAttack.attackAnimation);
        }
    
        public override void ExitState() { }
    
        public override void UpdateState()
        {
            _stateTime += Time.deltaTime;
            Manager.ComboManager.UpdateComboTimer();
        }
    
        public override ComboState GetNextState()
        {
            if (_stateTime >= _currentAttack.attackData.comboWindowStart)
            {
                return ComboState.ComboWindow;
            }
        
            return StateKey;
        }
    }
}