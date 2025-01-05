using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ComboTransition
{
    public AttackAnimation nextAttack;
    public ComboInputType  inputType;
    public float           transitionWindow = 0.2f; // Time window to input the combo
}

public enum ComboInputType
{
    LightAttack,
    HeavyAttack,
    Special
}

[CreateAssetMenu(fileName = "AttackAnimation", menuName = "Attack Animation")]
public class AttackAnimation : ScriptableObject
{
    public AnimationClip         attackAnimation;
    public float                 earlyComboWindowStart = 0.6f;
    public float                 comboWindowEnd        = 0.9f;
    public List<ComboTransition> possibleCombos        = new List<ComboTransition>();
    
    private bool           hasBufferedInput;
    private ComboInputType bufferedInputType;
    
    public void BufferInput(ComboInputType inputType)
    {
        hasBufferedInput  = true;
        bufferedInputType = inputType;
    }
    
    public bool TryGetNextAttack(out AttackAnimation nextAttack)
    {
        nextAttack = null;
        if (!hasBufferedInput) return false;
        
        foreach (var combo in possibleCombos)
        {
            if (combo.inputType == bufferedInputType)
            {
                nextAttack = combo.nextAttack;
                return true;
            }
        }
        return false;
    }
    
    public void ConsumeBufferedInput()
    {
        hasBufferedInput = false;
    }
    
    public bool HasBufferedInput => hasBufferedInput;
}