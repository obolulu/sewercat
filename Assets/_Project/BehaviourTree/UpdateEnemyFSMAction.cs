using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace _Project._Scripts.EnemyDir
{
    [Category("Enemy/Combat")]
    public class UpdateEnemyFSMAction : ActionTask
    {
        private EnemyStateMachine _stateMachine;
        
        protected override string OnInit()
        {
            // Cache the reference on initialization
            _stateMachine = agent.GetComponent<EnemyStateMachine>();
            return _stateMachine == null ? "No EnemyStateMachine found!" : null;
        }

        protected override void OnExecute()
        {
            _stateMachine.CustomUpdate();
            EndAction(true);
        }
    }
}