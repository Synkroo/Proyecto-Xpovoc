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

    private HashSet<string> readConversations = new HashSet<string>();

    private Dictionary<string, int> npcConversationIndices = new Dictionary<string, int>();

    private HashSet<string> destroyedObjects = new HashSet<string>();

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

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();

        foreach (var destroyedName in destroyedObjects)
        {
            var obj = GameObject.Find(destroyedName);
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void Start()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
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

        if (npcConversationIndices.TryGetValue(npc.name, out int savedIndex))
            npc.SetConversationIndex(savedIndex);

        var conversation = npc.GetCurrentConversation();

        if (conversation == null)
        {
            Debug.LogWarning("[DIALOGUE] NPC no tiene conversación asignada");
            EndDialogue();
            return;
        }

        readConversations.Add(conversation.name);

        foreach (var line in conversation.conversationLines)
            lines.Enqueue(line);

        if (npc.panelNombre != null)
            npc.panelNombre.SetActive(true);
        if (npc.panelDialogo != null)
            npc.panelDialogo.SetActive(true);

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

    private void NextLine()
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

                npcConversationIndices[currentNpc.name] = currentNpc.GetConversationIndex();

                foreach (var evt in pendingEvents)
                    ExecuteEvent(evt);

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
                pendingEvents.Add(currentLine);
            else
                ExecuteEvent(currentLine);
        }
    }

    private void ExecuteEvent(Line l)
    {
        switch (l.eventType)
        {
            case DialogueEventType.GiveItems:
                if (!string.IsNullOrEmpty(l.optionalParameter) && RewardManager.Instance != null)
                {
                    if (RewardManager.Instance.IsRewardGranted(l.optionalParameter))
                        return;
                }

                if (l.itemsToGive != null)
                {
                    foreach (var reward in l.itemsToGive)
                        InventoryManager.Instance.AddItem(reward.item, reward.amount);

                    InventoryUIManager.Instance?.RefreshAll();
                }

                if (!string.IsNullOrEmpty(l.optionalParameter) && RewardManager.Instance != null)
                    RewardManager.Instance.MarkRewardGranted(l.optionalParameter);

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
                {
                    string objName = currentNpc.gameObject.name;
                    destroyedObjects.Add(objName);
                    Destroy(currentNpc.gameObject);
                }
                break;
        }
    }
}
