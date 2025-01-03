using UnityEngine;
using Animancer;

namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    public class ComboManager: MonoBehaviour
    {
        [SerializeField] private ComboChainSO currentComboChain;
        [SerializeField] private AnimancerComponent   animancer;

        private AttackSO _currentAttack;
        private bool     _isInCombo;
        private float    _comboTimer;
        private int      _currentComboCount;
        
        public bool IsInCombo => _isInCombo;
        public AttackSO CurrentAttack => _currentAttack;
        public AnimancerComponent Animancer => animancer;

        private void Start()
        {
            ResetCombo();
        }
        
        public void SetCurrentAttack(AttackSO attack)
        {
            _currentAttack = attack;
        }
        
        public void ResetCombo()
        {
            _isInCombo         = false;
            _comboTimer        = 0;
            _currentComboCount = 0;
            _currentAttack      = null;
        }

        public bool CanStartCombo()
        {
            return !_isInCombo && currentComboChain != null;
        }
        
        public bool CanContinueCombo()
        {
            return _isInCombo && 
                   _currentComboCount < currentComboChain.chainData.maxComboCount &&
                   _comboTimer < currentComboChain.chainData.maxComboTime;        
        }
        
        // This will be called by NodeCanvas states
        public void UpdateComboTimer()
        {
            if (_isInCombo)
            {
                _comboTimer += Time.deltaTime;
                if (_comboTimer >= currentComboChain.chainData.maxComboTime)
                {
                    ResetCombo();
                }
            }
        }
    }
}