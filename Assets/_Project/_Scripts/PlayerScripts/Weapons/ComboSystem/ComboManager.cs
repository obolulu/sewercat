using UnityEngine;
using Animancer;

namespace _Project._Scripts.PlayerScripts.Weapons.ComboSystem
{
    public class ComboManager: MonoBehaviour
    {
        [SerializeField] private AnimancerComponent   animancer;

        private ComboChainSO _currentComboChain;
        private AttackSO _currentAttack;
        private bool     _isInCombo;
        private float    _comboTimer;
        private int      _currentComboCount;
        
        public bool               IsInCombo         => _isInCombo;
        public AttackSO           CurrentAttack     => _currentAttack;
        public AnimancerComponent Animancer         => animancer;
        public ComboChainSO       CurrentComboChain => _currentComboChain;


        private void Start()
        {
            ResetCombo();
        }
        
        public void SetComboChain(ComboChainSO comboChain)
        {
            _currentComboChain = comboChain;
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
            return !_isInCombo && _currentComboChain != null;
        }
        
        public bool CanContinueCombo()
        {
            return _isInCombo                                                      && 
                   _currentComboCount < _currentComboChain.chainData.maxComboCount &&
                   _comboTimer        < _currentComboChain.chainData.maxComboTime;        
        }
        
        // This will be called by NodeCanvas states
        public void UpdateComboTimer()
        {
            if (_isInCombo)
            {
                _comboTimer += Time.deltaTime;
                if (_comboTimer >= _currentComboChain.chainData.maxComboTime)
                {
                    ResetCombo();
                }
            }
        }
    }
}