using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[SerializeField]
public struct AnswerPrecondition
{
    public Dictionary<Quest, QuestState> quests;
}

[SerializeField]
public struct AnswerDialogue
{
    public AnswerPrecondition precondition;//사전조건
    public string answer;
    public AferAnswer afterAnswer;
    public int nextIndex; // nextIndex == 99 exit Dialogue
}

[SerializeField]
public struct AferAnswer
{
    public Dictionary<Quest, QuestState> questDatas;
}

[SerializeField]
public class Dialogue
{
    public int dialogueIndex;
    public string question;
    public AnswerDialogue[] answers;
    public Quest quest = null;
    public IngameUIState? ingameUIState = null;
    public bool canExit;
}

[CreateAssetMenu(menuName = ("NPC_Dialogue"))]
public class NPC_Dialogue : SerializedScriptableObject
{
    public List<Dialogue> dialogues;
}
