using System;
using UnityEngine;
using UnityEngine.Events;
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;
    
    private NPCStateManager NpcStateManager;

    [SerializeField] private PlayerStateMachine _psm;
    
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
        _psm.TransitionToState(PlayerStateMachine.PlayerState.Locked);
        
        NpcStateManager = npcStateManager;
    }

    public void EndDialogue()
    {
        //dialogueUI.SetActive(false);
        _psm.TransitionToState(PlayerStateMachine.PlayerState.Idle);
        NpcStateManager.TransitionToState(NPCStateManager.NPCState.Idle);
        //NpcStateManager = null;
    }
}