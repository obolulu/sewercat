using System;
using _Project._Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;
    
    private NPCStateManager NpcStateManager;

    [SerializeField] private PlayerController psm;
    
    [SerializeField] private GameObject dialogueUI;
    
    public UnityEvent onDialogueComplete;
    

    private void Awake()
    {
        instance = this;
        onDialogueComplete.AddListener(EndDialogue);
    }

    public void StartDialogue(NPCStateManager npcStateManager)
    {
        //dialogueUI.SetActive(true);
        psm.TransitionToState(PlayerController.PlayerState.Locked);
        
        NpcStateManager = npcStateManager;
    }

    public void EndDialogue()
    {
        //dialogueUI.SetActive(false);
        psm.TransitionToState(PlayerController.PlayerState.Idle);
        NpcStateManager.TransitionToState(NPCStateManager.NPCState.Idle);
        //NpcStateManager = null;
    }
}