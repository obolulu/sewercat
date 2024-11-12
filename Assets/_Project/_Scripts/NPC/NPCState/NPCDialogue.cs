using UnityEngine;

public class NPCDialogue : BaseState<NPCStateManager.NPCState>
{
    private NPCStateManager _stateManager;
    private NPCStateManager.NPCState _stateKey = NPCStateManager.NPCState.Dialogue;
    private YarnDialogueManager _yarnDialogueManager;
    public NPCDialogue(NPCStateManager.NPCState key,YarnDialogueManager yarnDialogueManager,NPCStateManager stateManager) : base(key)
    {
        _stateManager = stateManager;
        _yarnDialogueManager = yarnDialogueManager;
        //_inkStoryManager = inkStoryManager;
        
    }

    public override void EnterState()
    {
        Debug.Log("enmtering dialogue state");
        DialogueUI.instance.StartDialogue(_stateManager);
        _yarnDialogueManager.StartDialogue();
        //_inkStoryManager.StartStory();
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        //Debug.Log("exiting dialogue state");

        //DialogueUI.instance.EndDialogue();
    }

    public override NPCStateManager.NPCState GetNextState()
    {
        return _stateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }
}