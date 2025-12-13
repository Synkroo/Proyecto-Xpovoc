using TMPro;
using UnityEngine;

public class InitDialogueDistance : MonoBehaviour
{
    public ConversationTemplate conversation;

    [Header("UI DEL NPC")]
    public GameObject panelNombre;
    public GameObject panelDialogo;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI dialogo;

    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.StartDialogue(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
