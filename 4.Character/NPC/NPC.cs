using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character
{
    public Cinemachine.CinemachineVirtualCamera vCam;

    public NPC_Dialogue npc_Dialogue;

    public void StartDialogueNPC()
    {
        if (npc_Dialogue == null) return;

        UIMain uIMain = UIMain.Instance;
        uIMain.Panel_InGame.UpdateUI((int)IngameUIState.Dialogue);
        uIMain.Panel_InGame.IngameUIDialogue.InitDialouge(this);
        vCam.Priority = Main.Instance.cineVirtualCam.Priority + 1;
        vCam.m_Lens.OrthographicSize = 0.5f;
        Main main = Main.Instance;
        main.mainCam.cullingMask -= LayerMask.GetMask("Player");
        main.Player.ChangeState(CharacterState.Idle);
    }

    public void EndDialogueNPC()
    {
        UIMain uIMain = UIMain.Instance;
        uIMain.Panel_InGame.UpdateUI((int)IngameUIState.None);
        vCam.Priority = Main.Instance.cineVirtualCam.Priority - 1;
        vCam.m_Lens.OrthographicSize = 5f;
        Main main = Main.Instance;
        main.mainCam.cullingMask += LayerMask.GetMask("Player");
    }

    void Awake()
    {
        base.Awake();
        Init();
    }

    void start()
    {
        base.Start();
    }

    public void Init()
    {
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ChangeState(curState);
    }

    protected override void BeginAttack()
    {
        this.animator.Play("Attack");
    }

    protected override void BeginDie()
    {
        this.animator.Play("Die");
    }

    protected override void BeginIdle()
    {
        this.animator.Play("Idle");
    }

    protected override void BeginMove()
    {
        this.animator.Play("Move");
    }

    protected override void BeginSpell()
    {
        this.animator.Play("Spell");
    }

    protected override void EndAttack()
    {
        
    }

    protected override void EndDie()
    {
       
    }

    protected override void EndIdle()
    {
        
    }

    protected override void EndMove()
    {
        
    }

    protected override void EndSpell()
    {
        
    }

    protected override void UpdateAttack(float dt)
    {
       
    }

    protected override void UpdateDie(float dt)
    {
        
    }

    protected override void UpdateIdle(float dt)
    {
        
    }

    protected override void UpdateMove(float dt)
    {
        
    }

    protected override void UpdateSpell(float dt)
    {
       
    }

    protected override void BeginStun()
    {

    }

    protected override void UpdateStun(float dt)
    {

    }

    protected override void EndStun()
    {
    }

}
