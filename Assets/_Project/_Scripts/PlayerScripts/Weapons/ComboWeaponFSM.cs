namespace _Project._Scripts.PlayerScripts.Weapons
{
    public abstract class ComboWeaponFSM<EState>: StateManager<EState>, IComboWeapon where EState : System.Enum
    {
        public abstract void TryAttack();
        public abstract void Special();
        public abstract void OnRightClickDown();
        public abstract void OnRightClickUp();
        public abstract void ResetWeaponState();
    }
}