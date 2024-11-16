using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private PlayerStatsHandler playerStatsHandler;

    private void Awake()
    {
        /*
        dialogueRunner.AddFunction<string, bool>(
            "playerHasItem",
            PlayerHasItem
        );
        */
    }

    /*
    private bool PlayerHasItem(string item)
    {
        bool hasItem = playerStatsHandler.PlayerHasItem(item);
        dialogueRunner.VariableStorage.SetValue("$playerHasItem", hasItem);
        return hasItem;
    }
    */
}
