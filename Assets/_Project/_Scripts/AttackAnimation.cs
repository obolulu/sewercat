using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackAnimation
{
    public AnimationClip attackAnimation;
    public float         earlyComboWindowStart = 0.6f;
    public float         comboWindowEnd        = 0.9f;
    
    private bool hasBufferedInput;
    
    public void BufferInput()
    {
        hasBufferedInput = true;
    }
    
    public bool ConsumeBufferedInput()
    {
        bool hadInput = hasBufferedInput;
        hasBufferedInput = false;
        return hadInput;
    }
    
    public bool HasBufferedInput => hasBufferedInput;
}