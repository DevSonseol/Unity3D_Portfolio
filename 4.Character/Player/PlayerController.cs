using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public partial class Player : Character
{
    [SerializeField] private bool isDash;
    private float dashDelay = 1f;
    [SerializeField] private float dashCooltime = 5f;
    private Vector3 dashVec;

    void OnKeyActionEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDash)
        {
            ChangeState(CharacterState.Move);
            dashVec = destPos;
            nma.speed = GetStat(Stat.Speed) * 2;
            this.animator.speed = 1;
            this.animator.SetTrigger("doDash");
            isDash = true;

            RaycastHit hit;
            Main main = Main.Instance;
            Ray ray = main.mainCam.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, layerMask);

            Vector3 mousedir = hit.point - this.transform.position;
            mousedir.y = 0;

            nma.destination = this.transform.position + mousedir.normalized * GetStat(Stat.Speed);
            Invoke("DoDashOut", 0.6f);
            return;
        }   


        if(curState != CharacterState.Spell && Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIMain uimain = UIMain.Instance;
            InGame_QuickSlot slot = uimain.Panel_InGame.QuickSlots[0];
            //스킬 UI가 가지고 있는 걸로 시전 해야함
            if(slot.SkillTimer != null && slot.SkillTimer.CanCast && CheckSkillCost(slot.SkillTimer.skillData))
            {
                castingSkillTimer = slot.SkillTimer;
                this.ChangeState(CharacterState.Spell, castingSkillTimer.skillData);
            }
        }

        if (curState != CharacterState.Spell && Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIMain uimain = UIMain.Instance;
            InGame_QuickSlot slot = uimain.Panel_InGame.QuickSlots[1];
            //스킬 UI가 가지고 있는 걸로 시전 해야함
            if (slot.SkillTimer != null && slot.SkillTimer.CanCast && CheckSkillCost(slot.SkillTimer.skillData))
            {
                castingSkillTimer = slot.SkillTimer;
                this.ChangeState(CharacterState.Spell, castingSkillTimer.skillData);
            }
        }

        if (curState != CharacterState.Spell && Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIMain uimain = UIMain.Instance;
            InGame_QuickSlot slot = uimain.Panel_InGame.QuickSlots[2];
            //스킬 UI가 가지고 있는 걸로 시전 해야함
            if (slot.SkillTimer != null && slot.SkillTimer.CanCast && CheckSkillCost(slot.SkillTimer.skillData))
            {
                castingSkillTimer = slot.SkillTimer;
                this.ChangeState(CharacterState.Spell, castingSkillTimer.skillData);
            }
        }

        if (curState != CharacterState.Spell && Input.GetKeyDown(KeyCode.Alpha4))
        {
            UIMain uimain = UIMain.Instance;
            InGame_QuickSlot slot = uimain.Panel_InGame.QuickSlots[3];
            //스킬 UI가 가지고 있는 걸로 시전 해야함
            if (slot.SkillTimer != null && slot.SkillTimer.CanCast && CheckSkillCost(slot.SkillTimer.skillData))
            {
                castingSkillTimer = slot.SkillTimer;
                this.ChangeState(CharacterState.Spell, castingSkillTimer.skillData);
            }
        }

        if (curState != CharacterState.Spell && Input.GetKeyDown(KeyCode.Alpha5))
        {
            UIMain uimain = UIMain.Instance;
            InGame_QuickSlot slot = uimain.Panel_InGame.QuickSlots[4];
            //스킬 UI가 가지고 있는 걸로 시전 해야함
            if (slot.SkillTimer != null && slot.SkillTimer.CanCast && CheckSkillCost(slot.SkillTimer.skillData))
            {
                castingSkillTimer = slot.SkillTimer;
                this.ChangeState(CharacterState.Spell, castingSkillTimer.skillData);
            }
        }

        if (curState != CharacterState.Spell && Input.GetKeyDown(KeyCode.Alpha6))
        {
            UIMain uimain = UIMain.Instance;
            InGame_QuickSlot slot = uimain.Panel_InGame.QuickSlots[5];
            //스킬 UI가 가지고 있는 걸로 시전 해야함
            if (slot.SkillTimer != null && slot.SkillTimer.CanCast && CheckSkillCost(slot.SkillTimer.skillData))
            {
                castingSkillTimer = slot.SkillTimer;
                this.ChangeState(CharacterState.Spell, castingSkillTimer.skillData);
            }
        }


    }

    void DoDashOut()
    {
        nma.speed = GetStat(Stat.Speed);
        isDash = false;
        nma.ResetPath();
        this.animator.speed = 1;
        ChangeState(CharacterState.Idle);
    }

    void OnLeftMouseEvent(GlobalEnum.MouseEvent evt)
    {
        switch (curState)
        {
            case CharacterState.Idle:
                OnMouseEvent_Attack(evt);
                break;
            case CharacterState.Move:
                OnMouseEvent_Attack(evt);
                break;
            case CharacterState.Spell:
                UseSkill();
                break;
        }
    }

    void OnRightMouseEvent(GlobalEnum.MouseEvent evt)
    {
        if (isDash) 
            return;

        switch (curState)
        {
            case CharacterState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case CharacterState.Move:
                OnMouseEvent_IdleRun(evt);
                break;
        }
    }

    void OnMouseEvent_IdleRun(GlobalEnum.MouseEvent evt)
    {
        Main main = Main.Instance;
        RaycastHit hit;
        Ray ray = main.mainCam.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f,_mask);

        switch (evt)
        {
            case GlobalEnum.MouseEvent.RightPointerDown:
                {
                    if (raycastHit)
                    {
                        destPos = hit.point;
                        ChangeState(CharacterState.Move);

                        if (hit.collider.gameObject.layer == (int)GlobalEnum.Layer.Monster)
                        {
                            //몬스터 클릭
                            target = hit.collider.gameObject;
                        }
                        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
                        {
                            //아이템 클릭
                            targetItem = hit.collider.gameObject;
                        }
                        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
                        {
                            //NPC 클릭
                            targetNPC = hit.collider.gameObject;
                            targetNPC.GetComponent<NPC>().StartDialogueNPC();
                        }
                        else
                        {   //바닥 클릭일때
                            GameObject marker = main.Instantiate(PrefabContainer.Instance.MouseMarker);
                            marker.transform.position = destPos;
                        }
                    }
                }
                break;
            case GlobalEnum.MouseEvent.RightPress:
                {
                    if (raycastHit)
                        destPos = hit.point;
                      ChangeState(CharacterState.Move);
                }
                break;
        }
    }

    void OnMouseEvent_Attack(GlobalEnum.MouseEvent evt)
    {
        RaycastHit hit;
        Main main = Main.Instance;
        Ray ray = main.mainCam.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        if (raycastHit)
        {
            Vector3 vec = hit.point - transform.position;
            vec.y = 0;
            transform.rotation = Quaternion.LookRotation(vec).normalized;
        }

        switch (evt)
        {
            case GlobalEnum.MouseEvent.LeftPointerDown:
                ChangeState(CharacterState.Attack); 
                break;
        }
    }

    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Main main = Main.Instance;
        Ray ray = main.mainCam.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(main.mainCam.transform.position, hit.point);
    }
}
