using System;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.UI
{
    public abstract class Menu: MonoBehaviour, IToggleMenu
    {
        public Action OnMenuOpen;
        private void Awake()
        {
            
        }

        public abstract void SetupMenu();
        public abstract void ToggleMenu();

        protected virtual void MenuOpened()
        {
            OnMenuOpen?.Invoke();
            ToggleMenu();
        }
    }
}