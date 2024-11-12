using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCStateManager npcStateManager;
    public void onInteract()
    {
        npcStateManager.TransitionToState(NPCStateManager.NPCState.Dialogue);
    }
}