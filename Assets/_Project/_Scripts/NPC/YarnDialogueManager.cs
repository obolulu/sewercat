using UnityEngine;
using Yarn.Unity;
public class YarnDialogueManager : MonoBehaviour
{
    [SerializeField] private YarnProject yarnProject;
    [SerializeField] private string startNode = "test";
    
    [SerializeField] private DialogueRunner dialogueRunner;
    
    private void Start()
    {
    }
    
    public void StartDialogue()
    {
        dialogueRunner?.StartDialogue(startNode);
    }
}