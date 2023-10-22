using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum Skills
{ 
    EnergyExplosion,
    FlowerSlash,
    FrontSpikes,
    IceAttack,
    Meteor,
    Count
}

public class Skill : MonoBehaviour
{
    [SerializeField] private Collider collider;

    [SerializeField] private SkillData skillData;
    [SerializeField] private GameObject casterObject;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private float countDelayTimes;
    [SerializeField] private float countNum;

    
    public void InitSkill(SkillData data , GameObject caster , LayerMask Layer , GameObject target = null)
    {
        if (collider == null)
            collider = this.gameObject.GetComponent<SphereCollider>();

        skillData = data;
        casterObject = caster;
        targetLayer = Layer;
        targetObj = target;
        countDelayTimes = 0;
        countNum = 0;
    }

    public void EndSkill()
    {
        Main main = Main.Instance;
        main.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        countDelayTimes += Time.deltaTime;

        if (countNum >= skillData.AttackCounts) return;

        if (countDelayTimes >= skillData.AttackDelayTimes)
        {
            OverlapTargets();
            countDelayTimes =- skillData.AttackDelayTimes;
            countNum++;
            if (countNum >= skillData.AttackCounts) EndSkill();
        }
    }

    private void OverlapTargets()
    {
        Collider[] colliders;
        if (skillData.SkillName == "FrontSpikes")
        {
            FrontSpikesDamage();
            return;
        }
        else if (collider is SphereCollider)
        {
            SphereCollider sphereCollider = collider as SphereCollider;
            colliders = Physics.OverlapSphere(sphereCollider.transform.position, sphereCollider.radius, targetLayer);
            Damage(colliders);
            return;
        }
        else if (collider is BoxCollider)
        {
            BoxCollider boxCollider = collider as BoxCollider;
            colliders = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size, boxCollider.transform.rotation, targetLayer);
            Damage(colliders);
            return;
        }
    }

    private void Damage(params Collider[] colliders)
    {
        foreach (Collider col in colliders)
        {
            Character character = col.GetComponent<Character>();

            if (character == null || character.GetStat(Stat.HP) <= 0) continue;

            character.TakeDamage(CalculateDamage(), casterObject);
            Debug.Log(character.name + ": TakeDamaged");
        }
    }

    private float CalculateDamage()
    {
        //임시로 damage리턴 원래는 시전캐릭터의 능력치 보정해줘야함
        return skillData.SkillDamage;
    }

    private void FrontSpikesDamage()
    {
        bool isCollision = false;
        Vector3 interV = targetObj.transform.position - transform.position;

        // target과 나 사이의 거리가 radius 보다 작다면
        if (interV.magnitude <= casterObject.GetComponent<Boss>().radius)
        {
            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= casterObject.GetComponent<Boss>().angleRange / 2f)
                isCollision = true;
            else
                isCollision = false;

        }
        else
            isCollision = false;

        if (isCollision)
        {
            Character character = targetObj.GetComponent<Character>();
            if (character == null || character.GetStat(Stat.HP) <= 0)
                return;
            character.TakeDamage(CalculateDamage(), casterObject);
            Debug.Log(character.name + ": TakeDamaged");
        }
    }
}
