using GlobalEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MonsterSpawner;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private string prefabPath = "Monster/";
    public MonsterType monType = MonsterType.Boss;
    private int bossCnt = 0;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (bossCnt == 0)
            MakeBossMonster();
    }
    void MakeBossMonster()
    {
        bossCnt++;
        Main main = Main.Instance;

        PrefabContainer prefabContainer = PrefabContainer.Instance;
        GameObject bossMonster;
        bossMonster = main.Instantiate(prefabPath + monType.ToString(), this.transform);

        NavMeshAgent nma = bossMonster.GetOrAddComponent<NavMeshAgent>();
        Boss boss = bossMonster.GetComponent<Boss>();
        if (boss != null)
            boss.Init(this);
        return;
    }
}
