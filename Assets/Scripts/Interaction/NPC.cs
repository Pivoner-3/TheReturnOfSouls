using UnityEngine;

public class NPC : Interactable
{
    [TextArea(3, 5)]
    public string[] dialogueLines;

    private int currentLine = 0;
    private bool isTalking = false;

    public GameObject dialoguePanel;
    public TMPro.TextMeshProUGUI dialogueText;

    public override void Interact()
    {
        if (dialogueLines.Length == 0) return;

        isTalking = true;
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
        currentLine = (currentLine + 1) % dialogueLines.Length;
    }

    public void SkipDialogue()
    {
        dialoguePanel.SetActive(false);
        isTalking = false;
        currentLine = 0;
    }

    void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            SkipDialogue();
        }
    }
}