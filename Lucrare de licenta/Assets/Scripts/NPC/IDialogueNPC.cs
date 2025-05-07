public interface IDialogueNPC
{
    NPCDialogue DialogueData { get; }
    int DialogueIndex { get; }
    void NextLine();
    void EndDialogue();
}
