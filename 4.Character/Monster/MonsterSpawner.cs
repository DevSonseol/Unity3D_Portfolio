using GlobalEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private string prefabPath = "Monster/";
    public MonsterType monType = MonsterType.None;
    [SerializeField] private int monsterCount = 0;
    int reserveCount = 0;
    [SerializeField] private int keepMonsterCount = 1;
    
    [SerializeField] private float spawnTime;

    public float roaming_Width;
    public float roaming_Height;

    public Vector3[] wanderPos = new Vector3[4];

    public void AddMonsterCount(int value) { monsterCount += value; }
    public void SubMosnterCount(int value) { monsterCount -= value; }

    void Start()
    {
        CalculateWanderPos();
        MakeMonster(monType);
    }

    void Update()
    {
        while (reserveCount + monsterCount < keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    IEnumerator ReserveSpawn()
    {
        reserveCount++;
        yield return new WaitForSeconds(spawnTime);

        MakeMonster(monType);
        reserveCount--;
    }

    void MakeMonster(MonsterType monType)
    {
        Main main = Main.Instance;

        PrefabContainer prefabContainer = PrefabContainer.Instance;
        GameObject ingameMonster;

        if (monType == MonsterType.None)
        {
            Debug.Log("MonsterType None");
            return;
        }
        else
        {
            ingameMonster = main.Instantiate(prefabPath + monType.ToString(), this.transform);
        }

        AddMonsterCount(1);

        NavMeshAgent nma = ingameMonster.GetOrAddComponent<NavMeshAgent>();

        Monster monster = ingameMonster.GetComponent<Monster>();

        if (monster != null)
            monster.Init(this);

        while (true)
        {
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(wanderPos[0], path))
                break;
        }

        ingameMonster.GetComponent<NavMeshAgent>().Warp(wanderPos[0]);

        return;
    }

    //�迭�� ���� 4�� 
    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3 (roaming_Width, 0, roaming_Height));
        #endif
    }

    private void CalculateWanderPos()
    {
        wanderPos[0].x = -(roaming_Width/2);
        wanderPos[0].y = 0;
        wanderPos[0].z = (roaming_Height/2);
        wanderPos[1].x = (roaming_Width/2);
        wanderPos[1].y = 0;
        wanderPos[1].z = (roaming_Height/2);
        wanderPos[2].x = (roaming_Width/2);
        wanderPos[2].y = 0;
        wanderPos[2].z = -(roaming_Height/2);
        wanderPos[3].x = -(roaming_Width/2);
        wanderPos[3].y = 0;
        wanderPos[3].z = -(roaming_Height/2);

        for (int i = 0; i < wanderPos.Length; i++)
            wanderPos[i] = transform.position + wanderPos[i];
    }
}
