namespace _Project._Scripts.PlayerScripts.Weapons.Claws.NewSystem
{
    public abstract class ComboStateBase: BaseState<ComboState>
    {
        protected readonly ClawsComboStateMachine Manager;
    
        protected ComboStateBase(ComboState key, ClawsComboStateMachine manager) : base(key)
        {
            Manager = manager;
        }
    }
}