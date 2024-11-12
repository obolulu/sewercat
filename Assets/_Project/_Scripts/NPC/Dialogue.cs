using UnityEngine;

[System.Serializable]
public class Dialogue
{
    private string _dialogueText;
    private bool _isComplete;
    
    public bool IsComplete()
    {
        return _isComplete;
    }
    
    public void SetComplete()
    {
        _isComplete = true;
    }
}