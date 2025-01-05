using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.NewSystem
{
    public class ClawsIdleState : ComboStateBase
    {
        public ClawsIdleState(ComboState key, ClawsComboStateMachine manager) : base(key, manager) { }
    
        public override void EnterState()
        {
            if (!Manager.ComboManager.IsInCombo)
            {
                Manager.ComboManager.ResetCombo();
            }
        }
    
        public override void ExitState() { }
    
        public override void UpdateState() { }
    
        public override ComboState GetNextState()
        {
            if (Manager.WeaponHandler.ConsumeAttackInput())
            {
                if (Manager.ComboManager.CanStartCombo())
                {
                    return ComboState.Attack;
                }
            }
            else if (Manager.WeaponHandler.ConsumeSpecialInput())
            {
                return ComboState.SpecialAttack;
            }
        
            return StateKey;
        }
    }
}