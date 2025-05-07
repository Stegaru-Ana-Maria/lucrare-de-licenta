using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCWizard : MonoBehaviour, IInteractable, IDialogueNPC
{
    [Header("Dialogue Settings")]
    public NPCDialogue dialogueData;
    public TMP_Text intructionText;

    [Header("Wizard & Portal")]
    public Animator wizardAnimator;
    public Animator portalAnimator;

    [Header("Book Interaction")]
    private const KeyCode giveBookKey = KeyCode.Q;

    private int dialogueIndex;
    private bool isDialogueActive;

    public NPCDialogue DialogueData => dialogueData;
    public int DialogueIndex => dialogueIndex;

    public bool CanInteract() => !isDialogueActive;

    void Update()
    {
        if (MagicBook.hasCollectedBook && Input.GetKeyDown(giveBookKey))
        {
            StartCoroutine(PerformSpellSequence());
        }
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.isGamePaused && !isDialogueActive))
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
    IEnumerator PerformSpellSequence()
    {
        wizardAnimator.SetTrigger("spell");

        yield return new WaitForSeconds(0.5f);

        if (portalAnimator != null)
        {
            portalAnimator.SetTrigger("open");
        }

        MagicBook.hasCollectedBook = false;
    }
}