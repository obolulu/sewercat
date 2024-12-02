using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshPro dialogueText;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeInDuration  = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    
    [Header("Float Settings")]
    [SerializeField] private float floatHeight = 2f;           // Height above NPC
    [SerializeField] private float   amplitude = 0.2f;         // How much additional floating
    [SerializeField] private float   frequency = 1f;           // Float speed
    [SerializeField] private Vector3 offset    = Vector3.zero; // Offset from NPC position

    private Transform npcTransform;
    private Vector3   targetPosition;
    private bool      isDisplaying       = false;
    private float     currentDisplayTime = 0f;

    private void Start()
    {
        npcTransform = transform;
        
        if(!dialogueText) dialogueText = GetComponent<TextMeshPro>();
        
        dialogueText.alpha = 0f;
        dialogueText.gameObject.SetActive(false);
    }


    public void ShowDialogue(string text)
    {

        if (dialogueText != null)
        {
            // Stop any ongoing coroutines
            StopAllCoroutines();

            dialogueText.text = text;
            dialogueText.gameObject.SetActive(true);
            isDisplaying       = true;
            currentDisplayTime = 0f;

            // Start fade in
            StartCoroutine(FadeText(0f, 1f, fadeInDuration));
            
        }
    }
    public void HideDialogue()
    {
        if (dialogueText != null && isDisplaying)
        {
            StartCoroutine(FadeOutAndDisable());
        }
    }
    
    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            dialogueText.alpha = currentAlpha;
            yield return null;
        }
        
        dialogueText.alpha = endAlpha;
    }
    
    private IEnumerator FadeOutAndDisable()
    {
        yield return StartCoroutine(FadeText(1f, 0f, fadeOutDuration));
        
        isDisplaying = false;
        dialogueText.gameObject.SetActive(false);
    }
    
    // Optional: Queue multiple dialogue lines
    public void QueueDialogue(string[] dialogueLines, float delayBetweenLines = 0.5f)
    {
        StartCoroutine(DisplayDialogueQueue(dialogueLines, delayBetweenLines));
    }
    
    private IEnumerator DisplayDialogueQueue(string[] dialogueLines, float delayBetweenLines)
    {
        foreach (string line in dialogueLines)
        {
            ShowDialogue(line);
            yield return new WaitForSeconds(displayDuration + delayBetweenLines);
        }
    }
}