using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.NewSystem
{
    public class ClawsSpecialAttackState : ComboStateBase
    {
        private float _stateTime;

        public ClawsSpecialAttackState(ComboState key, ClawsComboStateMachine manager) : base(key, manager)
        {
        }

        public override void EnterState()
        {
            _stateTime = 0f;
            Manager.ComboManager.ResetCombo();
            // Implement special attack logic
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            _stateTime += Time.deltaTime;
        }

        public override ComboState GetNextState()
        {
            // Add appropriate timing for special attack duration
            if (_stateTime >= 1f)
            {
                return ComboState.Idle;
            }

            return StateKey;
        }
    }
}