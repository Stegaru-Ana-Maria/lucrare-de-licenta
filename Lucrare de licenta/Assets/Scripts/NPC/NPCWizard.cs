using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCWizard : MonoBehaviour, IInteractable
{
    [Header("Dialogue Settings")]
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText, intructionText;
    public Image portraitImage;

    [Header("Wizard & Portal")]
    public Animator wizardAnimator;
    public Animator portalAnimator;

    [Header("Book Interaction")]
    private const KeyCode giveBookKey = KeyCode.Q;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

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

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }
    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogLines[dialogueIndex]);
            isTyping = false;
        }
        else if (dialogueIndex < dialogueData.dialogLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }
    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
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