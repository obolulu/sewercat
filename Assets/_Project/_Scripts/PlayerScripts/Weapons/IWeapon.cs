namespace _Project._Scripts.PlayerScripts.Weapons
{
    public interface IWeapon
    {
        public void TryAttack();
        public void Special();
        public void OnRightClickDown();
        public void OnRightClickUp();
    }
}