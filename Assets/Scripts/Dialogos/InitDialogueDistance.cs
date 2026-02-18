using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitDialogueDistance : MonoBehaviour
{
    [Header("CONVERSACIONES")]
    public List<ConversationTemplate> conversations;

    private int currentConversationIndex = 0;

    [Header("UI DEL NPC")]
    public GameObject panelNombre;
    public GameObject panelDialogo;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI dialogo;

    private bool playerNear = false;

    private void Awake()
    {
        if (conversations == null || conversations.Count == 0)
            Debug.LogWarning($"InitDialogueDistance en {gameObject.name} sin conversaciones");
    }

    public ConversationTemplate GetCurrentConversation()
    {
        if (currentConversationIndex >= conversations.Count)
            return conversations[conversations.Count - 1];

        return conversations[currentConversationIndex];
    }

    public void AdvanceConversation()
    {
        if (currentConversationIndex < conversations.Count - 1)
            currentConversationIndex++;
    }

    public int GetConversationIndex() => currentConversationIndex;

    public void SetConversationIndex(int index)
    {
        currentConversationIndex = Mathf.Clamp(index, 0, conversations.Count - 1);
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
            DialogueManager.Instance.StartDialogue(this);
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
