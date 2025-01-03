namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    public interface IComboInputHandler
    {
        bool HasAttackInput  { get; }
        bool HasSpecialInput { get; }
        void ConsumeAttackInput();
        void ConsumeSpecialInput();
    }
}