using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPCDialogue")]
public class DialogueData : ScriptableObject
{
    public List<Dialogue> dialogues = new List<Dialogue>();
    
    public void AddDialogue(Dialogue dialogue)
    {
        dialogues.Add(dialogue);
    }
    
    public void RemoveDialogue(Dialogue dialogue)
    {
        dialogues.Remove(dialogue);
    }
    
    public List<Dialogue> GetDialogues()
    {
        return dialogues;
    }

    public Dialogue getNextDialogue()
    {
        foreach (Dialogue dialogue in dialogues)
        {
            if (!dialogue.IsComplete())
            {
                dialogue.SetComplete();
                return dialogue;
            }
        }

        return dialogues[^1];
    }
    
    private Dialogue GetDialogue(int index)
    {
        return dialogues[index];
    }
}