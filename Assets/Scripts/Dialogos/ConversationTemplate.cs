using System.Collections.Generic;
using UnityEngine;

public enum DialogueEventType
{
    None,
    ChangeScene,
    // Más eventos en el futuro: GiveReward, StartCombat, etc.
}

[System.Serializable]
public struct Line
{
    public string speakerName;
    [TextArea(2, 3)]
    public string dialogueLine;

    public DialogueEventType eventType;
    public string optionalParameter;  // Parámetro opcional (ej. nombre de escena)
}

[CreateAssetMenu(fileName = "NewConversation", menuName = "Dialogue/Conversation")]
public class ConversationTemplate : ScriptableObject
{
    public List<Line> conversationLines;
}
