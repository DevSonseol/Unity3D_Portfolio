using Sirenix.OdinInspector.Editor.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public partial class Boss : Character
{
    public float angleRange;
    public float radius;

    private Color _red = new Color(1f, 0f, 0f, 0.2f);

    public Projector projectorAtt1;
    public Projector projectorAtt2;

    private IEnumerator RunAttack1()
    {
        float runningTime = 1.0f;

        projectorAtt1.gameObject.SetActive(true);
        projectorAtt1.orthographicSize = 1.0f;

        while (runningTime > .0f)
        {
            runningTime -= Time.deltaTime;
            projectorAtt1.orthographicSize += 7.0f * Time.deltaTime;
            yield return null;
        }
        BossSkill();
        projectorAtt1.gameObject.SetActive(false);
    }

    private IEnumerator RunAttack2()
    {
        float runningTime = 1.0f;
        
        
        projectorAtt2.gameObject.SetActive(true);
        projectorAtt2.orthographicSize = 0.01f;

        while (runningTime > .0f)
        {
            runningTime -= Time.deltaTime;
            projectorAtt2.orthographicSize += 1.0f * Time.deltaTime;
            projectorAtt2.aspectRatio += 10.0f * Time.deltaTime;
            yield return null;
        }

        projectorAtt2.gameObject.SetActive(false);
        
        this.animator.Play("MonRush");
        this.animator.speed = GetStat(Stat.AttackSpeed);
        IsRushReady = true;
        
    }

    private IEnumerator MonRushCol()
    {
        float colTime = 1.5f;
        
        while (colTime > .0f)
        { 
            colTime -= Time.deltaTime;
            yield return null;
        }

        IsStunEnd = true;
    }

    private void OnDrawGizmos()
    {
        Handles.color = _red;
        // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
    }


}
