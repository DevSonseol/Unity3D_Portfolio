using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnswerButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Ingame_Dialogue Ingame_Dialogue;
    public Button button;
    public TMPro.TMP_Text answerText;
    [SerializeField] private int nextIndex;
    [SerializeField] private AnswerDialogue answerDialogue;

    public void InitAnswerButton(Ingame_Dialogue ingameDialogue  , AnswerDialogue answerData)
    {
        Ingame_Dialogue = ingameDialogue;
        answerDialogue = answerData;
        answerText.text = answerData.answer;
        answerText.color = Color.gray;
        this.nextIndex = answerData.nextIndex;
    }

    public void OnClickAnswerButton()
    {
        Ingame_Dialogue.LoadDialogue(nextIndex);

        if (answerDialogue.afterAnswer.questDatas != null)
            CheckAfterAnswer();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        answerText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        answerText.color = Color.gray;
    }

    private void CheckAfterAnswer()
    {
        Main main = Main.Instance;

        Dictionary<Quest,QuestState> questDatas = answerDialogue.afterAnswer.questDatas;

        foreach(KeyValuePair<Quest, QuestState> KV in questDatas)
        {
            //KV.value(QuestState) == Running 은 퀘스트 수락
            if(KV.Value == QuestState.Running )
            {
                main.RegisterQuest(KV.Key);
                continue;
            }
            //KV.value(QuestState) == Complete 은 퀘스트 완료 및 보상
            if(KV.Value == QuestState.Complete)
            {
                main.CompleteWaitingQeust(KV.Key);
                RewardQuest(KV.Key);
                continue;
            }
        }
    }

    private void RewardQuest(Quest questData)
    {

    }
}
