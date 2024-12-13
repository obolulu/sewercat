using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.UI
{
    public abstract class Menu: MonoBehaviour, IToggleMenu
    {
        public abstract void SetupMenu();
        public abstract void ToggleMenu();
    }
}