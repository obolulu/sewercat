using _Project._Scripts;
using UnityEngine;

public class NPCScript : Interactable
{
    [SerializeField] private NPCStateManager npcStateManager;
    public override void onInteract()
    {
        npcStateManager.TransitionToState(NPCStateManager.NPCState.Dialogue);
    }
}