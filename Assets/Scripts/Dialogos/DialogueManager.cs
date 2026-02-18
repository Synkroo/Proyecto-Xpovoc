using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<Line> lines = new Queue<Line>();
    private InitDialogueDistance currentNpc;
    private bool dialogueActive;
    public bool IsDialogueActive => dialogueActive;
    private Line currentLine;
    private List<Line> pendingEvents = new List<Line>();

    public PlayerController playerController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
            NextLine();
    }

    public void StartDialogue(InitDialogueDistance npc)
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();

        if (playerController != null)
        {
            playerController.canMove = false;
            playerController.StopMovement();
        }

        currentNpc = npc;
        lines.Clear();
        pendingEvents.Clear();

        var conversation = npc.GetCurrentConversation();

        foreach (var line in conversation.conversationLines)
            lines.Enqueue(line);

        if (npc != null)
        {
            if (npc.panelNombre != null)
                npc.panelNombre.SetActive(true);
            if (npc.panelDialogo != null)
                npc.panelDialogo.SetActive(true);
        }

        dialogueActive = true;

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        NextLine();
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        if (playerController != null)
            playerController.canMove = true;
        currentNpc = null;
    }

    public void CancelDialogue()
    {
        dialogueActive = false;
        lines.Clear();
        pendingEvents.Clear();
        if (playerController != null)
            playerController.canMove = true;
        currentNpc = null;
    }

    void NextLine()
    {
        if (lines.Count == 0)
        {
            dialogueActive = false;

            if (currentNpc != null)
            {
                if (currentNpc.panelNombre != null)
                    currentNpc.panelNombre.SetActive(false);
                if (currentNpc.panelDialogo != null)
                    currentNpc.panelDialogo.SetActive(false);

                currentNpc.AdvanceConversation();

                foreach (var evt in pendingEvents)
                {
                    ExecuteEvent(evt);
                }
                pendingEvents.Clear();
            }

            if (playerController != null)
                playerController.canMove = true;

            currentNpc = null;
            return;
        }

        currentLine = lines.Dequeue();

        if (currentNpc != null)
        {
            if (currentNpc.nombre != null)
                currentNpc.nombre.text = currentLine.speakerName;
            if (currentNpc.dialogo != null)
                currentNpc.dialogo.text = currentLine.dialogueLine;
        }

        if (currentLine.eventType != DialogueEventType.None)
        {
            if (currentLine.eventType == DialogueEventType.DestroyParent)
            {
                // Guardamos el evento para ejecutarlo al pasar la línea
                pendingEvents.Add(currentLine);
            }
            else
            {
                ExecuteEvent(currentLine);
            }
        }
    }

    void ExecuteEvent(Line l)
    {
        switch (l.eventType)
        {
            case DialogueEventType.GiveItems:
                if (l.itemsToGive != null)
                {
                    foreach (var reward in l.itemsToGive)
                    {
                        InventoryManager.Instance.AddItem(reward.item, reward.amount);
                        InventoryUIManager.Instance?.RefreshAll();
                    }
                }
                break;

            case DialogueEventType.ChangeScene:
                string sceneName = l.optionalParameter;
                if (!string.IsNullOrEmpty(sceneName))
                {
                    if (sceneName == "Combate" && InventoryUIManager.Instance != null)
                        InventoryUIManager.Instance.isCombatInventory = true;

                    SceneManager.LoadScene(sceneName);
                }
                break;

            case DialogueEventType.DestroyParent:
                if (currentNpc != null)
                    Destroy(currentNpc.gameObject);
                break;
        }
    }
}
