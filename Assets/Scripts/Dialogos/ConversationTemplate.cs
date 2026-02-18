using System.Collections.Generic;
using UnityEngine;

public enum DialogueEventType
{
    None,
    ChangeScene,
    GiveItems,
    DestroyParent,
}

[System.Serializable]
public struct Line
{
    public string speakerName;
    [TextArea(2, 3)]
    public string dialogueLine;

    public DialogueEventType eventType;

    // Por ejemplo: nombres de escenas o ids de recompensas: "Casa_Cofre", "Casa_NPC1", "Mapa_Anciano_Recompensa"
    public string optionalParameter;

    public List<DialogueItemReward> itemsToGive;
}



[CreateAssetMenu(fileName = "NewConversation", menuName = "Dialogue/Conversation")]
public class ConversationTemplate : ScriptableObject
{
    public List<Line> conversationLines;
}
