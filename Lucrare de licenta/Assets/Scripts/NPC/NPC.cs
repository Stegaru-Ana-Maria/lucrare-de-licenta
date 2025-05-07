using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable, IDialogueNPC
{
    public NPCDialogue dialogueData;

    private int dialogueIndex;
    private bool isDialogueActive;

    public NPCDialogue DialogueData => dialogueData;
    public int DialogueIndex => dialogueIndex;
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.isGamePaused &&!isDialogueActive))
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;
        DialogueManager.Instance.StartDialogue(this);
    }
    void NextLine()
    {
        if (DialogueManager.Instance.IsTyping)
        {
            DialogueManager.Instance.DisplayFullLine();
        }
        else if (++dialogueIndex < dialogueData.dialogLines.Length)
        {
            DialogueManager.Instance.StartDialogue(this);
        }
        else
        {
            EndDialogue();
        }
    }

    void IDialogueNPC.NextLine()
    {
        NextLine();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
    }
}
