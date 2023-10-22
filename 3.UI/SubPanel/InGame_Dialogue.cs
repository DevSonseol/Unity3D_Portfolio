using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ingame_Dialogue : MonoBehaviour
{
    public NPC_Dialogue dialogueData;
    private NPC npc;
    public NPC Interact_NPC => npc;
    public TMP_Text npcName;
    public TMP_Text question;
    public List<AnswerButton> answerButtons;
    public GameObject dialoguePrefab;

    public void InitDialouge(NPC npc)
    {
        UIMain uimain = UIMain.Instance;
        uimain.Panel_InGame.UpdateUI((int)IngameUIState.Dialogue);
        this.npc = npc;
        dialogueData = npc.npc_Dialogue;
        npcName.text = npc.name;
        LoadDialogue(0);
    }

    public void LoadDialogue(int index)
    {
        if (index == 99)
        {
            EndDialogue();
            return;
        }

        if (dialogueData.dialogues[index].ingameUIState == IngameUIState.Shop)
        {
            UIMain uimain = UIMain.Instance;
            uimain.Panel_InGame.UpdateUI((int)IngameUIState.Shop);
        }

        //question 바꿔주기
        question.text = dialogueData.dialogues[index].question;

        //기존 AnswerButton지우기
        foreach (AnswerButton answerButton in answerButtons)
            Main.Instance.Destroy(answerButton.gameObject);
        answerButtons.Clear();

        //dialogueData에 따라 버튼만들기
        for(int i = 0; i < dialogueData.dialogues[index].answers.Length;++i)
        {
            //사전 조건 체크
            AnswerDialogue answer = dialogueData.dialogues[index].answers[i];
            if (!CheckAnswerPrecondition(answer)) continue;

            GameObject answerbutton = Main.Instance.Instantiate(dialoguePrefab,this.gameObject.transform);
            answerbutton.GetComponent<AnswerButton>().InitAnswerButton(this, answer);
            answerbutton.transform.SetAsLastSibling();
            AnswerButton ab = answerbutton.GetComponent<AnswerButton>();
            answerButtons.Add(ab);
        }

        if (dialogueData.dialogues[index].quest != null)
        {
            Main main = Main.Instance;
            main.RegisterQuest(dialogueData.dialogues[index].quest);
            Debug.Log(dialogueData.dialogues[index].quest.name + "이 등록됨");
        }
    }

    private bool CheckAnswerPrecondition(AnswerDialogue answer)
    {
        Main main = Main.Instance;

        if (answer.precondition.quests == null) return true;

        foreach(KeyValuePair<Quest, QuestState> KV in answer.precondition.quests)
        {
            if(KV.Value == QuestState.Inactive)
            {
                if (main.ContainsInActiveQuests(KV.Key) || main.ContainsInCompleteQuests(KV.Key)) return false;
            }
            else if (KV.Value == QuestState.WaitingForCompletion)
            {
                if (!main.ContainsInActiveQuests(KV.Key) || main.ContainsInCompleteQuests(KV.Key)) return false;

                foreach(Quest activeQuest in main.ActiveQuests)
                {
                    if(activeQuest.CodeName == KV.Key.CodeName)
                    {
                        if(activeQuest.State != KV.Value ) return false;
                    }
                }
            }
        }

        return true;
    }

    private void EndDialogue()
    {
        UIMain uimain = UIMain.Instance;
        npc.EndDialogueNPC();
        uimain.Panel_InGame.UpdateUI((int)IngameUIState.None);
    }
}
