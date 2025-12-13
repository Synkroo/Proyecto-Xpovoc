using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<Line> lines = new Queue<Line>();
    private InitDialogueDistance currentNpc;
    private bool dialogueActive;
    private Line currentLine;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDialogue(InitDialogueDistance npc)
    {
        currentNpc = npc;
        lines.Clear();

        foreach (var l in npc.conversation.conversationLines)
            lines.Enqueue(l);

        npc.panelNombre.SetActive(true);
        npc.panelDialogo.SetActive(true);

        dialogueActive = true;

        // La primera línea
        currentLine = new Line(); // limpio
        NextLine();
    }

    void NextLine()
    {
        // 1. Antes de pasar a la siguiente línea, ejecutamos el evento de la actual
        if (currentLine.eventType != DialogueEventType.None)
            HandleDialogueEvent(currentLine);

        // 2. Si ya no quedan más líneas, terminamos diálogo
        if (lines.Count == 0)
        {
            dialogueActive = false;

            currentNpc.panelNombre.SetActive(false);
            currentNpc.panelDialogo.SetActive(false);

            currentNpc = null;
            return;
        }

        // 3. Pasamos a la siguiente línea
        currentLine = lines.Dequeue();

        // 4. Mostramos en pantalla
        currentNpc.nombre.text = currentLine.speakerName;
        currentNpc.dialogo.text = currentLine.dialogueLine;
    }

    void HandleDialogueEvent(Line l)
    {
        switch (l.eventType)
        {
            case DialogueEventType.ChangeScene:
                if (!string.IsNullOrEmpty(l.optionalParameter))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(l.optionalParameter);
                }
                break;

                // Aquí puedes añadir más eventos futuros:
                // case DialogueEventType.GiveReward:
                // case DialogueEventType.StartCombat:
        }
    }
}
