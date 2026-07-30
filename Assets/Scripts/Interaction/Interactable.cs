using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string interactionPrompt = "ֽאזלטעו E";
    public GameObject promptUI;

    private bool isPlayerNear = false;

    protected virtual void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public abstract void Interact();
}