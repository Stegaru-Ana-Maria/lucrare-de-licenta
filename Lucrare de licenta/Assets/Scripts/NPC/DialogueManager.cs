using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private IDialogueNPC currentNPC;
    private Coroutine typingCoroutine;
    private bool isTyping;
    public bool IsTyping => isTyping;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(IDialogueNPC npc)
    {
        currentNPC = npc;

        nameText.SetText(npc.DialogueData.npcName);
        portraitImage.sprite = npc.DialogueData.npcPortrait;
        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine());
    }

    public IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");
        string line = currentNPC.DialogueData.dialogLines[currentNPC.DialogueIndex];
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(currentNPC.DialogueData.typingSpeed);
        }

        isTyping = false;

        if (currentNPC.DialogueData.autoProgressLines.Length > currentNPC.DialogueIndex &&
            currentNPC.DialogueData.autoProgressLines[currentNPC.DialogueIndex])
        {
            yield return new WaitForSeconds(currentNPC.DialogueData.autoProgressDelay);
            currentNPC.NextLine();
        }
    }

    public void DisplayFullLine()
    {

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.SetText(currentNPC.DialogueData.dialogLines[currentNPC.DialogueIndex]);
        isTyping = false;
    }

    public void NextLine()
    {
        currentNPC.NextLine();
    }

    public void CloseDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (currentNPC != null)
            currentNPC.EndDialogue();

        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }
}
